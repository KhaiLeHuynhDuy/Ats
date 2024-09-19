using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ats.Domain.Identity.Models;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Ats.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected UserManager<User> _userManager;
        protected SignInManager<User> _signInManager;

        public BaseController(UserManager<User> userManage)
        {
        }

        public BaseController(UserManager<User> userManage, SignInManager<User> signInManager)
        {
            _userManager = userManage;
            _signInManager = signInManager;
        }
        protected string GetUserIdLogin()
        {
            return User.FindFirstValue(ClaimTypes.Name);        
        }
        protected string GetUserEmailLogin()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        protected Guid GetUserIdLogin_Guid()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            Guid.TryParse(userId, out var id);
            return id;
        }
    }
}