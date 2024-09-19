using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ats.Domain.Identity.Models;
using Ats.Models.Account;
using Ats.Models.User;
using Ats.Security.WebSecurity;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Ats.Web.Controllers
{
    [Authorize]
    public class ProfilesController : BaseController
    {
        private IAccountService _accountService;
        private IAdminManagementService _adminMngService;
        private IConfiguration _config;
        private int pageSize;

        public ProfilesController(UserManager<User> userManage,
                                  SignInManager<User> signInManager,
                                  ILoggerManager logger,
                                  IAccountService accountService, IAdminManagementService adminMngService, IConfiguration config) : base(userManage, signInManager, logger)
        {
            _accountService = accountService;
            _adminMngService = adminMngService;
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        //[AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_MENU_ORGANIZATION_USERS)]
        public IActionResult Index(string searchText, int? pageIndex)
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchWithPaging(string searchText, int pageIndex)
        {
            return RedirectToAction("Index", new { searchText = searchText, pageIndex = pageIndex });
        }

        public IActionResult Edit(Guid id)
        {
            if (TempData["newPassword"] != null)
            {
                ViewBag.NewPw = TempData["newPassword"].ToString();
                TempData.Remove("newPassword");
            }
            if (TempData["success"] != null)
            {
                ViewBag.StatusMessage = TempData["success"].ToString();
                TempData.Remove("success");
            }
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMessage = TempData["error"].ToString();
                TempData.Remove("error");
            }

            var roleGroups = this._accountService.GetAllRoleGroups().AsEnumerable();
            IEnumerable<SelectListItem> items = roleGroups.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.RoleGroups = items;


            if (id == null || id == Guid.Empty)
            {
                return View(new UserProfileModel { GroupId = roleGroups.FirstOrDefault(x => x.Name == Ats.Commons.Constants.SYS_USER_GROUP_MEMBER).Id });
            }
            else
            {
                var user = this._accountService.GetUser(id);
                //var departments = this._adminMngService.GetUserDeparments(id);

                var userView = new ProfileModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                };
                
                return View(userView);
            }
        }
    }
}