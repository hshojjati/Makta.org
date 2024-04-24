using Common;
using Data.Repositories;
using Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSetting _emailSetting;
        private const string BaseFullLogPath = "wwwroot\\Logs";
        private readonly SMSSetting _smsSettings;
        private readonly IRepository<Setting> _settingRepository;
        private readonly Setting _settings;

        public EmailSender(EmailSetting emailSetting, SMSSetting smsSettings, IRepository<Setting> settingRepository)
        {
            _emailSetting = emailSetting;
            _smsSettings = smsSettings;

            if (!Directory.Exists(BaseFullLogPath))
            {
                Directory.CreateDirectory(BaseFullLogPath);
            }
            _settingRepository = settingRepository;

            _settings = _settingRepository.TableNoTracking.FirstOrDefault();
        }

        private async Task SendEmail(EmailMessage message)
        {
            message.From = new MailboxAddress(_emailSetting.FromName, _emailSetting.FromEmail);
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);
            //await Log(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(message.From);
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.Content,
                TextBody = ""
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.CheckCertificateRevocation = false;

                await client.ConnectAsync(_emailSetting.SmtpServer, _emailSetting.Port, SecureSocketOptions.Auto);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailSetting.UserName, _emailSetting.Password);
                await client.SendAsync(mailMessage);

                var fromaddress = mailMessage.From[0] as MailboxAddress;

                foreach (var item in mailMessage.To)
                {
                    var toAddress = item as MailboxAddress;
                    LogEmail("Email Sent successfully from: " + fromaddress.Address + " to: " + toAddress.Address);

                }

            }
            catch (Exception ex)
            {
                LogEmail(ex.Message);

            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        //--------------------
        public async Task SendContactUsMessage(ContactUs contactUs)
        {
            try
            {
                MailboxAddress from = new MailboxAddress(contactUs.SenderName, contactUs.SenderEmail);

                string subject = "You have a Contact-Us Message (safe!!)";

                MailboxAddress to1 = new MailboxAddress(" Info", "email@site.com");

                List<MailboxAddress> toAddresses = new List<MailboxAddress>
                {
                    to1
                };

                string body = "";
                body += "Dear <b>Admin</b>,";
                body += $"<br>You have a contact-us form submitted in :{DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm tt")}";
                body += $"<br><br>From: <b>{contactUs.SenderName} ({contactUs.SenderEmail})</b>";
                body += $"<br>Phone: <b>{contactUs.SenderPhone}</b>";
                body += $"<br>Message: <b>{contactUs.Message}</b>";

                EmailMessage message = new EmailMessage(toAddresses, subject, body);

                await SendEmail(message);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendForgotPasswordEmail(ApplicationUser user, string returnUrl)
        {
            try
            {
                string subject = "Forgot Password";

                string body = @"Dear " + user.FirstName +
                    @",<br>Please reset your password by <a href = '" + returnUrl + "' > clicking here </a>.";

                EmailMessage message = new EmailMessage(new List<MailboxAddress> { new MailboxAddress(user.Email) }, subject, body);
                await SendEmail(message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendEmailFromAdmin(ApplicationUser user, string subject, string content)
        {
            try
            {
                EmailMessage message = new EmailMessage(new List<MailboxAddress> { new MailboxAddress(user.Email) }, subject, content);

                await SendEmail(message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetDeepPropertyValue(object src, string propName)
        {
            if (src == null)
                return string.Empty;
            if (propName.Contains('.'))
            {
                string[] Split = propName.Split('.');
                string RemainingProperty = propName.Substring(propName.IndexOf('.') + 1);
                return GetDeepPropertyValue(src.GetType().GetProperty(Split[0])?.GetValue(src, null), RemainingProperty);
            }
            else
                return src.GetType().GetProperty(propName)?.GetValue(src, null)?.ToString() ?? string.Empty;
        }

        public async Task LogSystem(string logDescription)
        {
            await File.AppendAllTextAsync(BaseFullLogPath + "\\systemlog.txt", DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") + " - " + logDescription + Environment.NewLine);
        }

        public async Task LogPayment(string logDescription)
        {
            await File.AppendAllTextAsync(BaseFullLogPath + "\\paylog.txt", DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") + " - " + logDescription + Environment.NewLine);
        }

        private void LogEmail(string logMessage)
        {
            File.AppendAllText(BaseFullLogPath + "\\Emaillog.txt", DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss") + " - " + logMessage + Environment.NewLine);
        }

        private async Task SendSMS(string DestinatonNumber, string body)
        {
            try
            {
                var ClientNo = _smsSettings.ClientNo;
                var APIPassword = _smsSettings.APIPassword;
                var ClientIDHash = _smsSettings.ClientIDHash;
                var LineNo = _smsSettings.LineNo;
                LineNo = new string(LineNo.Where(c => char.IsDigit(c)).ToArray());
                DestinatonNumber = new string(DestinatonNumber.Where(c => char.IsDigit(c)).ToArray());

                var url = $"{_smsSettings.Url}?";
                var urlParameters = $"?APIusr={ClientNo}&APIpwd={APIPassword}&ClientIDHashed={ClientIDHash}&source={LineNo}&destination={DestinatonNumber}&MsgDirection=0&type=SMS&message={body}";

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(urlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsStringAsync();
                    Console.WriteLine("{0}", dataObjects);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                client.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        public async Task SendVerificationSMS(string phoneNumber, string code)
        {
            var EncodedMessage = System.Web.HttpUtility.UrlEncode($"Verification code: {code}");
            SendSMS(phoneNumber, EncodedMessage);
        }

        public async Task SendVerificationEmail(string emailAddress, string code)
        {
            try
            {
                string subject = $"No-Reply -- Verification code";
                string body = "";
                body += $"Verification code: {code}";

                var to = new List<MailboxAddress> { new MailboxAddress(emailAddress) };

                //-------------
                foreach (var item in to)
                {
                    BodyBuilder bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = body,
                        TextBody = ""
                    };

                    var emailMessage = new MimeMessage();
                    emailMessage.To.Add(item);
                    emailMessage.Subject = subject;

                    emailMessage.Body = bodyBuilder.ToMessageBody();

                    //send the email
                    Send(emailMessage);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task SendNewSubscribe(string email)
        {
            try
            {
                if (_settings == null)
                {
                    LogEmail("Settings not defined!");
                    return;
                }

                string subject = "New Makta subscriber";

                MailboxAddress to1 = new MailboxAddress($"{_settings.CommunityName} Community", _settings.CommunityEmail);

                List<MailboxAddress> toAddresses = new List<MailboxAddress>
                {
                    to1
                };

                string body = "";
                body += "Dear <b>Makta Admin</b>,";
                body += $"<br>You have a new subscriber enrolled in :{DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm tt")} UTC time";
                body += $"<br>Email: <b>{email}</b>";
                body += $"<br><br>{_settings.CommunityName} Auto e-Mailing System";

                EmailMessage message = new EmailMessage(toAddresses, subject, body);

                await SendEmail(message);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendSubscribeEmailtoAdmin(string body)
        {
            try
            {
                if (_settings == null)
                {
                    LogEmail("Settings not defined!");
                    return;
                }



                string subject = "New Makta Contributer";

                MailboxAddress to1 = new MailboxAddress(_settings.CommunityName + " Community", _settings.CommunityEmail);

                List<MailboxAddress> toAddresses = new List<MailboxAddress>
                {
                    to1
                };

                string bodyText = "";
                bodyText += "Dear <b>Makta Admin</b>,";
                bodyText += $"<br>You have a new community member request at :{DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm tt")} UTC time";
                bodyText += $"<br>Info: <b>{body}</b>";
                bodyText += $"<br><br>{_settings.CommunityName} Auto e-Mailing System";

                EmailMessage message = new EmailMessage(toAddresses, subject, bodyText);

                await SendEmail(message);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveSubscribeData(string data)
        {
            await File.AppendAllTextAsync(BaseFullLogPath + "\\requests.txt", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " - " + data + Environment.NewLine);
        }
    }
}