using Ats.Commons;
using Ats.Domain.Departments.Models;
using Ats.Domain.Identity.Models;
using Ats.Models.Department;
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
    public class DepartmentController : BaseController
    {
        private IDepartmentService _departmentService;
        private IAccountService _accountService;
        private IAdminManagementService _adminMngService;
        private IConfiguration _config;
        public DepartmentController(UserManager<User> userManage, IDepartmentService departmentService, IAdminManagementService adminMngService, IConfiguration config,
                        IAccountService accountService, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _departmentService = departmentService;
            _accountService = accountService;
            _adminMngService = adminMngService;
            _config = config;
        }

        #region Department
        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Department Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            DepartmenttSearchViewModel model = new DepartmenttSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var departments = _departmentService.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.Departments = departments;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = departments.Count();

            #region Select list
            var managers = _accountService.GetActiveUsers();
            var slManagers = managers.ToList().Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
            ViewBag.Managers = new SelectList(slManagers, "Value", "Text");
            #endregion

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
        public ActionResult Search(string keyword, int pageIndex)
        {
            return RedirectToAction("Index", new { search = keyword, pageIndex = pageIndex });
        }
        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Create(DepartmenttSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Department (Name: {model.Department.Name})");
            _departmentService.CreateDepartment(model.Department);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(Guid id)
        {
            _logger.LogDebug($"Department Detail(Id: {id})");
            var model = new DepartmenttViewModel();
            var entity = _departmentService.GetDepartment(id);
            if (entity != null) model = entity;

            #region Select list
            var managers = _accountService.GetActiveUsers();
            var slManagers = managers.ToList().Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
            ViewBag.Managers = new SelectList(slManagers, "Value", "Text");
            #endregion

            #region Status message
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            if (TempData["addUserSuccess"] != null)
            {
                ViewBag.StatusUserMessage = TempData["addUserSuccess"].ToString();
                TempData.Remove("addUserSuccess");
            }
            if (TempData["updateUserSuccess"] != null)
            {
                ViewBag.StatusUserMessage = TempData["updateUserSuccess"].ToString();
                TempData.Remove("updateUserSuccess");
            }
            if (TempData["deleteUserSuccess"] != null)
            {
                ViewBag.StatusUserMessage = TempData["deleteUserSuccess"].ToString();
                TempData.Remove("deleteUserSuccess");
            }
            #endregion

            return View("Edit", model);
        }
        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(DepartmenttViewModel model)
        {
            _logger.LogDebug($"Edit Department(Id: {model.Id})");
            var entity = _departmentService.GetDepartment(model.Id);
            if (entity != null)
            {
                _departmentService.UpdateDepartment(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }         
            return RedirectToAction("Detail", new { Id = model.Id });
        }
        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Department(Id: {id})");
            var entity = _departmentService.GetDepartment(id);
            if (entity != null)
            {
                _departmentService.DeleteDepartment(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully + " " + Resource.Common_label_department + ": " + entity.Name;
            }
            
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        #endregion

        #region Department users
        //[AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        //[AuthorizeRoles(RoleName.SYS_ROLE_DEPARTMENTSUSER_MANAGEMENT_CREATE)]
        [HttpGet] 
        public IActionResult AddDepartmentUser(Guid id)
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            var users = this._accountService.GetActiveUsers();
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
                ViewBag.ErrorMessage = Resource.Common_errorMessage_noUser;
            }
            return View(model);
        }

        //[AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        [HttpPost] 
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
                            var users = _accountService.GetActiveUsers();
                            IEnumerable<SelectListItem> items = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
                            model.Items = items;

                            ViewBag.ErrorMessage = Resource.Common_errorMessage_errorCompareDate;
                            return View(model);
                        }
                        _adminMngService.AddDepartmentUser(departmentId, userId, role, model.StartDate, model.EndDate);
                        TempData["addUserSuccess"] = Resource.Common_notify_addUserSuccessfully;
                        return RedirectToAction("Detail", new { id = model.Id });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Resource.Common_errorMessage_userAlreadyExisted;
                        return RedirectToAction("AddDepartmentUser", new { id = model.Id });
                    }
                }
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

        //[AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
        //[AuthorizeRoles(RoleName.SYS_ROLE_DEPARTMENTSUSER_MANAGEMENT_EDIT)]
        public IActionResult EditDepartmentUser(Guid id)
        {
            var users = this._accountService.GetActiveUsers();
            var detail = this._adminMngService.GetDepartmentUserDetail(id);

            detail.userList = users.Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });

            if (users.Count() == 0)
            {
                ViewBag.ErrorMessage = Resource.Common_errorMessage_noUser;
            }
            return View(detail);
        }

        [HttpPost]
        //[AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)]
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

                    ViewBag.ErrorMessage = Resource.Common_errorMessage_errorCompareDate;
                    return View(model);
                }

                _adminMngService.UpdateDepartmentUser(model);
                TempData["updateUserSuccess"] = Resource.Common_notify_updateUserSuccessfully;
                return RedirectToAction("Detail", new { id = model.DepartmentId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(model);
            }
        }

        //[AuthorizeRoles(Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_ALL, Ats.Commons.Constants.SYS_ROLE_DEPARTMENT_MANAGEMENT_EDIT)] 
        //[AuthorizeRoles(RoleName.SYS_ROLE_DEPARTMENTSUSER_MANAGEMENT_DELETE)]
        public IActionResult RemoveDepartmentUser(Guid id, Guid did)
        {
            if (id != Guid.Empty && did != Guid.Empty)
            {
                _adminMngService.DeleteDepartmentUser(did, id);
                TempData["deleteUserSuccess"] = Resource.Common_notify_deleteUserSuccessfully;
            }            
            return RedirectToAction("Detail", new { id = did });
        }
        #endregion
    }
}