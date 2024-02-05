using AutoMapper;
using Common;
using Common.Utilities;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Makta.Areas.SiteUser.Controllers
{
    [Area("SiteUser")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        private readonly IRepository<Setting> _styleRepository;
        private readonly CommonSetting _commonSettings;

        private readonly IEmailSender _emailSender;
        public HomeController(
                              IUserRepository userRepository,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IMapper mapper, IRepository<Setting> styleRepository, CommonSetting commonSettings, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;

            _styleRepository = styleRepository;
            _commonSettings = commonSettings;

            _emailSender = emailSender;
        }

        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        public async Task<ActionResult> SiteStyling(CancellationToken cancellationToken)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveSiteStyling(Microsoft.AspNetCore.Http.IFormCollection data, CancellationToken cancellationToken)
        {
            try
            {
                var companyInfo = data["companyInfo"];

                var existingImageFiles = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data["existingImages"]);

                Dictionary<string, string> imageFiles = new Dictionary<string, string>();

                foreach (var file in data.Files)
                {
                    try
                    {
                        var result = await FileUtility.Upload(file);
                        if (result.HasValue())
                        {
                            imageFiles.Add(file.Name, "/upload/" + result);
                        }
                        else
                        {
                            var message = "File did not uploaded correctly. please upload safe files and check your connection.";
                            ModelState.AddModelError("PictureFile", message);
                            throw new Exception(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                foreach (var existingImage in existingImageFiles)
                {
                    imageFiles.Add(existingImage.Key, existingImage.Value);
                }

                var imageFileJson = Newtonsoft.Json.JsonConvert.SerializeObject(imageFiles);

                Setting siteStyle = new Setting();

                await _styleRepository.AddAsync(siteStyle, cancellationToken, true);

            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }

            return new JsonResult("File uploaded successfully");
        }

        public async Task<ActionResult> EmailBlast(CancellationToken cancellationToken)
        {
            return View();
        }
        public async Task<ActionResult> SendEmailBlast(Microsoft.AspNetCore.Http.IFormCollection data, CancellationToken cancellationToken)
        {
            try
            {
                var emailInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data["emailInfo"]);

                var subjectLine = emailInfo["subjectLine"];
                var emailBody = emailInfo["emailBody"];

                var userList = _userRepository.TableNoTracking.Where(p => p.EmailBlast == true).ToList<ApplicationUser>();

                foreach (var user in userList)
                {
                    //Services.Email.EmailMessage message = new Services.Email.EmailMessage(from, toAddresses, subjectLine, emailBody);
                    _emailSender.SendEmailFromAdmin(user, subjectLine, emailBody);
                }

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }
    }
}