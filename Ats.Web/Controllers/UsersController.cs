using Ats.Models.Account;
using Ats.Models.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Ats.Security.WebSecurity;
using Ats.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Ats.Domain.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ats.Web.Resources;
using Ats.Domain.Accounts.Models;
using System.Threading.Tasks;
using Ats.Models;
using Ats.Services.Extensions;
using Newtonsoft.Json;
using Ats.Commons;
using Ats.Services.Interfaces;

namespace Ats.Web.Controllers
{
    [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_SYSTEM_ADMINISTRATION, Ats.Commons.Constants.SYS_ROLE_USER_MANAGEMENT)]
    public class UsersController : BaseController
    {
        private IAccountService _accountService;
        private IAdminManagementService _adminMngService;
        private IConfiguration _config;
        private int pageSize;

        public UsersController(UserManager<User> userManage,
                                SignInManager<User> signInManager,
                                IConfiguration config,
                                IAccountService accountService,
                                IAdminManagementService adminMngService,
                                ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            this._accountService = accountService;
            this._adminMngService = adminMngService;
            this._config = config;

            pageSize = _config.GetValue<int>("PageSize");
        }

        // GET: Users
        public IActionResult Index(string searchText, int? pageIndex)
        {
            UserSearchViewModel model = new UserSearchViewModel()
            {
                SearchText = searchText,
                PageIndex = pageIndex != null && pageIndex != 0 ? pageIndex.Value : 1,
                PageSize = pageSize
            };

            //var users = this._accountService.Search();
            var users = this._accountService.SearchWithPaging(model.SearchText, model.PageIndex);
            model.UsersProfile = users.UsersProfile;
            model.TotalItem = users.TotalItem;

            return View(model);
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

            var roleGroups = this._accountService.GetAllRoleGroups();
            IEnumerable<SelectListItem> items = roleGroups.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.RoleGroups = items;


            if (id == null || id == Guid.Empty)
            {
                return View(new UserProfileModel { GroupId = roleGroups.FirstOrDefault(x => x.Name == Ats.Commons.Constants.SYS_USER_GROUP_MEMBER).Id });
            }
            else
            {
                var user = this._accountService.GetUser(id);
                user.Roles = this._accountService.GetUserRoles(id);

                if (user.GroupId == Guid.Empty)
                {
                    user.GroupId = roleGroups.FirstOrDefault(x => x.Name == Ats.Commons.Constants.SYS_USER_GROUP_MEMBER).Id;
                }
               
                return View(user);
            }
        }

        public IActionResult View(Guid id)
        {
            var roleGroups = this._accountService.GetAllRoleGroups();
            IEnumerable<SelectListItem> items = roleGroups.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.RoleGroups = items;

       
            if (id == null || id == Guid.Empty)
            {
                return View(new UserProfileModel { GroupId = roleGroups.FirstOrDefault(x => x.Name == Ats.Commons.Constants.SYS_USER_GROUP_MEMBER).Id });
            }
            else
            {
                var user = this._accountService.GetUser(id);
                user.Roles = this._accountService.GetUserRoles(id);

                if (user.GroupId == Guid.Empty)
                {
                    user.GroupId = roleGroups.FirstOrDefault(x => x.Name == Ats.Commons.Constants.SYS_USER_GROUP_MEMBER).Id;
                }

                return View(user);
            }
        }

        [HttpPost]
        //[MultipleButton(Name = "action", Argument = "Save")]
        public IActionResult Edit(UserProfileModel model,string StartDate, string JoiningDate, string ResignationDate, string DateOfBirth)
        {
            model.StartDate = StartDate.StringToNullableDate();
            model.JoiningDate = JoiningDate.StringToNullableDate();
            model.ResignationDate = ResignationDate.StringToNullableDate();
            model.DateOfBirth = DateOfBirth.StringToNullableDate();

            var roleGroups = this._accountService.GetAllRoleGroups();
            IEnumerable<SelectListItem> items = roleGroups.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.RoleGroups = items;

            //if (!ModelState.IsValid)
            //{
            //    var message = ViewData.ModelState.Values.Where(x => x.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid).Select(k => k.Errors[0].ErrorMessage).FirstOrDefault();
            //    ModelState.AddModelError("Error", message);
            //    return View("edit", model);
            //}
            var userId = GetUserIdLogin();
            //this._accountService.SetUser(userId);
            var response = new BaseResponse<UserProfileModel>();

            if (model.Id == Guid.Empty)
            {
                response = _accountService.CreateUser(model).Result;
            }
            else
            {
                response = _accountService.UpdateUserProfile(model).Result;
            }

            if (response.Success)
            {
                TempData["success"] = Resource.SaveSuccessfully;

                return RedirectToAction("Edit", new { id = response.Data.Id });
            }
            else
            {
                ModelState.AddModelError("Error", response.Errors.FirstOrDefault());
               // TempData["error"] = 
                return View(model);
            }
        }
        public IActionResult Delete(Guid id, int pageIndex)
        {
            var departList = this._adminMngService.GetDepartmentUser(id).ToList();
            foreach (var depart in departList)
            {
                var ddd = depart.Value;
                this._adminMngService.DeleteDepartmentUser(Guid.Parse(depart.Value), id);
            }

            this._accountService.DeleteUser(id);
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        [HttpGet]
        //[MultipleButton(Name = "action", Argument = "ResetPwd")]
        public async Task<IActionResult> ResetPassword(Guid userId)
        {
            string newPassword = Ats.Commons.Ultility.CreatePassword(8);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                // Don't reveal that the user does not exist
                TempData["error"] = Resource.UserNotFound;
                return RedirectToAction("Edit", new { id = userId });
            }
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (passwordChangeResult.Succeeded)
            {
                TempData["newPassword"] = String.Format("Password changed: {0}", newPassword);
                //ViewBag.StatusMessagePassword = String.Format("Password changed:{0}", newPassword);
            }
            else
            {
                TempData["error"] = passwordChangeResult.Errors.FirstOrDefault()?.Description;
                foreach (var error in passwordChangeResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return RedirectToAction("Edit", new { id = userId });
        }

        public IActionResult RoleGroups(string searchText, int? pageIndex)
        {
            //var roleGroups = this._accountService.GetAllRoleGroups();
            RoleGroupSearchViewModel model = new RoleGroupSearchViewModel()
            {
                SearchText = searchText,
                PageIndex = pageIndex != null && pageIndex != 0 ? pageIndex.Value : 1,
                PageSize = pageSize
            };
            var roleGroups = this._accountService.GetRoleGroupWithPaging(model.SearchText, model.PageIndex);
            model.RoleGroups = roleGroups.RoleGroups;
            model.TotalItem = roleGroups.TotalItem;

            return View(model);
        }
        public IActionResult RoleGroupsWithPaging(string searchText, int pageIndex)
        {
            return RedirectToAction("RoleGroups", new { searchText = searchText, pageIndex = pageIndex });
        }
        public IActionResult EditRoleGroup(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return View();
            }
            if (TempData["success"] != null)
            {
                ViewBag.StatusMessage = TempData["success"].ToString();
                TempData.Remove("success");
            }
            var user = this._accountService.GetRoleGroup(id);  
            return View(user);
        }

        [HttpPost]
        public IActionResult EditRoleGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = new BaseResponse<GroupViewModel>();
                response = model.Id == Guid.Empty ? _accountService.CreateRoleGroup(model).Result : _accountService.UpdateRoleGroup(model).Result;               
                if (response.Success)
                {
                    TempData["success"] = Resource.SaveSuccessfully;
                    return RedirectToAction("EditRoleGroup", new { id = model.Id });
                }
                else
                {
                    ModelState.AddModelError("Error", response.Errors.FirstOrDefault());
                    model.ListRoles = _accountService.GetRoles(model.Roles.Select(x => x.Id).ToList());
                    return View(model);
                }
            }
            else
            {
                return View();
            }
        }
        public IActionResult DeleteRoleGroup(Guid id, int pageIndex)
        {
            this._accountService.DeleteGroup(id);
            return RedirectToAction("RoleGroups", new { pageIndex = pageIndex });
        }
           
    }
}