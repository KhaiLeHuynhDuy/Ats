using Ats.Commons;
using Ats.Domain;
using Ats.Domain.Identity.Models;
using Ats.Domain.Loan.Models;
using Ats.Models;
using Ats.Models.Product;
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

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class LoanProductController : BaseController
    {
        private ILoanProductService _productService;
        private ICommonService _commonService;
        private IConfiguration _config;
        public LoanProductController(UserManager<User> userManage, ILoanProductService productService, IConfiguration config,
                        ICommonService commonService, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _productService = productService;
            _commonService = commonService;
            _config = config;
        }

        #region Product Category
        [HttpGet]
        public ActionResult ProductCategory(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Product Category Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            ProductCategorySearchViewModel model = new ProductCategorySearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("ProductCategory", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var productCategories = _productService.SearchProductCategory(model.Keyword, field, sort, page, pageSize, out count);
            model.ProductCategories = productCategories;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productCategories.Count();

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
        public ActionResult SearchProductCategory(string keyword, int pageIndex)
        {
            return RedirectToAction("ProductCategory", new { search = keyword, pageIndex = pageIndex });
        }
        [HttpPost]
        public ActionResult CreateProductCategory(ProductCategorySearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Product Category (Name: {model.ProductCategory.Name})");
            _productService.CreateProductCategory(model.ProductCategory);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("ProductCategory", new { pageIndex = pageIndex });
        }
        public ActionResult DetailProductCategory(int id)
        {
            _logger.LogDebug($"Product Category Detail(Id: {id})");
            var model = new ProductCategoryViewModel();
            var entity = _productService.GetProductCategory(id);
            if (entity != null) model = entity;
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);
            return View("EditProductCategory", model);
        }
        [HttpPost]
        public ActionResult EditProductCategory(ProductCategoryViewModel model)
        {
            _logger.LogDebug($"Edit Product Category(Id: {model.Id})");
            var entity = _productService.GetProductCategory(model.Id);
            if (entity != null)
            {
                _productService.UpdateProductCategory(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("DetailProductCategory", new { Id = model.Id });
        }
        public ActionResult DeleteProductCategory(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Product Category(Id: {id})");
            var entity = _productService.GetProductCategory(id);
            if (entity != null)
            {
                _productService.DeleteProductCategory(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);
            return RedirectToAction("ProductCategory", new { pageIndex = pageIndex });
        }
        [HttpPost]
        public ActionResult CreateProductPropertyCate(ProductCategoryViewModel model)
        {
            _logger.LogDebug($"Create Product Property (Name: {model.ProductProperty.Name})");
            _productService.CreateProductProperty(model.ProductProperty);
            return RedirectToAction("DetailProductCategory", new { Id = model.ProductProperty.ProductCategoryId});
        }
        public ActionResult DeleteProductPropertyCate(int id, int proCateId)
        {
            _logger.LogDebug($"Delete Product Property(Id: {id})");
            var entity = _productService.GetProductProperty(id);
            if (entity != null)
            {
                _productService.DeleteProductProperty(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("DetailProductCategory", new { id = proCateId });
        }
        #endregion

        #region Product Property
        [HttpGet]
        public ActionResult ProductProperty(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Product Property Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            ProductPropertySearchViewModel model = new ProductPropertySearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("ProductProperty", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var productProperties = _productService.SearchProductProperty(model.Keyword, field, sort, page, pageSize, out int count);
            model.ProductProperties = productProperties;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productProperties.Count();
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);

            List<LoanProductCategory> productCategories = _commonService.GetProductCategories();
            productCategories.Add(new LoanProductCategory() { Id = 0, Name = "--- Chọn ---" });
            var slProductCategories = productCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ProductCategories = new SelectList(slProductCategories, "Value", "Text");

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
        public ActionResult SearchProductProperty(string keyword, int pageIndex)
        {
            return RedirectToAction("ProductProperty", new { search = keyword, pageIndex = pageIndex });
        }

        [HttpPost]
        public ActionResult CreateProductProperty(ProductPropertySearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Product Property (Name: {model.ProductProperty.Name})");
            if (model.ProductProperty.ProductCategoryId == 0) model.ProductProperty.ProductCategoryId = null;
            _productService.CreateProductProperty(model.ProductProperty);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("ProductProperty", new { pageIndex = pageIndex });
        }
        public ActionResult DetailProductProperty(int id)
        {
            _logger.LogDebug($"Product Property Detail(Id: {id})");
            var model = new ProductPropertyViewModel();
            var entity = _productService.GetProductProperty(id);
            if (entity != null) model = entity;
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }

            List<LoanProductCategory> productCategories = _commonService.GetProductCategories();
            productCategories.Add(new LoanProductCategory() { Id = 0, Name = "--- Chọn ---" });
            var slProductCategories = productCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ProductCategories = new SelectList(slProductCategories, "Value", "Text");
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);

            return View("EditProductProperty", model);
        }
        [HttpPost]
        public ActionResult EditProductProperty(ProductPropertyViewModel model)
        {
            _logger.LogDebug($"Edit Product Property(Id: {model.Id})");
            if (model.ProductCategoryId == 0) model.ProductCategoryId = null;
            var entity = _productService.GetProductProperty(model.Id);
            if (entity != null)
            {
                _productService.UpdateProductProperty(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("DetailProductProperty", new { Id = model.Id });
        }
        public ActionResult DeleteProductProperty(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Product Property(Id: {id})");
            var entity = _productService.GetProductProperty(id);
            if (entity != null)
            {
                _productService.DeleteProductProperty(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("ProductProperty", new { pageIndex = pageIndex });
        }
        #endregion

        #region Product 
        [HttpGet]
        public ActionResult Product(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Product Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            LoanProductSearchViewModel model = new LoanProductSearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("Product", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var products = _productService.SearchProduct(model.Keyword, field, sort, page, pageSize, out int count);
            model.Products = products;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = products.Count();

            List<LoanProductCategory> productCategories = _commonService.GetProductCategories();
            productCategories.Add(new LoanProductCategory() { Id = 0, Name = "--- Chọn ---" });
            var slProductCategories = productCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ProductCategories = new SelectList(slProductCategories, "Value", "Text");

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
        public ActionResult CreateProduct(LoanProductSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Product (Name: {model.Product.Name})");
            if (model.Product.ProductCategoryId == 0) model.Product.ProductCategoryId = null;
            _productService.CreateProduct(model.Product);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Product", new { pageIndex = pageIndex });
        }
        public ActionResult DetailProduct(int id)
        {
            _logger.LogDebug($"Product Detail(Id: {id})");
            var model = new LoanProductViewModel();
            var entity = _productService.GetProduct(id);
            if (entity != null) model = entity;

            #region Select list
            List<LoanProductCategory> productCategories = _commonService.GetProductCategories();
            productCategories.Add(new LoanProductCategory() { Id = 0, Name = "--- Chọn ---" });
            var slProductCategories = productCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ProductCategories = new SelectList(slProductCategories, "Value", "Text");

            List<LoanProductProperty> productProperties = _commonService.GetProductProperties();
            var slProductProperties = productProperties.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ProductProperties = new SelectList(slProductProperties, "Value", "Text");
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
            if(TempData["errorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["errorMessage"].ToString();
                TempData.Remove("errorMessage");
            }
            #endregion

            return View("EditProduct", model);
        }
        [HttpPost]
        public ActionResult EditProduct(LoanProductViewModel model)
        {
            _logger.LogDebug($"Edit Product (Id: {model.Id})");
            if (model.ProductCategoryId == 0) model.ProductCategoryId = null;
            var entity = _productService.GetProduct(model.Id);
            if (entity != null)
            {
                _productService.UpdateProduct(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("DetailProduct", new { Id = model.Id });
        }
        public ActionResult DeleteProduct(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Product (Id: {id})");
            var entity = _productService.GetProduct(id);
            if (entity != null)
            {
                _productService.DeleteProduct(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Product", new { pageIndex = pageIndex });
        }
        #endregion

        #region Product Attribute
        [HttpPost]
        public IActionResult AddProductAttribute(LoanProductViewModel model)
        {
            _logger.LogDebug($"Add Product Attribute (productId: {model.ProductAttribute.ProductId}, productPropertyId: {model.ProductAttribute.ProductPropertyId})");
            if (!_productService.CheckExistAddProductAttribute(model.ProductAttribute.ProductId, model.ProductAttribute.ProductPropertyId))
            {
                _productService.AddProductAttribute(model.ProductAttribute);
                TempData["addAttributeSuccess"] = Resource.Common_notify_addAttributeSuccessfully;
            }
            else
            {
                TempData["errorMessage"] = Resource.Common_errorMessage_attributeAlreadyExisted;
            }
            return RedirectToAction("DetailProduct", new { id = model.ProductAttribute.ProductId });
        }
        [HttpGet]
        public ActionResult DetailProductAttribute(int id)
        {
            _logger.LogDebug($"Product Attribute Detail(Id: {id})");
            var model = new ProductAttributeModel();
            var entity = _productService.GetProductAttribute(id);
            if (entity != null) model = entity;
            ViewBag.DataType = Ultility.EnumToSelectList<DATA_TYPE>(false);

            return PartialView("_DetailProductAttribute", model);
        }
        [HttpPost]
        public ActionResult EditProductAttribute(ProductAttributeModel model)
        {
            _logger.LogDebug($"Edit Product Attribute(Id: {model.Id})");
            var entity = _productService.GetProductAttribute(model.Id);
            if (entity != null)
            {
                _productService.UpdateProductAttribute(model);
                TempData["updateAttributeSuccess"] = Resource.Common_notify_updateAttributeSuccessfully;
            }
            return RedirectToAction("DetailProduct", new { id = model.ProductId });
        }
        [HttpGet]
        public ActionResult DeleteProductAttribute(int id, int productId)
        {
            _logger.LogDebug($"Delete Product Attribute (Id: {id})");
            var entity = _productService.GetProductAttribute(id);
            if (entity != null)
            {
                _productService.DeleteProductAttribute(id);
                TempData["deleteAttributeSuccess"] = Resource.Common_notify_deleteAttributeSuccessfully;
            }
            return RedirectToAction("DetailProduct", new { id = productId });
        }
        #endregion
    }
}