using Common;
using Common.Utilities;
using Data.Repositories;
using Entities;
using Makta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Email;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace Makta.Areas.SitePublic.Controllers
{
    [Area("SitePublic")]
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Setting> _settingRepository;

        public HomeController(IEmailSender emailSender, IRepository<Setting> settingRepository)
        {
            _emailSender = emailSender;
            _settingRepository = settingRepository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var _settings = await _settingRepository.TableNoTracking.FirstOrDefaultAsync(cancellationToken);
            ViewData["_settings"] = _settings;
            return View("ComingSoon");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult BlogDetails()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Subscribe(string email)
        {
            try
            {
                if (!StringHelper.IsEmailValid(email))
                    return new JsonResult("Please enter a valid email address.");

                _emailSender.SendNewSubscribe(email);

                _emailSender.SaveSubscribeData($"subscribe - {email}");

                return new JsonResult("You are in! Thanks for being a part of Makta community.");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        public IActionResult joinus()
        {
            return View();
        }

        [HttpPost]
        public IActionResult submitjoinus(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return new JsonResult(new ReturnClass { Description = "Please fill the form and try again.", IsSucceed = false, ButtonText = "Got it!" });

                var model = JsonConvert.DeserializeObject<JoinUsDto>(data);
                if (model == null)
                    return new JsonResult(new ReturnClass { Description = "something went wrong, please try again later.", IsSucceed = false, ButtonText = "Sure!" });

                if (!StringHelper.IsEmailValid(model.Email))
                    return new JsonResult(new ReturnClass { Description = "Please enter a valid email address.", IsSucceed = false, ButtonText = "ok let me check!" });

                _emailSender.SendSubscribeEmailtoAdmin(data);

                _emailSender.SaveSubscribeData($"joinus - {data}");
                return new JsonResult(new ReturnClass { Description = "Thanks for being a part of Makta community. A community member will process your request and will contact you no more than 2 business days.", IsSucceed = true, ButtonText = "Cool, thanks!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}