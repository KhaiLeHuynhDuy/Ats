using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Account;
using Ats.Models.User;
using Ats.Security.WebSecurity;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Models;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private IAccountService _accountService;
        private IUserService _userService;
        private ICommonService _commonService;
        private IConfiguration _config;
        public UserController(UserManager<User> userManage, IUserService userService, IConfiguration config, ICommonService commonService,
                          IAccountService accountService, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _accountService = accountService;
            _userService = userService;
            _commonService = commonService;
            _config = config;
        }
        #region User 
        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"User Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            UserrSearchViewModel model = new UserrSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var users = _userService.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.Users = users;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = users.Count();

            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["deleteSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["deleteSuccess"].ToString();
                TempData.Remove("deleteSuccess");
            }
            if (TempData["addError"] != null)
            {
                ViewBag.ErrorMessage = TempData["addError"].ToString();
                TempData.Remove("addError");
            }
            #endregion

            return View(model);
        }
        public ActionResult Search(string keyword, int pageIndex)
        {
            return RedirectToAction("Index", new { search = keyword, pageIndex = pageIndex });
        }
        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]

        [HttpPost]
        public ActionResult Create(UserrSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create User (UserName: {model.User.UserName})");
            var roleGroups = _accountService.GetAllRoleGroups();
            model.User.GroupId = roleGroups.FirstOrDefault(x => x.Name == Constants.SYS_USER_GROUP_MEMBER).Id;
            //Check existed
            if (!_userService.CheckExistUserNameOrEmail(model.User))
            {
                _userService.CreateUser(model.User);
                TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            }
            else TempData["addError"] = Resource.Common_errorMessage_userNameOrEmailExisted;
            
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(Guid id, string pw)
        {
            _logger.LogDebug($"User Detail(Id: {id})");
            var model = new UserrProfileViewModel();

            #region Select list
            var roleGroups = _accountService.GetAllRoleGroups();
            var slRoleGroups = roleGroups.ToList().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.RoleGroups = new SelectList(slRoleGroups, "Value", "Text");
            if (pw != null) ViewBag.StatusMessagePassword = pw;
            #endregion

            var entity = _userService.GetUser(id);
            if (entity != null)
            {
                entity.Roles = _accountService.GetUserRoles(id);
                if (entity.GroupId == Guid.Empty)
                {
                    entity.GroupId = roleGroups.FirstOrDefault(x => x.Name == Constants.SYS_USER_GROUP_MEMBER).Id;
                }
                model = entity;
                if (TempData["updateSuccess"] != null)
                {
                    ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                    TempData.Remove("updateSuccess");
                }
            }

            return View("Edit", model);
        }
        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(UserrProfileViewModel model, string dob)
        {
            _logger.LogDebug($"Edit User(Id: {model.Id})");
            var entity = _userService.GetUser(model.Id);
            if (entity != null)
            {
                if (dob != null) model.DOB = Ultility.StringToDate(dob);
                _userService.UpdateUser(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Detail", new { Id = model.Id });
        }
        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(Guid userId)
        {
            string newPassword = Ultility.CreatePassword(8);


            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (passwordChangeResult.Succeeded)
                {
                    string pw = $"{Resource.Common_message_passwordChanged}: {newPassword}";
                    return RedirectToAction("Detail", new { id = userId, pw = pw });
                }
            }          

            return RedirectToAction("Detail", new { id = userId });
        }
        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete User(Id: {id})");
            var entity = _userService.GetUser(id);
            if (entity != null)
            {
                _userService.DeleteUser(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully + " " + Resource.Common_label_username + ": " + entity.UserName;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        #endregion

        #region Role group

        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult RoleGroup(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Role group Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            RoleGrouppSearchViewModel model = new RoleGrouppSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var roleGroups = _userService.SearchRoleGroup(model.Keyword, field, sort, page, pageSize, out count);
            model.RoleGroups = roleGroups;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = roleGroups.Count();

            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["deleteSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["deleteSuccess"].ToString();
                TempData.Remove("deleteSuccess");
            }
            #endregion

            return View(model);
        }
        public ActionResult SearchRoleGroup(string search, int pageIndex)
        {
            return RedirectToAction("RoleGroup", new { search = search, pageIndex = pageIndex });
        }
        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult CreateRoleGroup(RoleGrouppSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Role group(Name: {model.RoleGroup.Name})");
            _userService.CreateRoleGroup(model.RoleGroup);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;

            return RedirectToAction("RoleGroup", new { pageIndex = pageIndex });
        }

        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult DetailGroup(Guid id)
        {
            if (id == null || id == Guid.Empty) return View();
            var model = _accountService.GetRoleGroup(id);

            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }

            return View("EditGroup", model);
        }
        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult EditGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = new BaseResponse<GroupViewModel>();
                response = model.Id == Guid.Empty ? _accountService.CreateRoleGroup(model).Result : _accountService.UpdateRoleGroup(model).Result;
                if (response.Success)
                {
                    TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                    return RedirectToAction("DetailGroup", new { id = model.Id });
                }
                else
                {
                    //ModelState.AddModelError("Error", response.Errors.FirstOrDefault());
                    model.ListRoles = _accountService.GetRoles(model.Roles.Select(x => x.Id).ToList());
                    return View(model);
                }
            }
            else
            {
                return View();
            }
        }

        [AuthorizeRoles(Constants.SYS_ROLE_USER_MANAGEMENT, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult DeleteRoleGroup(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Role group(Id: {id})");
            var entity = _userService.GetRoleGroup(id);
            if (entity != null)
            {            
                _userService.DeleteRoleGroup(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully + " " + Resource.Common_label_roleGroups + ": " + entity.Name;
            }              
            return RedirectToAction("RoleGroup", new { pageIndex = pageIndex });
        }
        #endregion
    }
}