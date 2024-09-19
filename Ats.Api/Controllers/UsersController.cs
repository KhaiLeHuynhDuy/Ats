using System;
using Ats.Api.Authentication;
using Ats.Api.Model;
using Ats.Api.Response;
using Ats.Service.EmailEngine;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ats.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ILoggerManager _logger;
        public UsersController(UserManager<Domain.Identity.Models.User> userManage, SignInManager<Domain.Identity.Models.User> signInManager,
            IConfiguration configuration, IEmailSender emailSender, ILoggerManager logger) : base(userManage, signInManager)
        {
            _configuration = configuration;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public BaseResponse<object> Login([FromBody] LoginViewModel model)
        {
            var rs = new BaseResponse<object>();
            var rsLogin = new LoginReponse();
            try
            {
                var user = _userManager.FindByEmailAsync(model.Email).Result;
                if (user != null)
                {
                    var result = _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, lockoutOnFailure: false).Result;
                    if (result.Succeeded)
                    {
                        TokenProvider _tokenProvider = new TokenProvider();
                        var secretKey = _configuration.GetSection("SecretKey").Value;
                        var domain = _configuration.GetSection("Domain").Value;
                        var userToken = _tokenProvider.LoginUser(user.Email.Trim(),
                                        user.PasswordHash.Trim(), user.Id, secretKey, domain);
                        if (string.IsNullOrEmpty(userToken))
                            rs.Message = "Can not generate token!";
                        else
                        {
                            rsLogin.AccessToken = userToken;
                            rsLogin.Email = user.Email;

                            rs.Data = rsLogin;
                            rs.Success = true;
                            rs.Message = "Success";
                            return rs;
                        }
                    }
                    else
                        rs.Message = "Password is incorrect!";
                }
                else
                    rs.Message = "Not Found User";
            }
            catch (Exception ex)
            {
                rs.Message = ex.Message;
                rs.Success = false;
                _logger.LogDebug("Login bug: " + ex.Message);
            }
            return rs;
        }
    }
}