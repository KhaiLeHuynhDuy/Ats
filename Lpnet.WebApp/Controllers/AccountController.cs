using Ats.Models.Email;
using Ats.Security.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ats.Domain.Identity.Models;
using Ats.Service.EmailEngine;
using System.Text.Encodings.Web;
using Ats.Services.Extensions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<User> userManage,
                                 SignInManager<User> signInManager, IEmailSender emailSender, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _emailSender = emailSender;
        }

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return await RedirectToLocal("/Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            //var userId = _userManager.GetUserId(HttpContext.User);
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                // This doesn't count login failures towards account lockout
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        //var claims = new List<Claim>
                        //                {
                        //                  new Claim(ClaimTypes.Name, user.Email)
                        //                };

                        //var claimsIdentity = new ClaimsIdentity(
                        //  claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //var authProperties = new AuthenticationProperties();

                        //await HttpContext.SignInAsync(
                        //  CookieAuthenticationDefaults.AuthenticationScheme,
                        //  new ClaimsPrincipal(claimsIdentity),
                        //  authProperties);

                        return await RedirectToLocal("/");
                    }
                }

                ModelState.AddModelError("Error", "Username or password is incorrect.");
                return View(model);

                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion Login

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();           
            return RedirectToAction(nameof(HomeController.Index), "Dashboard");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return await RedirectToLocal(returnUrl);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                        // Send an email with this link
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                        ConfirmAccountModel msgmodel = new ConfirmAccountModel
                        {
                            ToAddress = user.Email,
                            UserName = user.Email,
                            UrlConfirmAccount = callbackUrl
                        };
                        var msg = await _emailSender.BuildMailMessageAsync("~/Views/Email/ConfirmAccount.cshtml", msgmodel);
                        await _emailSender.SendAsync(msg);

                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        return View("ConfirmEmailRegister");
                    }
                    AddErrors(result);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(action: nameof(AccountController.ResetPassword),
                                        controller: "Account",
                                        values: new { user.Id, code },
                                        protocol: Request.Scheme);

                ResetPasswordModel msgmodel = new ResetPasswordModel
                {
                    ToAddress = user.Email,
                    UserName = user.Email,
                    UrlResetPassword = callbackUrl,
                };
                var msg = await _emailSender.BuildMailMessageAsync("~/Views/Email/ResetPassword.cshtml", msgmodel);
                await _emailSender.SendAsync(msg);

                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("/");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public async Task<IActionResult> AccessDenied()
        { 
            return View();
        }
    }
}