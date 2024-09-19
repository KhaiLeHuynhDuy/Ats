using Ats.Commons;
using Ats.Domain;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Identity.Models;
using Ats.Domain.Loyalty.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Brand;
using Ats.Models.Channel;
using Ats.Models.Loyalty;
using Ats.Models.LoyaltyTier;
using Ats.Models.MemberWallet;
using Ats.Models.Product;
using Ats.Models.Store;
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
    public class LoyaltyPointRulesController : BaseController
    {
        private ILoyaltyPointRulesService _loyaltyservice;
        private IConfiguration _config;
        private ILoyaltyTierService _loyaltyTierservice;

        private IStoreService _storeservice;
        private IProductService _productservice;
        private IMemberChannelService _memberChannelservice;
        //private IBrandService _brandservice;
        private IMemberChannelService _channelservice;
        private IProductCategoryService _productCategoryservice;
        private IPointRuleStoresService _pointRuleStoreservice;
        private IPointRuleProductService _pointRuleProductservice;
        private IPointRuleChannelService _pointRuleChannelservice;
        private IPointRuleBrandService _pointRuleBrandservice;
        private IPointRuleCategoryService _pointRuleCategoryservice;
        private ILoyaltyPointSettingService _loyaltyPointSettingservice;



        #region Mapping IserVices
        public LoyaltyPointRulesController(UserManager<User> userManage, ILoyaltyPointRulesService loyaltyservice, IConfiguration config, ILoyaltyTierService loyaltyTierCatService,
            IPointRuleStoresService pointRuleStoreservice, ILoyaltyPointSettingService loyaltyPointSettingservice, IPointRuleProductService pointRuleProductservice,
            IMemberChannelService memberChannelservice, IPointRuleChannelService pointRuleChannelservice, IPointRuleBrandService pointRuleBrandservice, IPointRuleCategoryService pointRuleCategoryservice,
            IStoreService storeService,  IMemberChannelService channelservice, IProductCategoryService productCategoryservice,
            IProductService productService, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _loyaltyservice = loyaltyservice;
            _config = config;
            _loyaltyTierservice = loyaltyTierCatService;

            _storeservice = storeService;
            _productservice = productService;
            _memberChannelservice = memberChannelservice;
            //_brandservice = brandService;
            _channelservice = channelservice;
            _productCategoryservice = productCategoryservice;
            _loyaltyPointSettingservice = loyaltyPointSettingservice;
            _pointRuleStoreservice = pointRuleStoreservice;
            _pointRuleProductservice = pointRuleProductservice;
            _pointRuleChannelservice = pointRuleChannelservice;
            _pointRuleBrandservice = pointRuleBrandservice;
            _pointRuleCategoryservice = pointRuleCategoryservice;

        }
        #endregion

        #region View TierRule
        [AuthorizeRoles( RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult TierRule(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Loyalty.TierRule loaded");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            LoyaltyTierSearchViewModel model = new LoyaltyTierSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var loyaltyTiers = _loyaltyTierservice.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.LoyaltyTiers = loyaltyTiers;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = loyaltyTiers.Count();

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
        #endregion

        #region View Referral
        [AuthorizeRoles( RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Referral()
        {
            _logger.LogDebug($"Loyalty.Referral loaded");

            return View();
        }
        #endregion

        #region View Point Strandrad Rules
        [AuthorizeRoles( RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult PointRuleStandard(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Loyalty.PointRule loaded");
            if (page == 0) page += 1; /*int id = 1;*/
            int pageSize = _config.GetValue<int>("PageSize");
            LoyaltySearchViewModel model = new LoyaltySearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0;
            var loyaltyPointRules = _loyaltyservice.SearchLoyaltyPointRule(model.Keyword, field, sort, page, pageSize, out count);
            var loyaltyPointTypes = _loyaltyservice.SearchLoyaltyPointType(model.Keyword, field, sort, page, pageSize, out count);


            model.LoyaltyPointRules = loyaltyPointRules;
            model.LoyaltyPointTypes = loyaltyPointTypes;


            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = loyaltyPointRules.Count();


            #region Select List Category
            List<CouponCategory> couponCategories = _loyaltyservice.GetCouponCategories();
            couponCategories.Add(new CouponCategory() { Id = 0, CouponName = "Select" });
            var slCouponCategories = couponCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CouponName });
            ViewBag.CouponCategories = new SelectList(slCouponCategories, "Value", "Text");

            List<CouponCategory> month = _loyaltyservice.GetCouponCategories();
            month.Add(new CouponCategory() { Id = 0, CouponName = "Month(s)" });
            var slmonth = month.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CouponName });
            ViewBag.Month = new SelectList(slmonth, "Value", "Text");


            #endregion

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

            ViewBag.Months = Ultility.EnumToSelectList<NameMonths>(false);
            return View(model);
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult PointRule(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Loyalty.PointRule loaded");
            if (page == 0) page += 1; /*int id = 1;*/
            int pageSize = _config.GetValue<int>("PageSize");
            LoyaltySearchViewModel model = new LoyaltySearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("PointRule", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0;
            var loyaltyPointRules = _loyaltyservice.SearchLoyaltyPointRule(model.Keyword, field, sort, page, pageSize, out count);
           

            model.LoyaltyPointRules = loyaltyPointRules;
         

            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = loyaltyPointRules.Count();
          

            #region Select List Category
            List<CouponCategory> couponCategories = _loyaltyservice.GetCouponCategories();
            couponCategories.Add(new CouponCategory() { Id = 0, CouponName = "Select" });
            var slCouponCategories = couponCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CouponName });
            ViewBag.CouponCategories = new SelectList(slCouponCategories, "Value", "Text");

            List<CouponCategory> month = _loyaltyservice.GetCouponCategories();
            month.Add(new CouponCategory() { Id = 0, CouponName = "Month(s)" });
            var slmonth = month.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CouponName });
            ViewBag.Month = new SelectList(slmonth, "Value", "Text");

           
            #endregion

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

            ViewBag.Months = Ultility.EnumToSelectList<NameMonths>(false);
            return View(model);
        }
        #endregion

        #region controller point rule custom

        [AuthorizeRoles( RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult CreatePointRule()
        {
            _logger.LogDebug($"Create new Point Rule");

            return View();
        }

        [HttpPost]
        public ActionResult CreatePointRule(LoyaltySearchViewModel model)
        {
            _logger.LogDebug($"Create LoyaltyPointRules (Name: {model.LoyaltyPointRule.RuleName})");
            _loyaltyservice.CreateLoyaltyPointRule(model.LoyaltyPointRule);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("PointRule");
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Detail(Guid id)
        {
           
            _logger.LogDebug($"Team Detail(Id: {id})");
            var model = new LoyaltyPointRulesViewModel();
            var entity = _loyaltyservice.GetLoyaltyPointRule(id);
            if (entity != null) model = entity;

            #region Status message Store
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            if (TempData["addStoreSuccess"] != null)
            {
                ViewBag.StatusStoreMessage = TempData["addStoreSuccess"].ToString();
                TempData.Remove("addStoreSuccess");
            }
            if (TempData["updateStoreSuccess"] != null)
            {
                ViewBag.StatusStoreMessage = TempData["updateStoreSuccess"].ToString();
                TempData.Remove("updateStoreSuccess");
            }
            if (TempData["deleteStoreSuccess"] != null)
            {
                ViewBag.StatusStoreMessage = TempData["deleteStoreSuccess"].ToString();
                TempData.Remove("deleteStoreSuccess");
            }
            #endregion

            #region Status message Category
            if (TempData["addCateSuccess"] != null)
            {
                ViewBag.StatusCateMessage = TempData["addCateSuccess"].ToString();
                TempData.Remove("addCateSuccess");
            }
            if (TempData["updateCateSuccess"] != null)
            {
                ViewBag.StatusCateMessage = TempData["updateCateSuccess"].ToString();
                TempData.Remove("updateCateSuccess");
            }
            if (TempData["deleteCateSuccess"] != null)
            {
                ViewBag.StatusCateMessage = TempData["deleteCateSuccess"].ToString();
                TempData.Remove("deleteCateSuccess");
            }
            #endregion

            #region Status message Channel
            if (TempData["addChannelSuccess"] != null)
            {
                ViewBag.StatusChannelMessage = TempData["addChannelSuccess"].ToString();
                TempData.Remove("addChannelSuccess");
            }
            if (TempData["updateChannelSuccess"] != null)
            {
                ViewBag.StatusChannelMessage = TempData["updateChannelSuccess"].ToString();
                TempData.Remove("updateChannelSuccess");
            }
            if (TempData["deleteChannelSuccess"] != null)
            {
                ViewBag.StatusChannelMessage = TempData["deleteChannelSuccess"].ToString();
                TempData.Remove("deleteChannelSuccess");
            }
            #endregion

            #region Status message Product
            if (TempData["addProductSuccess"] != null)
            {
                ViewBag.StatusProductMessage = TempData["addProductSuccess"].ToString();
                TempData.Remove("addProductSuccess");
            }
            if (TempData["updateProductSuccess"] != null)
            {
                ViewBag.StatusProductMessage = TempData["updateProductSuccess"].ToString();
                TempData.Remove("updateProductSuccess");
            }
            if (TempData["deleteProductSuccess"] != null)
            {
                ViewBag.StatusProductMessage = TempData["deleteProductSuccess"].ToString();
                TempData.Remove("deleteProductSuccess");
            }
            #endregion

            #region Status message Brand
            if (TempData["addBrandSuccess"] != null)
            {
                ViewBag.StatusBrandMessage = TempData["addBrandSuccess"].ToString();
                TempData.Remove("addBrandSuccess");
            }
            if (TempData["updateBrandSuccess"] != null)
            {
                ViewBag.StatusBrandMessage = TempData["updateBrandSuccess"].ToString();
                TempData.Remove("updateBrandSuccess");
            }
            if (TempData["deleteBrandSuccess"] != null)
            {
                ViewBag.StatusBrandMessage = TempData["deleteBrandSuccess"].ToString();
                TempData.Remove("deleteBrandSuccess");
            }
            #endregion

            return View("EditPointRule", model);
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult EditPointRule(LoyaltyPointRulesViewModel model)
        {
            
                _logger.LogDebug($"Edit Loyalty PointRule(Id: {model.Id})");
                var entity = _loyaltyservice.GetLoyaltyPointRule(model.Id);
                if (entity != null)
                {
                    _loyaltyservice.UpdateLoyaltyPointRule(model);
                    TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                }
           
            return RedirectToAction("PointRule", new { Id = model.Id });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult DeletePointRule(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete LoyaltyTier(Id: {id})");
            var entity = _loyaltyservice.GetLoyaltyPointRule(id);
            if (entity != null)
            {
                _loyaltyservice.DeleteLoyaltyPointRule(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("PointRule", new { pageIndex = pageIndex });
        }

        #endregion

        #region Item Store
        [HttpGet]
        public ActionResult CreateItemPointRuleStore(Guid ruleId)
        {
            _logger.LogDebug($"Create new Point Rule Store (Id: {ruleId})");
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            var model = new PointRuleStoresViewModel()
            {
                LoyaltyPointRuleId = ruleId,
                Valid = true

            };
            List<StoreViewModel> stores = _storeservice.GetStores();
            stores.Add(new StoreViewModel() { Id = 0, StoreName = "--- Chọn Cửa Hàng ---" });
            var slStores = stores.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.Store = new SelectList(slStores, "Value", "Text");
            return View(model);
        }   
        [HttpPost]
        public ActionResult CreateItemPointRuleStore(PointRuleStoresViewModel model)
        {
            _logger.LogDebug($"Create LoyaltyPointRules Stores (Name: {model.Id})");
            if (!_loyaltyservice.CheckExistAddStores(model.LoyaltyPointRuleId, model.StoreId) && model.StoreId !=0)
            {
                _loyaltyservice.CreateLoyaltyItemPointRuleStore(model);
                TempData["addStoreSuccess"] = Resource.Common_notify_addSuccessfully;
                return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
            }
            else
            {
                if(model.StoreId ==0)
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_storeSelectedErr;
                }
                else
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_storeAlreadyExisted;
                }
                return RedirectToAction("CreateItemPointRuleStore", new { ruleId = model.LoyaltyPointRuleId });
            }
        }
        [HttpGet]
        public ActionResult EditItemPointRuleStore(Guid id)
        {
            _logger.LogDebug($"Edit Point Rule Store  Detail(Id: {id})");
           
            var model = _loyaltyservice.GetPointRuleStore(id);


            List<StoreViewModel> stores = _storeservice.GetStores();
            stores.Add(new StoreViewModel() { Id = 0, StoreName = "--- Chọn Cửa Hàng ---" });
            var slStores = stores.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.Store = new SelectList(slStores, "Value", "Text");

            List<LoyaltyPointRulesViewModel> loyaltyPointRules = _loyaltyservice.GetLoyaltyPointRules();
            loyaltyPointRules.Add(new LoyaltyPointRulesViewModel() { Id = Guid.Empty, RuleName = "--- Chọn Tên Quy Tắc ---" });
            var slLoyaltyPointRules = loyaltyPointRules.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.RuleName });
            ViewBag.LoyaltyPointRule = new SelectList(slLoyaltyPointRules, "Value", "Text");


            return View(model);

        }
        [HttpPost]
        public ActionResult EditItemPointRuleStore(PointRuleStoresViewModel model)
        {

            if (model.StoreId == 0) model.StoreId = 0;
            _logger.LogDebug($"Edit Point Rule Store(Id: {model.Id})");
            _loyaltyservice.UpdateLoyaltyItemPointRuleStore(model);
            TempData["updateStoreSuccess"] = Resource.Common_notify_updateSuccessfully;

            return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
        }
        public ActionResult DeleteItemPointRuleStore(Guid id, Guid ruleId)
        {
            _logger.LogDebug($"Delete LoyaltyPoint Rule Store(Id: {id})");
            var entity = _loyaltyservice.GetPointRuleStore(id);
            if (entity != null)
            {
                _loyaltyservice.DeleteLoyaltyItemPointRuleStore(id);
                TempData["deleteStoreSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Detail", new { id = ruleId });
        }
        #endregion

        #region Item Product
        [HttpGet]
        public ActionResult CreateItemPointRuleProduct(Guid ruleId)
        {
            _logger.LogDebug($"Create new Point Rule Product(Id: {ruleId})");
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            var model = new PointRuleProductViewModel()
            {
                LoyaltyPointRuleId = ruleId,
                Valid = true

            };
            List<ProductViewModel> products = _productservice.GetProductsAllowEarnPoint();
            products.Add(new ProductViewModel() { Id = 0, ProductName = "--- Chọn Sản Phẩm ---" });
            var slProducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ProductName });
            ViewBag.Product = new SelectList(slProducts, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateItemPointRuleProduct(PointRuleProductViewModel model)
        {
            
            _logger.LogDebug($"Create LoyaltyPointRules Product (Name: {model.Id})");
            if (!_loyaltyservice.CheckExistAddProducts(model.LoyaltyPointRuleId, model.ProductId) && model.ProductId !=0)
            {
                _loyaltyservice.CreateLoyaltyItemPointRuleProduct(model);
                TempData["addProductSuccess"] = Resource.Common_notify_addSuccessfully;
                return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
            }
            else
            {
                if (model.ProductId == 0)
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_productSelectedErr;
                }
                else
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_productAlreadyExisted;
                }
                return RedirectToAction("CreateItemPointRuleProduct", new { ruleId = model.LoyaltyPointRuleId });
            }
        }

        [HttpGet]
        public ActionResult EditItemPointRuleProduct(Guid id)
        {
            _logger.LogDebug($"Edit Point Rule Product  Detail(Id: {id})");

            var model = _loyaltyservice.GetPointRuleProduct(id);

            List<ProductViewModel> products = _productservice.GetProducts();
            products.Add(new ProductViewModel() { Id = 0, ProductName = "--- Chọn Sản Phẩm ---" });
            var slProducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ProductName });
            ViewBag.Product = new SelectList(slProducts, "Value", "Text");

            List<LoyaltyPointRulesViewModel> loyaltyPointRules = _loyaltyservice.GetLoyaltyPointRules();
            loyaltyPointRules.Add(new LoyaltyPointRulesViewModel() { Id = Guid.Empty, RuleName = "--- Chọn Tên Quy Tắc ---" });
            var slLoyaltyPointRules = loyaltyPointRules.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.RuleName });
            ViewBag.LoyaltyPointRule = new SelectList(slLoyaltyPointRules, "Value", "Text");


            return View(model);
        }

        [HttpPost]
        public ActionResult EditItemPointRuleProduct(PointRuleProductViewModel model)
        {

            _logger.LogDebug($"Edit Point Rule Product(Id: {model.Id})");
            _loyaltyservice.UpdateLoyaltyItemPointRuleProduct(model);
            TempData["updateProdcutSuccess"] = Resource.Common_notify_updateSuccessfully;

            return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
        }

        public ActionResult DeleteItemPointRuleProduct(Guid id, Guid ruleId)
        {
            _logger.LogDebug($"Delete LoyaltyPoint Rule Product(Id: {id})");
            var entity = _loyaltyservice.GetPointRuleProduct(id);
            if (entity != null)
            {
                _loyaltyservice.DeleteLoyaltyItemPointRuleProduct(id);
                TempData["deleteProductSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Detail", new { id = ruleId });
        }
        #endregion

        #region Item Channel

        [HttpGet]
        public ActionResult CreateItemPointRuleChannel(Guid ruleId)
        {
            _logger.LogDebug($"Create new Point Rule Channel (Id: {ruleId})");
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            var model = new PointRuleChannelViewModel()
            {
                LoyaltyPointRuleId = ruleId,
                Valid = true

            };
            List<ChannelViewModel> channels = _channelservice.GetChannels();
            channels.Add(new ChannelViewModel() { Id = 0, ChannelName = "--- Chọn Kênh ---" });
            var slChannels = channels.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.Channel = new SelectList(slChannels, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateItemPointRuleChannel(PointRuleChannelViewModel model)
        {

            _logger.LogDebug($"Create LoyaltyPointRules Channel (Name: {model.Id})");
            if (!_loyaltyservice.CheckExistAddChannels(model.LoyaltyPointRuleId, model.ChannelId)&& model.ChannelId !=0)
            {
                _loyaltyservice.CreateLoyaltyItemPointRuleChannel(model);
                TempData["addChannelSuccess"] = Resource.Common_notify_addSuccessfully;
                return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
            }
            else
            {
                if (model.ChannelId == 0)
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_channelSelectedErr;
                }
                else
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_channelAlreadyExisted;
                }
                return RedirectToAction("CreateItemPointRuleChannel", new { ruleId = model.LoyaltyPointRuleId });
            }
        }

        [HttpGet]
        public ActionResult EditItemPointRuleChannel(Guid id)
        {
            _logger.LogDebug($"Edit Point Rule Channel  Detail(Id: {id})");

            var model = _loyaltyservice.GetPointRuleChannel(id);

            List<ChannelViewModel> channels = _channelservice.GetChannels();
            channels.Add(new ChannelViewModel() { Id = 0, ChannelName = "--- Chọn Kênh ---" });
            var slChannels = channels.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.Channel = new SelectList(slChannels, "Value", "Text");

            List<LoyaltyPointRulesViewModel> loyaltyPointRules = _loyaltyservice.GetLoyaltyPointRules();
            loyaltyPointRules.Add(new LoyaltyPointRulesViewModel() { Id = Guid.Empty, RuleName = "--- Chọn Tên Quy Tắc ---" });
            var slLoyaltyPointRules = loyaltyPointRules.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.RuleName });
            ViewBag.LoyaltyPointRule = new SelectList(slLoyaltyPointRules, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        public ActionResult EditItemPointRuleChannel(PointRuleChannelViewModel model)
        {
          
            _logger.LogDebug($"Edit Point Rule Channel(Id: {model.Id})");
            _loyaltyservice.UpdateLoyaltyItemPointRuleChannel(model);
            TempData["updateChannelSuccess"] = Resource.Common_notify_updateSuccessfully;
            return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
        }

        public ActionResult DeleteItemPointRuleChannel(Guid id, Guid ruleId)
        {
            _logger.LogDebug($"Delete LoyaltyPoint Rule Channel(Id: {id})");
            var entity = _loyaltyservice.GetPointRuleChannel(id);
            if (entity != null)
            {
                _loyaltyservice.DeleteLoyaltyItemPointRuleChannel(id);
                TempData["deleteChannelSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Detail", new { id = ruleId });
        }
        #endregion

        #region Item Brand

        [HttpGet]
        public ActionResult CreateItemPointRuleBrand(Guid ruleId)
        {
            _logger.LogDebug($"Create Point Rule Brand");
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            var model = new PointRuleBrandViewModel()
            {
                LoyaltyPointRuleId = ruleId,
                Valid = true

            };
            //List<BrandViewModel> brands = _brandservice.GetBrands();
            //brands.Add(new BrandViewModel() { Id = 0, BrandName = "--- Chọn Thương Hiệu ---" });
            //var slBrands= brands.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.BrandName });
            //ViewBag.Brand = new SelectList(slBrands, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateItemPointRuleBrand(PointRuleBrandViewModel model)
        {
            _logger.LogDebug($"Create new Point Rules Brand (Id: {model.Id})");
           
            if (!_loyaltyservice.CheckExistAddBrands(model.LoyaltyPointRuleId, model.BrandId) && model.BrandId !=0)
            {
                _loyaltyservice.CreateLoyaltyItemPointRuleBrand(model);
                TempData["addBrandSuccess"] = Resource.Common_notify_addSuccessfully;
                return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
            }
            else
            {
                if (model.BrandId == 0)
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_brandSelectedErr;
                }
                else
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_brandAlreadyExisted;
                }
                return RedirectToAction("CreateItemPointRuleBrand", new { ruleId = model.LoyaltyPointRuleId });
            }
        }

        [HttpGet]
        public ActionResult EditItemPointRuleBrand(Guid id)
        {
            _logger.LogDebug($"Edit Point Rule Brand  Detail(Id: {id})");


            var model = _loyaltyservice.GetPointRuleBrand(id);

            //List<BrandViewModel> brands = _brandservice.GetBrands();
            //brands.Add(new BrandViewModel() { Id = 0, BrandName = "--- Chọn Thương Hiệu ---" });
            //var slBrands = brands.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.BrandName });
            //ViewBag.Brand = new SelectList(slBrands, "Value", "Text");
            List<LoyaltyPointRulesViewModel> loyaltyPointRules = _loyaltyservice.GetLoyaltyPointRules();
            loyaltyPointRules.Add(new LoyaltyPointRulesViewModel() { Id = Guid.Empty, RuleName = "--- Chọn Tên Quy Tắc ---" });
            var slLoyaltyPointRules = loyaltyPointRules.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.RuleName });
            ViewBag.LoyaltyPointRule = new SelectList(slLoyaltyPointRules, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        public ActionResult EditItemPointRuleBrand(PointRuleBrandViewModel model, int pageIndex)
        {
            

            _logger.LogDebug($"Edit Point Rule Brand(Id: {model.Id})");
            _loyaltyservice.UpdateLoyaltyItemPointRuleBrand(model);
            TempData["updateBrandSuccess"] = Resource.Common_notify_updateSuccessfully;

            return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
        }

        public ActionResult DeleteItemPointRuleBrand(Guid id, Guid ruleId)
        {
            _logger.LogDebug($"Delete LoyaltyPoint Rule Brand(Id: {id})");
            var entity = _loyaltyservice.GetPointRuleBrand(id);
            if (entity != null)
            {
                _loyaltyservice.DeleteLoyaltyItemPointRuleBrand(id);
                TempData["deleteBrandSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Detail", new { id = ruleId });
        }
        #endregion

        #region Item Category

        [HttpGet]
        public ActionResult CreateItemPointRuleCategory(Guid ruleId)
        {
            _logger.LogDebug($"Create new Point Rule Category(Id: {ruleId})");
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            var model = new PointRuleCategoryViewModel()
            {
                LoyaltyPointRuleId = ruleId,
                Valid = true

            };
            List<ProductCategoryViewModel> categories = _productCategoryservice.GetProductCategorys();
            categories.Add(new ProductCategoryViewModel() { Id = 0, Name = "--- Chọn Thương Hiệu ---" });
            var slCategories = categories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Categories = new SelectList(slCategories, "Value", "Text");

           
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateItemPointRuleCategory(PointRuleCategoryViewModel model)
        {
            
            _logger.LogDebug($"Create LoyaltyPointRules Category (Name: {model.Id})");
            if (!_loyaltyservice.CheckExistAddCategories(model.LoyaltyPointRuleId, model.ProductCateId) && model.ProductCateId != 0)
            {
                _loyaltyservice.CreateLoyaltyItemPointRuleCategory(model);
                TempData["addCateSuccess"] = Resource.Common_notify_addSuccessfully;
                return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
            }
            else
            {
                if (model.ProductCateId == 0)
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_productCateSelectedErr;
                }
                else
                {
                    TempData["ErrorMessage"] = Resource.Common_errorMessage_productCateAlreadyExisted;
                }
                return RedirectToAction("CreateItemPointRuleCategory", new { ruleId = model.LoyaltyPointRuleId });
            }
        }

        [HttpGet]
        public ActionResult EditItemPointRuleCategory(Guid id)
        {
            _logger.LogDebug($"Edit Point Rule Category  Detail(Id: {id})");

            var model = _loyaltyservice.GetPointRuleCate(id);


            List<ProductCategoryViewModel> categories = _productCategoryservice.GetProductCategorys();
            categories.Add(new ProductCategoryViewModel() { Id = 0, Name = "--- Chọn Thương Hiệu ---" });
            var slCategories = categories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Categories = new SelectList(slCategories, "Value", "Text");

            List<LoyaltyPointRulesViewModel> loyaltyPointRules = _loyaltyservice.GetLoyaltyPointRules();
            loyaltyPointRules.Add(new LoyaltyPointRulesViewModel() { Id = Guid.Empty, RuleName = "--- Chọn Tên Quy Tắc ---" });
            var slLoyaltyPointRules = loyaltyPointRules.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.RuleName });
            ViewBag.LoyaltyPointRule = new SelectList(slLoyaltyPointRules, "Value", "Text");

            return View(model);

        }

        [HttpPost]
        public ActionResult EditItemPointRuleCategory(PointRuleCategoryViewModel model)
        {
           
            _logger.LogDebug($"Edit Point Rule Categories(Id: {model.Id})");
            _loyaltyservice.UpdateLoyaltyItemPointRuleCategory(model);
            TempData["updateCateSuccess"] = Resource.Common_notify_updateSuccessfully;

            return RedirectToAction("Detail", new { id = model.LoyaltyPointRuleId });
        }

        public ActionResult DeleteItemPointRuleCategory(Guid id, Guid ruleId)
        {
            _logger.LogDebug($"Delete LoyaltyPoint Rule Category(Id: {id})");
            var entity = _loyaltyservice.GetPointRuleCate(id);
            if (entity != null)
            {
                _loyaltyservice.DeleteLoyaltyItemPointRuleCategory(id);
                TempData["deleteCateSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Detail", new { id = ruleId });

        }
        #endregion

       
    }
}