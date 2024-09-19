using Ats.Models.Email;
using Ats.Security.Models.Account;
using Ats.WebApp.Extensions;
using Ats.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ats.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ats.Domain.Identity.Models;
using Ats.Service.EmailEngine;
using System.Text.Encodings.Web;
using Ats.Security.WebSecurity;
using Microsoft.Extensions.Configuration;
using Ats.Web.Resources;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ats.Models;
using Ats.Web.Models;
using Ats.Models.Department;
using Ats.Domain.Departments.Models;
using Ats.Commons.Utilities;
using Ats.Services.Extensions;
using Ats.Commons;
using Ats.Domain.Address;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ats.Services.Interfaces;

namespace Ats.Web.Controllers
{
    public class AdminController : BaseController
    {
        private IAddressService _addressService;
        private IAccountService _accountService;
        private IAdminManagementService _adminMngService;
        private IConfiguration _config;

        public AdminController(UserManager<User> userManage,
                                SignInManager<User> signInManager,
                                IConfiguration config,
                                IAddressService addressService,
                                IAccountService accountService,
                                IAdminManagementService adminMngService, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            this._addressService = addressService;
            this._accountService = accountService;
            this._adminMngService = adminMngService;
            this._config = config;
        }

        [AuthorizeRoles(Roles = Ats.Commons.Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public IActionResult Index()
        {
            return View();
        }
      
        #region Department
        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL)]
        public IActionResult Departments(string searchText, int? pageIndex)
        {
            int pageSize = _config.GetValue<int>("PageSize");

            DepartmentSearchViewModel model = new DepartmentSearchViewModel()
            {
                SearchText = searchText,
                PageIndex = pageIndex != null && pageIndex != 0 ? pageIndex.Value : 1,
                PageSize = pageSize
            };
            var departments = this._adminMngService.SearchDepartmentWithPaging(model.SearchText, model.PageIndex);
            model.Departments = departments.Departments;
            model.TotalItem = departments.TotalItem;

            return View(model);
        }
        public IActionResult SearchDepartmentWithPaging(string searchText, int pageIndex)
        {
            return RedirectToAction("Departments", new { searchText = searchText, pageIndex = pageIndex });
        }
        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        public IActionResult EditDepartment(Guid id)
        {
            var users = this._accountService.GetActiveUsers();
            IEnumerable<SelectListItem> items = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
            ViewBag.ActiveUsers = items;

            if (id == null || id == Guid.Empty)
            {
                return View();
            }
            if (TempData["success"] != null)
            {
                ViewBag.StatusMessage = TempData["success"].ToString();
                TempData.Remove("success");
            }
            var model = this._adminMngService.GetDepartment(id);
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        public IActionResult EditDepartment(DepartmentViewModel model)
        {
            var users = this._accountService.GetActiveUsers();
            IEnumerable<SelectListItem> items = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
            ViewBag.ActiveUsers = items;
            try
            {
                ModelState.Remove("Manager.Email");
                ModelState.Remove("Manager.UserCode");
                ModelState.Remove("Manager.UserName");
                ModelState.Remove("Manager.FirstName");
                if (ModelState.IsValid)
                {
                    if (model.Id != Guid.Empty)
                    {
                        this._adminMngService.UpdateDepartment(model);
                        TempData["success"] = Resource.SaveSuccessfully;
                    }
                    else
                    {
                        model.Id = this._adminMngService.CreateDepartment(model);
                        TempData["success"] = Resource.SaveSuccessfully;
                    }
                    return RedirectToAction("EditDepartment", new { id = model.Id });
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                if (model.Id != Guid.Empty)
                {
                    var depart = this._adminMngService.GetDepartment(model.Id);
                    model.Users = depart.Users;
                    return View(model);
                }
                return View();
            }
        }

        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_DELETE)]
        public IActionResult DeleteDepartment(Guid id, int pageIndex)
        {
            this._adminMngService.DeleteDerpartment(id);
            return RedirectToAction("Departments", new { pageIndex = pageIndex });
        }

        [HttpGet]
        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        public IActionResult AddDepartmentUser(Guid id)
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            var users = this._accountService.GetActiveUsers();

            //var users = this._adminMngService.GetNotSelectedDepartmentUser(id);

            IEnumerable<SelectListItem> items = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });

            var model = new DepartmentUserSelectionViewModel<SelectListItem>()
            {
                Id = id,
                SelectedId = Guid.Empty,
                Items = items,
                Role = DEPARTMENT_ROLE.UNDEFINED,
                StartDate = DateTime.Now,
                EndDate = null
            };
            if (users.Count() == 0)
            {
                ViewBag.ErrorMessage = Resource.NoUser;
            }
            return View(model);
        }

        [HttpPost]
        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        public IActionResult AddDepartmentUser(DepartmentUserSelectionViewModel<SelectListItem> model, string StartDate, string EndDate)
        {
            try
            {
                if (model.SelectedId != Guid.Empty)
                {
                    
                    var departmentId = model.Id;
                    var userId = model.SelectedId;  
                    var role = model.Role;
                    if (!_adminMngService.CheckExistAddDepartmentUser(departmentId, userId))
                    {
                        ModelState.Remove("StartDate");
                        ModelState.Remove("EndDate");

                        model.StartDate = StartDate.StringToDate();
                        model.EndDate = EndDate.StringToNullableDate();

                        if (model.StartDate > model.EndDate)
                        {
                            var users = this._accountService.GetActiveUsers();
                            IEnumerable<SelectListItem> items = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
                            model.Items = items;

                            ViewBag.ErrorMessage = Resource.ErrorCompareDate;
                            return View(model);
                        }
                        this._adminMngService.AddDepartmentUser(departmentId, userId, role, model.StartDate, model.EndDate);
                        return RedirectToAction("EditDepartment", new { id = model.Id });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Resource.UserAlreadyExists;
                        return RedirectToAction("AddDepartmentUser", new { id = model.Id });
                    }
                }
                ViewBag.ErrorMessage = Resource.UserIsEmpty;
                return View(model);

            }
            catch (Exception ex)
            {
                var users = this._accountService.GetActiveUsers();
                IEnumerable<SelectListItem> items = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
                model.Items = items;

                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }

        }
        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        public IActionResult EditDepartmentUser(Guid id)
        {
            var users = this._accountService.GetActiveUsers();
            var detail = this._adminMngService.GetDepartmentUserDetail(id);
            //var users = this._adminMngService.GetNotSelectedUser(id);

            //IEnumerable<SelectListItem> items = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
            detail.userList = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });

            if (users.Count() == 0)
            {
                ViewBag.ErrorMessage = Resource.NoUser;
            }
            return View(detail);
        }

        [HttpPost]
        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        public IActionResult EditDepartmentUser(DepartmentUserEditModel model, string StartDate, string EndDate)
        {
            try
            {
                ModelState.Remove("StartDate");
                ModelState.Remove("EndDate");

                model.StartDate = StartDate.StringToDate();
                model.EndDate = EndDate.StringToNullableDate();
                if (model.StartDate > model.EndDate)
                {
                    var users = this._accountService.GetActiveUsers();
                    model.userList = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });

                    ViewBag.ErrorMessage = Resource.ErrorCompareDate;
                    return View(model);
                }
 
                this._adminMngService.UpdateDepartmentUser(model);
                return RedirectToAction("EditDepartment", new { id = model.DepartmentId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }

        [AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        public IActionResult RemoveDepartmentUser(Guid id, Guid did)
        {
            this._adminMngService.DeleteDepartmentUser(did, id);

            return RedirectToAction("EditDepartment", new { id = did });
        }
        #endregion

    }
}