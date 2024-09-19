using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Brand;
using Ats.Models.Occupation;
using Ats.Models.Product;
using Ats.Models.Unit;
using Ats.Security.WebSecurity;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Models;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class ProductController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private IProductService _productservice;
        private IUnitService _unitService;
        private IConfiguration _config;
        //public ICommonServiceProduct _commonService;
        public ProductController(UserManager<User> userManage,IUnitService unitService, IProductService productService, IConfiguration config, IWebHostEnvironment hostEnvironment,
                        /*ICommonServiceProduct commonService,*/ SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _productservice = productService;
            _unitService = unitService;
            _config = config;
            //_commonService = commonService;
            webHostEnvironment = hostEnvironment;
        }


        [AuthorizeRoles( RoleName.SYS_PRODUCT_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Product Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            ProductSearchViewModel model = new ProductSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var products = _productservice.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.Products = products;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = products.Count();

            #region List Category
            List<ProductCategoryViewModel> productCategories = _productservice.GetProductCategories();
            productCategories.Add(new ProductCategoryViewModel() { CateId = null, Name = "--- Select ---" });
            var slProductCategories = productCategories.OrderBy(x => x.CateId).ToList().Select(x => new SelectListItem { Value = x.CateId.ToString(), Text = x.Name });
            ViewBag.ProductCategories = new SelectList(slProductCategories, "Value", "Text");

            List<BrandViewModel> brandCategories = _productservice.GetBrandCategories();
            brandCategories.Add(new BrandViewModel() { BrandId = null, BrandName = "--- Select ---" });
            var slBrandCategories = brandCategories.OrderBy(x => x.BrandId).ToList().Select(x => new SelectListItem { Value = x.BrandId.ToString(), Text = x.BrandName });
            ViewBag.BrandCategories = new SelectList(slBrandCategories, "Value", "Text");

            List<UnitViewModel> unitCategories = _unitService.GetUnits();
            unitCategories.Add(new UnitViewModel() { UnitId = null, Name = "--- Select ---" });
            var slUnitCategories = unitCategories.OrderBy(x => x.UnitId).ToList().Select(x => new SelectListItem { Value = x.UnitId.ToString(), Text = x.Name });
            ViewBag.UnitCategories = new SelectList(slUnitCategories, "Value", "Text");
            #endregion List Category

            
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
        public ActionResult Search(string keyword, string Field, bool Sort, int Page)
        {
            return RedirectToAction("Index", new { search = keyword,field=Field,sort=Sort, page = Page });
        }
        [AuthorizeRoles( RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Create(ProductSearchViewModel model, int pageIndex)
        {

            #region Nullable 
            //if (model.Product.BrandId == 0) model.Product.BrandId = null;
            //if (model.DistrictId == 0) model.DistrictId = null;
            //if (model.OccupationId == 0) model.OccupationId = null;
            //if (model.JobTitleId == 0) model.JobTitleId = null;
            //if (model.YOB == 0) model.YOB = 0;

            #endregion
            _logger.LogDebug($"Create Product (Name: {model.Product.Name})");
            _productservice.CreateProduct(model.Product);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }


        [AuthorizeRoles( RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            _logger.LogDebug($"Product Detail(Id: {id})");
            var entity = _productservice.GetProduct(id);

            //model.Product.uniqueFileName = entity.ImageProfile.ToString();
            #region List Category
            List<ProductCategoryViewModel> productCategories = _productservice.GetProductCategories();
            productCategories.Add(new ProductCategoryViewModel() {  CateId= null, Name = "--- Select ---" });
            var slProductCategories = productCategories.OrderBy(x => x.CateId).ToList().Select(x => new SelectListItem { Value = x.CateId.ToString(), Text = x.Name });
            ViewBag.ProductCategories = new SelectList(slProductCategories, "Value", "Text");

            List<BrandViewModel> brandCategories = _productservice.GetBrandCategories();
            brandCategories.Add(new BrandViewModel() { BrandId = null, BrandName = "--- Select ---" });
            var slBrandCategories = brandCategories.OrderBy(x => x.BrandId).ToList().Select(x => new SelectListItem { Value = x.BrandId.ToString(), Text = x.BrandName });
            ViewBag.BrandCategories = new SelectList(slBrandCategories, "Value", "Text");

            List<UnitViewModel> unitCategories = _unitService.GetUnits();
            unitCategories.Add(new UnitViewModel() { UnitId = null, Name = "--- Select ---" });
            var slUnitCategories = unitCategories.OrderBy(x => x.UnitId).ToList().Select(x => new SelectListItem { Value = x.UnitId.ToString(), Text = x.Name });
            ViewBag.UnitCategories = new SelectList(slUnitCategories, "Value", "Text");
            #endregion List Category
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
            return View(entity);
        }


        [AuthorizeRoles( RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult EditProduct(int id)
        {


            _logger.LogDebug($"Product Detail(Id: {id})");
            var model = new ProductViewModel();
            var entity = _productservice.GetProduct(id);
            if (entity != null) model = entity;
            #region List Category
            List<ProductCategoryViewModel> productCategories = _productservice.GetProductCategories();
            productCategories.Add(new ProductCategoryViewModel() { CateId = null, Name = "--- Chọn ---" });
            var slProductCategories = productCategories.OrderBy(x => x.CateId).ToList().Select(x => new SelectListItem { Value = x.CateId.ToString(), Text = x.Name });
            ViewBag.ProductCategories = new SelectList(slProductCategories, "Value", "Text");

            List<BrandViewModel> brandCategories = _productservice.GetBrandCategories();
            brandCategories.Add(new BrandViewModel() { BrandId = null, BrandName = "--- Chọn ---" });
            var slBrandCategories = brandCategories.OrderBy(x => x.BrandId).ToList().Select(x => new SelectListItem { Value = x.BrandId.ToString(), Text = x.BrandName });
            ViewBag.BrandCategories = new SelectList(slBrandCategories, "Value", "Text");

            List<UnitViewModel> unitCategories = _unitService.GetUnits();
            unitCategories.Add(new UnitViewModel() { UnitId = null, Name = "--- Chọn ---" });
            var slUnitCategories = unitCategories.OrderBy(x => x.UnitId).ToList().Select(x => new SelectListItem { Value = x.UnitId.ToString(), Text = x.Name });
            ViewBag.UnitCategories = new SelectList(slUnitCategories, "Value", "Text");
            #endregion List Category

            return PartialView("_EditProduct", model);

        }

        [HttpPost]
        public ActionResult EditProduct(ProductViewModel model)
        {
            _logger.LogDebug($"Edit Product(Id: {model.Id})");
            var entity = _productservice.GetProduct(model.Id);
            if (entity != null)
            {
                _productservice.UpdateProduct(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
          
            return RedirectToAction("index");

        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel model)
        {
            _logger.LogDebug($"Edit Product(Id: {model.Id})");
            var entity = _productservice.GetProduct(model.Id);
            if (entity != null)
            {
                _productservice.UpdateProduct(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }

            return RedirectToAction("index");

        }

        [AuthorizeRoles( RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Product(Id: {id})");
            var entity = _productservice.GetProduct(id);
            if (entity != null)
            {
                _productservice.DeleteProduct(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
    }
}