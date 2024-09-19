using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Ats.Services;
using Microsoft.AspNetCore.Identity;
using Ats.Domain.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Ats.Web.Resources;
using Ats.Models.Account;
using Ats.Services.Extensions;
using Ats.Security.WebSecurity;
using Ats.Services.Interfaces;

namespace Ats.Web.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private IAccountService _accountService;

        public ManageController(UserManager<User> userManage,
                                SignInManager<User> signInManager,
                                IAccountService accountService, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            this._accountService = accountService;
        }

        //
        // GET: /Manage/Index
        public async Task<IActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = GetUserIdLogin();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            //var model = new IndexViewModel
            //{
            //    HasPassword = await HasPassword(),
            //    PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
            //    TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
            //    Logins = await _userManager.GetLoginsAsync(user),
            //};

            return View();
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var userId = GetUserIdLogin();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            ManageMessageId? message;
            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }


        //
        // GET: /Manage/ChangePassword
        public IActionResult ChangePassword()
        {
            if (TempData["success"] != null)
            {
                ViewBag.StatusMessage = TempData["success"].ToString();
                TempData.Remove("success");
            }
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = GetUserIdLogin();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["success"] = Resource.ChangePwSuccessfully;
                return RedirectToAction("ChangePassword");
            }
            else
            {

                if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
                {
                    ModelState.AddModelError("incorrect", Resource.IncorrectPw);
                    return View(model);
                }
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            var userId = GetUserIdLogin();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //User profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(UserProfileModel user)
        {
            ModelState.Remove("UserCode");
            ModelState.Remove("UserName");
            if (!ModelState.IsValid)
            {
                return View("EditProfile", user);
            }
            var userId = GetUserIdLogin();
            //this._accountService.SetUser(userId);
            var userList = this._accountService.GetAllUsers().Any(x => x.Email.ToLower().Equals(user.Email.ToLower()) && !user.Id.Equals(x.Id));
            if (userList)
            {
                ModelState.AddModelError("EmailExist", Resource.EmailAlreadyExists);
                return View("EditProfile", user);
            }

            var id = this._accountService.EditUserProfile(user);
            ViewBag.StatusMessage = Resource.SaveSuccessfully;
            TempData["success"] = ViewBag.StatusMessage;
            return View(user);
        }

        public IActionResult EditProfile(Guid id)
        {
            if (TempData["success"] != null)
            {
                ViewBag.StatusMessage = TempData["success"].ToString();
                TempData.Remove("success");
            }
            var userId = GetUserIdLogin();
            var user = this._accountService.GetUser(id);
            return View(user);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsryIf";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private async Task<bool> HasPassword()
        {
            var userId = GetUserIdLogin();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private async Task<bool> HasPhoneNumber()
        {
            var userId = GetUserIdLogin();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}