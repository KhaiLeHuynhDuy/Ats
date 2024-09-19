using Ats.Commons;
using Ats.Domain;
using Ats.Domain.Identity.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Organization.Models;
using Ats.Models;
using Ats.Models.Asset;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
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

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class AssetController : BaseController
    {
        private IAssetService _assetService;
        private ICommonService _commonService;
        private IConfiguration _config;
        public AssetController(UserManager<User> userManage, IAssetService assetService, IConfiguration config,
                        ICommonService commonService, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _assetService = assetService;
            _commonService = commonService;
            _config = config;
        }

        #region Asset Type
        [HttpGet]
        public ActionResult AssetType(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Asset Type Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            AssetTypeSearchViewModel model = new AssetTypeSearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("AssetType", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var assetTypes = _assetService.SearchAssetType(model.Keyword, field, sort, page, pageSize, out count);
            model.AssetTypes = assetTypes;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = assetTypes.Count();

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
        public ActionResult SearchAssetType(string keyword, int pageIndex)
        {
            return RedirectToAction("AssetType", new { search = keyword, pageIndex = pageIndex });
        }
        [HttpPost]
        public ActionResult CreateAssetType(AssetTypeSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Asset Type (Name: {model.AssetType.Name})");
            _assetService.CreateAssetType(model.AssetType);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("AssetType", new { pageIndex = pageIndex });
        }
        public ActionResult DetailAssetType(int id)
        {
            _logger.LogDebug($"Asset Type Detail(Id: {id})");
            var model = new AssetTypeViewModel();
            var entity = _assetService.GetAssetType(id);
            if (entity != null) model = entity;
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);
            return View("EditAssetType", model);
        }
        [HttpPost]
        public ActionResult EditAssetType(AssetTypeViewModel model)
        {
            _logger.LogDebug($"Edit Asset Type(Id: {model.Id})");
            var entity = _assetService.GetAssetType(model.Id);
            if (entity != null)
            {
                _assetService.UpdateAssetType(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("DetailAssetType", new { Id = model.Id });
        }
        public ActionResult DeleteAssetType(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Asset Type(Id: {id})");
            var entity = _assetService.GetAssetType(id);
            if (entity != null)
            {
                _assetService.DeleteAssetType(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);
            return RedirectToAction("AssetType", new { pageIndex = pageIndex });
        }
        [HttpPost]
        public ActionResult CreateAssetPropertyType(AssetTypeViewModel model)
        {
            _logger.LogDebug($"Create Asset Property (Name: {model.AssetProperty.Name})");
            _assetService.CreateAssetProperty(model.AssetProperty);
            return RedirectToAction("DetailAssetType", new { Id = model.AssetProperty.AssetTypeId });
        }
        public ActionResult DeleteAssetPropertyType(int id, int assetTypeId)
        {
            _logger.LogDebug($"Delete Asset Property(Id: {id})");
            var entity = _assetService.GetAssetProperty(id);
            if (entity != null) _assetService.DeleteAssetProperty(id);
            return RedirectToAction("DetailAssetType", new { id = assetTypeId });
        }
        #endregion

        #region Asset Property
        [HttpGet]
        public ActionResult AssetProperty(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Asset Property Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            AssetPropertySearchViewModel model = new AssetPropertySearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("AssetProperty", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var assetProperties = _assetService.SearchAssetProperty(model.Keyword, field, sort, page, pageSize, out int count);
            model.AssetProperties = assetProperties;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = assetProperties.Count();

            List<AssetType> assetTypes = _commonService.GetAssetTypes();
            assetTypes.Add(new AssetType() { Id = 0, Name = "--- Chọn ---" });
            var slAssetTypes = assetTypes.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.AssetTypes = new SelectList(slAssetTypes, "Value", "Text");
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);

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
        public ActionResult SearchAssetProperty(string keyword, int pageIndex)
        {
            return RedirectToAction("AssetProperty", new { search = keyword, pageIndex = pageIndex });
        }

        [HttpPost]
        public ActionResult CreateAssetProperty(AssetPropertySearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Asset Property (Name: {model.AssetProperty.Name})");
            if (model.AssetProperty.AssetTypeId == 0) model.AssetProperty.AssetTypeId = null;
            _assetService.CreateAssetProperty(model.AssetProperty);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("AssetProperty", new { pageIndex = pageIndex });
        }
        public ActionResult DetailAssetProperty(int id)
        {
            _logger.LogDebug($"Asset Property Detail(Id: {id})");
            var model = new AssetPropertyViewModel();
            var entity = _assetService.GetAssetProperty(id);
            if (entity != null) model = entity;
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }

            List<AssetType> assetTypes = _commonService.GetAssetTypes();
            assetTypes.Add(new AssetType() { Id = 0, Name = "--- Chọn ---" });
            var slAssetTypes = assetTypes.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.AssetTypes = new SelectList(slAssetTypes, "Value", "Text");
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);

            return View("EditAssetProperty", model);
        }
        [HttpPost]
        public ActionResult EditAssetProperty(AssetPropertyViewModel model)
        {
            _logger.LogDebug($"Edit Asset Property(Id: {model.Id})");
            if (model.AssetTypeId == 0) model.AssetTypeId = null;
            var entity = _assetService.GetAssetProperty(model.Id);
            if (entity != null)
            {
                _assetService.UpdateAssetProperty(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("DetailAssetProperty", new { Id = model.Id });
        }
        public ActionResult DeleteAssetProperty(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Asset Property(Id: {id})");
            var entity = _assetService.GetAssetProperty(id);
            if (entity != null)
            {
                _assetService.DeleteAssetProperty(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("AssetProperty", new { pageIndex = pageIndex });
        }
        #endregion

        #region Asset 
        [HttpGet]
        public ActionResult Asset(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Asset Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            AssetSearchViewModel model = new AssetSearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("Asset", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var assets = _assetService.SearchAsset(model.Keyword, field, sort, page, pageSize, out int count);
            model.Assets = assets;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = assets.Count();

            #region Select List
            List<AssetType> assetTypes = _commonService.GetAssetTypes();
            assetTypes.Add(new AssetType() { Id = 0, Name = "--- Chọn ---" });
            var slAssetTypes = assetTypes.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.AssetTypes = new SelectList(slAssetTypes, "Value", "Text");

            List<Loan> loans = _commonService.GetLoans();
            loans.Add(new Loan() { Id = Guid.Empty, Amount = 0 });
            var slLoans = loans.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Amount.ToString("#,###") });
            ViewBag.Loans = new SelectList(slLoans, "Value", "Text");

            List<Warehouse> warehouses = _commonService.GetWarehouses();
            warehouses.Add(new Warehouse() { Id = Guid.Empty, Name = "--- Chọn ---" });
            var slWarehouses = warehouses.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Warehouses = new SelectList(slWarehouses, "Value", "Text");
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
        [HttpPost]
        public ActionResult CreateAsset(AssetSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Asset (Name: {model.Asset.Name})");

            #region Nullable
            if (model.Asset.AssetTypeId == 0) model.Asset.AssetTypeId = null;
            if (model.Asset.LoanId == Guid.Empty) model.Asset.LoanId = null;
            if (model.Asset.WarehouseId == Guid.Empty) model.Asset.WarehouseId = null;
            #endregion

            _assetService.CreateAsset(model.Asset);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Asset", new { pageIndex = pageIndex });
        }
        public ActionResult DetailAsset(int id)
        {
            _logger.LogDebug($"Asset Detail(Id: {id})");
            var model = new AssetViewModel();
            var entity = _assetService.GetAsset(id);
            if (entity != null) model = entity;

            #region Select list
            List<AssetType> assetTypes = _commonService.GetAssetTypes();
            assetTypes.Add(new AssetType() { Id = 0, Name = "--- Chọn ---" });
            var slAssetTypes = assetTypes.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.AssetTypes = new SelectList(slAssetTypes, "Value", "Text");

            List<Loan> loans = _commonService.GetLoans();
            loans.Add(new Loan() { Id = Guid.Empty, Amount = 0 });
            var slLoans = loans.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Amount.ToString("#,###") });
            ViewBag.Loans = new SelectList(slLoans, "Value", "Text");

            List<Warehouse> warehouses = _commonService.GetWarehouses();
            warehouses.Add(new Warehouse() { Id = Guid.Empty, Name = "--- Chọn ---" });
            var slWarehouses = warehouses.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Warehouses = new SelectList(slWarehouses, "Value", "Text");

            List<AssetProperty> assetProperties = _commonService.GetAssetProperties();
            var slAssetProperties = assetProperties.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.AssetProperties = new SelectList(slAssetProperties, "Value", "Text");     
            #endregion

            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);

            #region Status message
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            if (TempData["addAttributeSuccess"] != null)
            {
                ViewBag.StatusAttributeMessage = TempData["addAttributeSuccess"].ToString();
                TempData.Remove("addAttributeSuccess");
            }
            if (TempData["updateAttributeSuccess"] != null)
            {
                ViewBag.StatusAttributeMessage = TempData["updateAttributeSuccess"].ToString();
                TempData.Remove("updateAttributeSuccess");
            }
            if (TempData["deleteAttributeSuccess"] != null)
            {
                ViewBag.StatusAttributeMessage = TempData["deleteAttributeSuccess"].ToString();
                TempData.Remove("deleteAttributeSuccess");
            }
            if (TempData["errorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["errorMessage"].ToString();
                TempData.Remove("errorMessage");
            }
            #endregion

            return View("EditAsset", model);
        }
        [HttpPost]
        public ActionResult EditAsset(AssetViewModel model)
        {
            _logger.LogDebug($"Edit Asset (Id: {model.Id})");

            #region Nullable
            if (model.AssetTypeId == 0) model.AssetTypeId = null;
            if (model.LoanId == Guid.Empty) model.LoanId = null;
            if (model.WarehouseId == Guid.Empty) model.WarehouseId = null;
            #endregion

            var entity = _assetService.GetAsset(model.Id);
            if (entity != null)
            {
                _assetService.UpdateAsset(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("DetailAsset", new { Id = model.Id });
        }
        public ActionResult DeleteAsset(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Asset (Id: {id})");
            var entity = _assetService.GetAsset(id);
            if (entity != null)
            {
                _assetService.DeleteAsset(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Asset", new { pageIndex = pageIndex });
        }
        #endregion

        #region Asset Attribute
        [HttpPost]
        public IActionResult AddAssetAttribute(AssetViewModel model)
        {
            _logger.LogDebug($"Add Asset Attribute (assetId: {model.AssetAttribute.AssetId}, assetPropertyId: {model.AssetAttribute.AssetPropertyId})");
            if (!_assetService.CheckExistAddAssetAttribute(model.AssetAttribute.AssetId, model.AssetAttribute.AssetPropertyId))
            {
                _assetService.AddAssetAttribute(model.AssetAttribute);
                TempData["addAttributeSuccess"] = Resource.Common_notify_addAttributeSuccessfully;
            }
            else
            {
                TempData["errorMessage"] = Resource.Common_errorMessage_attributeAlreadyExisted;
            }
            return RedirectToAction("DetailAsset", new { id = model.AssetAttribute.AssetId });
        }
        [HttpGet]
        public ActionResult DetailAssetAttribute(int id)
        {
            _logger.LogDebug($"Asset Attribute Detail(Id: {id})");
            var model = new AssetAttributeModel();
            var entity = _assetService.GetAssetAttribute(id);
            if (entity != null) model = entity;
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);

            return PartialView("_DetailAssetAttribute", model);
        }
        [HttpPost]
        public ActionResult EditAssetAttribute(AssetAttributeModel model)
        {
            _logger.LogDebug($"Edit Asset Attribute(Id: {model.Id})");
            var entity = _assetService.GetAssetAttribute(model.Id);
            if (entity != null)
            {
                _assetService.UpdateAssetAttribute(model);
                TempData["updateAttributeSuccess"] = Resource.Common_notify_updateAttributeSuccessfully;
            }
            return RedirectToAction("DetailAsset", new { id = model.AssetId });
        }
        [HttpGet]
        public ActionResult DeleteAssetAttribute(int id, int assetId)
        {
            _logger.LogDebug($"Delete Asset Attribute (Id: {id})");
            var entity = _assetService.GetAssetAttribute(id);
            if (entity != null)
            {
                _assetService.DeleteAssetAttribute(id);
                TempData["deleteAttributeSuccess"] = Resource.Common_notify_deleteAttributeSuccessfully;
            }
            return RedirectToAction("DetailAsset", new { id = assetId });
        }
        #endregion
    }
}
