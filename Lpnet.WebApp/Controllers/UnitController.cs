using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lpnet.WebApp.Resources;
using Ats.Domain.Identity.Models;
using Ats.Models.Unit;
using Ats.Services;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ats.Services.Interfaces;
using static Ats.Commons.Constants;
using Ats.Security.WebSecurity;
using Ats.Commons;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class UnitController : BaseController
    {
        private IUnitService _service;
        private IConfiguration _config;
        public UnitController(UserManager<User> userManage, IUnitService productCatService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _service = productCatService;
            _config = config;
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Unit Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            UnitSearchViewModel model = new UnitSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var productCategorys = _service.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.Units = productCategorys;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productCategorys.Count();

            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
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

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Create(UnitSearchViewModel model)
        {
            //_logger.LogDebug($"Create (Name: {model.ProductCategory.Name})");
            _service.CreateUnit(model.Unit);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index");
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            //_logger.LogDebug($"Occupation Detail(Id: {id})");
            var model = new UnitSearchViewModel();
            var entity = _service.GetUnit(id);
            if (entity != null) model.Unit = entity;
            return PartialView("_Detail", model);
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(UnitSearchViewModel model)
        {
            _logger.LogDebug($"Edit Unit(Id: {model.Unit.Id})");
            var entity = _service.GetUnit(model.Unit.Id);
            if (entity != null)
            {
                _service.UpdateUnit(model.Unit);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Index");
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Unit(Id: {id})");
            var entity = _service.GetUnit(id);
            if (entity != null)
            {
                _service.DeleteUnity(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index");
        }
    }
}
