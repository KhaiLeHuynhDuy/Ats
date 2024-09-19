using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ats.Services;
using static Ats.Commons.Constants;
using Ats.Domain.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;

namespace Ats.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IAccountService _accountService;
        private IAdminManagementService _adminMngService;

        public HomeController(UserManager<User> userManage, SignInManager<User> signInManager,
            IAccountService accountService, IAdminManagementService adminMngService, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            this._accountService = accountService;
            this._adminMngService = adminMngService;
        }

        public IActionResult Index()
        {
            var userId = GetUserIdLogin();
            _logger.LogInfo($"Index- userId: '{userId}'");            

            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public IActionResult Admin()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
