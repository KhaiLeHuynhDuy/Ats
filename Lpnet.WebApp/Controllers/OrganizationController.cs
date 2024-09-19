using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Domain.Organization.Models;
using Ats.Models;
using Ats.Models.Organization;
using Ats.Services;
using Ats.Services.Extensions;
using Lpnet.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lpnet.WebApp.Resources;
using Ats.Services.Interfaces;
using static Ats.Commons.Constants;
using Ats.Security.WebSecurity;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class OrganizationController : BaseController
    {
        private IOrganizationService _organizationService;
        private IWarehouseService _warehouseService;
        private IConfiguration _config;
        public OrganizationController(UserManager<User> userManage, IOrganizationService organizationService, IConfiguration config,
                         IWarehouseService warehouseService, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _organizationService = organizationService;
            _warehouseService = warehouseService;
            _config = config;
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, ORGANIZATION_TYPE? type, string field, bool sort, int page)
        {
            _logger.LogDebug($"Organization Index Search={search}, Type={type}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            OrganizationSearchViewModel model = new OrganizationSearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var organizations = _organizationService.Search(model.Keyword, type, field, sort, page, pageSize, out int count);
            model.Organizations = organizations;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = organizations.Count();

            ViewBag.OrganizationType = Ultility.EnumToSelectList<ORGANIZATION_TYPE>();
            ViewBag.OrganizationTypeAdd = Ultility.EnumToSelectList<ORGANIZATION_TYPE>(false);

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
        public ActionResult Search(string keyword, ORGANIZATION_TYPE? type, int pageIndex)
        {
            return RedirectToAction("Index", new { search = keyword, type = type, pageIndex = pageIndex });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Create(OrganizationSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Organization (Name: {model.Organization.Name})");
            _organizationService.CreateOrganization(model.Organization);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        [AuthorizeRoles( RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(Guid id)
        {
            _logger.LogDebug($"Organization Detail(Id: {id})");
            var model = new OrganizationViewModel();
            var entity = _organizationService.GetOrganization(id);
            if (entity != null) model = entity;
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            ViewBag.OrganizationType = Ultility.EnumToSelectList<ORGANIZATION_TYPE>(false);
            ViewBag.WarehouseType = Ultility.EnumToSelectList<WAREHOUSE_TYPE>(false);

            return View("Edit", model);
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(OrganizationViewModel model)
        {
            _logger.LogDebug($"Edit Organization(Id: {model.Id})");
            var entity = _organizationService.GetOrganization(model.Id);
            if (entity != null)
            {
                _organizationService.UpdateOrganization(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Detail", new { Id = model.Id });
        }


        [AuthorizeRoles( RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Organization(Id: {id})");
            var entity = _organizationService.GetOrganization(id);
            if (entity != null)
            {
                _organizationService.DeleteOrganization(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }


        [HttpPost]
        public ActionResult CreateWarehouseOrg(OrganizationViewModel model)
        {
            _logger.LogDebug($"Create Warehouse (Name: {model.Warehouse.Name})");
            _warehouseService.CreateWarehouse(model.Warehouse);
            return RedirectToAction("Detail", new { Id = model.Warehouse.OrganizationId });
        }
        public ActionResult DeleteWarehouseOrg(Guid id, Guid orgId)
        {
            _logger.LogDebug($"Delete Warehouse (Id: {id})");
            var entity = _warehouseService.GetWarehouse(id);
            if (entity != null) _warehouseService.DeleteWarehouse(id);
            return RedirectToAction("Detail", new { id = orgId });
        }
    }
}