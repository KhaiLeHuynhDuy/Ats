using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Warehouse;
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
using Ats.Domain.Organization.Models;
using Ats.Domain.Loan.Models;
using Ats.Services.Interfaces;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class WarehouseController : BaseController
    {
        private IWarehouseService _warehouseService;
        private IAssetService _assetService;
        private ICommonService _commonService;
        private IConfiguration _config;
        public WarehouseController(UserManager<User> userManage, IWarehouseService warehouseService, IAssetService assetService, IConfiguration config,
                      ICommonService commonService, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _assetService = assetService;
            _warehouseService = warehouseService;
            _commonService = commonService;
            _config = config;
        }
        [HttpGet]
        public ActionResult Index(string search, WAREHOUSE_TYPE? type, string field, bool sort, int page)
        {
            _logger.LogDebug($"Warehouse Index Search={search}, Type={type}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            WarehouseSearchViewModel model = new WarehouseSearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var warehouses = _warehouseService.Search(model.Keyword, type, field, sort, page, pageSize, out int count);
            model.Warehouses = warehouses;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = warehouses.Count();

            #region Select List
            ViewBag.WarehouseType = Ultility.EnumToSelectList<WAREHOUSE_TYPE>();
            ViewBag.WarehouseTypeAdd = Ultility.EnumToSelectList<WAREHOUSE_TYPE>(false);

            List<Organization> organizations = _commonService.GetOrganizations();
            organizations.Add(new Organization() { Id = Guid.Empty, Name = "--- Chọn ---" });
            var slOrganizations = organizations.OrderBy(x => x.Name).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Organizations = new SelectList(slOrganizations, "Value", "Text");
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
        public ActionResult Search(string keyword, WAREHOUSE_TYPE? type, int pageIndex)
        {
            return RedirectToAction("Index", new { search = keyword, type = type, pageIndex = pageIndex });
        }

        [HttpPost]
        public ActionResult Create(WarehouseSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Warehouse (Name: {model.Warehouse.Name})");
            if (model.Warehouse.OrganizationId == Guid.Empty) model.Warehouse.OrganizationId = null;
            _warehouseService.CreateWarehouse(model.Warehouse);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        public ActionResult Detail(Guid id)
        {
            _logger.LogDebug($"Warehouse Detail(Id: {id})");
            var model = new WarehouseViewModel();
            var entity = _warehouseService.GetWarehouse(id);
            if (entity != null) model = entity;

            #region Select List
            ViewBag.WarehouseType = Ultility.EnumToSelectList<WAREHOUSE_TYPE>(false);

            List<Organization> organizations = _commonService.GetOrganizations();
            organizations.Add(new Organization() { Id = Guid.Empty, Name = "--- Chọn ---" });
            var slOrganizations = organizations.OrderBy(x => x.Name).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Organizations = new SelectList(slOrganizations, "Value", "Text");
           
            List<AssetType> assetTypes = _commonService.GetAssetTypes();
            assetTypes.Add(new AssetType() { Id = 0, Name = "--- Chọn ---" });
            var slAssetTypes = assetTypes.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.AssetTypes = new SelectList(slAssetTypes, "Value", "Text");

            List<Loan> loans = _commonService.GetLoans();
            loans.Add(new Loan() { Id = Guid.Empty, Amount = 0 });
            var slLoans = loans.OrderBy(x => x.Amount).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Amount.ToString("#,###") });
            ViewBag.Loans = new SelectList(slLoans, "Value", "Text");

            #endregion

            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }

            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Edit(WarehouseViewModel model)
        {
            _logger.LogDebug($"Edit Warehouse(Id: {model.Id})");
            var entity = _warehouseService.GetWarehouse(model.Id);

            #region Nullable
            if (model.OrganizationId == Guid.Empty) model.OrganizationId = null;
            if(model.Asset != null)
            {
                if (model.Asset.AssetTypeId == 0) model.Asset.AssetTypeId = null;
                if (model.Asset.LoanId == Guid.Empty) model.Asset.LoanId = null;
                if (model.Asset.WarehouseId == Guid.Empty) model.Asset.WarehouseId = null;
            }
            #endregion

            if (entity != null)
            {
                _warehouseService.UpdateWarehouse(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Detail", new { Id = model.Id });
        }
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Warehouse(Id: {id})");
            var entity = _warehouseService.GetWarehouse(id);
            if (entity != null)
            {
                _warehouseService.DeleteWarehouse(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        [HttpPost]
        public ActionResult CreateAssetWarehouse(WarehouseViewModel model)
        {
            _logger.LogDebug($"Create Asset (Name: {model.Asset.Name})");

            #region Nullable
            if (model.Asset != null)
            {
                if (model.Asset.AssetTypeId == 0) model.Asset.AssetTypeId = null;
                if (model.Asset.LoanId == Guid.Empty) model.Asset.LoanId = null;
                if (model.Asset.WarehouseId == Guid.Empty) model.Asset.WarehouseId = null;
            }
            #endregion

            _assetService.CreateAsset(model.Asset);
            return RedirectToAction("Detail", new { Id = model.Asset.WarehouseId });
        }
        public ActionResult DeleteAssetWarehouse(int id, Guid warehouseId)
        {
            _logger.LogDebug($"Delete Asset (Id: {id})");
            var entity = _assetService.GetAssetProperty(id);
            if (entity != null) _assetService.DeleteAsset(id);
            return RedirectToAction("Detail", new { id = warehouseId });
        }
    }
}