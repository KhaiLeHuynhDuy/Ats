using Ats.Domain.Identity.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Store;
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
    public class ProductCollectionItemController : BaseController
    {
        private IProductCollectionItemService _productCollectionItemservice;
        private IProductCollectionService _productCollectionservice;
        private IProductService _productservice;
        private IConfiguration _config;
        public ProductCollectionItemController(UserManager<User> userManage, IProductCollectionItemService productCollectionItemservice, IConfiguration config, 
            IProductService productservice, IProductCollectionService productCollectionservice,
        SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _productCollectionItemservice = productCollectionItemservice;
            _productservice = productservice;
            _productCollectionservice = productCollectionservice;
            _config = config;
        }

        [HttpGet]
        public ActionResult Index(string search, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo, string field, bool sort, int page)
        {
            _logger.LogDebug($"Store Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            ProductCollectionItemSearchViewModel model = new ProductCollectionItemSearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            
            int count = 0;
            var productCollectionItems = _productCollectionItemservice.Search(model.Keyword, productId, productCollectionItemId, stockFrom,stockTo, field, sort, page, pageSize, out count );
            model.ProductCollectionItems = productCollectionItems;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productCollectionItems.Count();

            #region List Category

            List<ProductViewModel> products = _productservice.GetProducts();
            products.Add(new ProductViewModel() { Id = 0, Name = "--- Chọn Sản Phẩm ---" });
            var slProducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Product = new SelectList(slProducts, "Value", "Text");

            List<ProductCollectionViewModel> productCollectionItemes = _productCollectionservice.Gets();
            productCollectionItemes.Add(new ProductCollectionViewModel() { Id = Guid.Empty, Name = "--- Chọn Tên Bộ Sưu Tập Sản Phẩm ---" });
            var slProductCollectionItems = productCollectionItemes.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ProductCollection = new SelectList(slProductCollectionItems, "Value", "Text");

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
        [HttpPost]
        public ActionResult Search(string Keyword, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo,
        string field, bool sort, int page, int size, string search, string reset)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index", new
                {
                    search = Keyword,
                    ProductId = productId,
                    ProductCollectionItemId = productCollectionItemId,
                    StockFrom = stockFrom,
                    StockTo = stockTo,

                    field,
                    sort,
                    Page = page,
                    size
                });
            }
            else
            {
                return RedirectToAction("Index", new
                {
                    field,
                    sort,
                    page,
                    size
                });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            _logger.LogDebug($"Create new Product CollectionItem");
            #region List Category
           
            List<ProductViewModel> products = _productservice.GetProducts();
            products.Add(new ProductViewModel() { Id = 0, Name = "--- Chọn Sản Phẩm ---" });
            var slProducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Product = new SelectList(slProducts, "Value", "Text");

            List<ProductCollectionViewModel> productCollectionItems = _productCollectionservice.Gets();
            productCollectionItems.Add(new ProductCollectionViewModel() { Id = Guid.Empty, Name = "--- Chọn Tên Bộ Sưu Tập Sản Phẩm ---" });
            var slProductCollectionItems = productCollectionItems.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ProductCollection = new SelectList(slProductCollectionItems, "Value", "Text");

            #endregion List Category

            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCollectionItemViewModel model, int pageIndex)
        {
            if (model.ProductId == 0) model.ProductId = null;
            if (model.ProductCollectionId == Guid.Empty) model.ProductCollectionId = null;
            _logger.LogDebug($"Create ProductCollectionItem (Name: {model})");
         
            _productCollectionItemservice.Create(model);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        public ActionResult Detail(Guid id)
        {
            _logger.LogDebug($"ProductCollectionItem Detail(Id: {id})");
            if (id == null) { return NotFound(); }
            var model = new ProductCollectionItemViewModel();
            var entity = _productCollectionItemservice.Get(id);
            if (entity != null) model = entity;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            if (id == null) { return NotFound(); }
            #region List Category

            List<ProductViewModel> products = _productservice.GetProducts();
            products.Add(new ProductViewModel() { Id = 0, Name = "--- Chọn Sản Phẩm ---" });
            var slProducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Product = new SelectList(slProducts, "Value", "Text");

            List<ProductCollectionViewModel> productCollectionItems = _productCollectionservice.Gets();
            productCollectionItems.Add(new ProductCollectionViewModel() { Id = Guid.Empty, Name = "--- Chọn Tên Bộ Sưu Tập Sản Phẩm ---" });
            var slProductCollectionItems = productCollectionItems.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.ProductCollection = new SelectList(slProductCollectionItems, "Value", "Text");

            #endregion List Category

            var model = new ProductCollectionItemViewModel();
            var productCollectionItem = _productCollectionItemservice.Get(id);
            if (productCollectionItem == null) { return NotFound(); }
            else
            {
                model = productCollectionItem;
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult Edit(ProductCollectionItemViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Edit ProductCollectionItem (Id: {model.Id})");

            if (model.ProductId == 0) model.ProductId = null;
            if (model.ProductCollectionId == Guid.Empty) model.ProductCollectionId = null;

            var entity = _productCollectionItemservice.Get(model.Id);
            if (entity != null)
            {
                _productCollectionItemservice.Update(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Product CollectionItem (Id: {id})");
            var entity = _productCollectionItemservice.Get(id);
            if (entity != null)
            {
                _productCollectionItemservice.Delete(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
    }
}