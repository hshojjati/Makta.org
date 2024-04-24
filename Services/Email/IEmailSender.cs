using Common;
using Entities;
using System.Threading.Tasks;

namespace Services.Email
{
    public interface IEmailSender
    {
        Task SendContactUsMessage(ContactUs contactUs);
        Task SendForgotPasswordEmail(ApplicationUser user, string returnUrl);
        Task SendEmailFromAdmin(ApplicationUser user, string subject, string content);
        Task LogSystem(string logDescription);
        Task SendVerificationSMS(string phoneNumber, string code);
        Task SendVerificationEmail(string emailAddress, string code);
        Task SendNewSubscribe(string email);
        Task SendSubscribeEmailtoAdmin(string body);
        Task SaveSubscribeData(string data);
    }
}
