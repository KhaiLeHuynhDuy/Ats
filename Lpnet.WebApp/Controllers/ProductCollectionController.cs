using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
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
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class ProductCollectionController : BaseController
    {
        private IProductCollectionService _productCollectionservice;
        private IProductCollectionItemService _productCollectionItemservice;
        private IProductService _productservice;
        private IConfiguration _config;
        public ProductCollectionController(UserManager<User> userManage, IProductCollectionService productCollectionservice, IProductService productService, IConfiguration config,
                         SignInManager<User> signInManager, IProductCollectionItemService productcollectionitem, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _productCollectionservice = productCollectionservice;
            _productCollectionItemservice = productcollectionitem;
            _productservice = productService;
            _config = config;
        }


        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Store Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            ProductCollectionSearchViewModel model = new ProductCollectionSearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var productCollections = _productCollectionservice.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.ProductCollections = productCollections;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productCollections.Count();


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


        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Create()
        {
            _logger.LogDebug($"Create new Product Collection");

            return View();
        }
        [HttpPost]
        public ActionResult Create(ProductCollectionViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create ProductCollection (Name: {model.Name})");
            _productCollectionservice.Create(model);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(Guid id, Guid iditem)
        {
            _logger.LogDebug($"ProductCollection Detail(Id: {id})");
            if (id == null) { return NotFound(); }

            var model = new ProductCollectionSearchViewModel();
            var entity = _productCollectionservice.Get(id, iditem);
            if (entity != null) model.ProductCollection = entity;
            model.ProductCollectionItems = _productCollectionservice.GetItems(id);
            //model.ProductCollectionItems = _productCollectionservice.GetProductsCollectionItem(id);
            return View(model);
        }




        [HttpGet]
        public ActionResult AddProduct(Guid idcollection, Guid iditem, string search, string field, bool sort, int page)
        {

            #region product
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            ProductCollectionSearchViewModel model = new ProductCollectionSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("AddProduct", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0;
            List<ProductViewModel> product = _productservice.GetProducts(); // bỏ
            var productmodel = _productservice.SearchForOrder(model.Keyword, field, sort, page, pageSize, out count);
            
            #endregion product

            // checked collection
            if (idcollection == Guid.Empty) { return NotFound(); }
            var productCollection = _productCollectionservice.Get(idcollection, iditem);
            if (productCollection == null) { return NotFound(); }



            if (iditem != Guid.Empty)  // tồn tại id Item -> Edit
            {
                var ProColItem = _productCollectionservice.Get(idcollection, iditem);
                model.ProductCollection = ProColItem;
                var item = _productCollectionItemservice.Get(iditem);
                if (item != null) { model.ProductCollectionItem = item; }
                // truyền list có selected vào.  
                var allItems = productmodel.Select(x => new Ats.Models.DisplayItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = false,
                }).ToList();
                foreach (DisplayItem selectedItem in model.ProductCollection.Items)
                {
                    DisplayItem iitteemm = allItems.Where(x => String.Compare(x.Value, selectedItem.Value, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();
                    if (iitteemm != null)
                    {
                        iitteemm.Selected = true;
                    }
                }
                model.Pager.TotalItem = count;
                model.Pager.TotalItemInPage = productmodel.Count();
                model.ProductCollection.Items = allItems;
                return View(model);
            }  // tồn tại id Item -> Edit

            model.ProductCollection = productCollection;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = productmodel.Count();


            model.Products = productmodel;
            model.ProductCollection.Items = productmodel.Select(x => new Ats.Models.DisplayItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Price = x.RetailPrice,
                Selected = false
            }).ToList();

            return View(model);
        }


 

        [HttpPost]
        public ActionResult AddProduct(ProductCollectionSearchViewModel model, Guid iditem)
        {
            if (model.ProductCollection.Id == null) { return NotFound(); }
            var productCollection = _productCollectionservice.Get(model.ProductCollection.Id, iditem);
            if (productCollection == null) { return NotFound(); }



            _productCollectionservice.CreateProductItem(model);

            return RedirectToAction("Edit", new { id = model.ProductCollection.Id });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Edit(Guid id, Guid iditem, string search, string field, bool sort, int page, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo)
        {

            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            ProductCollectionSearchViewModel model = new ProductCollectionSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("EditOrder", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0;


            if (id == null) { return NotFound(); }

            var listitemsearch = _productCollectionItemservice.SearchItemOfCollect(model.Keyword, productId, productCollectionItemId, stockFrom, stockTo, field, sort, page, pageSize, out count, id);
            var productCollection = _productCollectionservice.Get(id, iditem); 
            model.ProductCollectionItems = listitemsearch;

            if (productCollection == null) { return NotFound(); }
            else
            {
                model.ProductCollection = productCollection; 
                model.Pager.TotalItem = count; 
                model.Pager.TotalItemInPage = listitemsearch.Count(); 
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCollectionSearchViewModel model, Guid iditem, int pageIndex)
        {
            _logger.LogDebug($"Edit ProductCollection (Id: {model.ProductCollection.Id})");
            var entity = _productCollectionservice.Get(model.ProductCollection.Id, iditem);
            if (entity != null)
            {
                _productCollectionservice.Update(model.ProductCollection);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        public ActionResult SearchItem(string keyword, int pageIndex)
        {
            return RedirectToAction("Edit", new { search = keyword, pageIndex = pageIndex });
        }
        public ActionResult SearchProduct(string Keyword, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo,
        string field, bool sort, int page, int size, string search, string reset, Guid id)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Edit", new
                {
                    search = Keyword,
                    ProductId = productId,
                    ProductCollectionItemId = productCollectionItemId,
                    id,
                    field,
                    sort,
                    Page = page,
                    size
                });
            }
            else
            {
                return RedirectToAction("Edit", new
                {
                    field,
                    sort,
                    page,
                    size
                });
            }
        }
        public ActionResult SearchAllProduct(string Keyword, int? productId, Guid? productCollectionItemId, int? stockFrom, int? stockTo,
        string field, bool sort, int page, int size, string search, string reset, Guid idcollection)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return RedirectToAction("AddProduct", new
                {
                    search = Keyword,
                    ProductId = productId,
                    ProductCollectionItemId = productCollectionItemId,
                    idcollection,
                    field,
                    sort,
                    Page = page,
                    size
                });
            }
            else
            {
                return RedirectToAction("AddProduct", new
                {
                    field,
                    sort,
                    page,
                    size
                });
            }
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, Guid iditem, int pageIndex)
        {
            _logger.LogDebug($"Delete Store (Id: {id})"); 
            _productCollectionservice.Delete(id, iditem);
            TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully; 
            if(iditem != Guid.Empty)
            {
                return RedirectToAction("Edit", new { pageIndex = pageIndex, id });
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
    }
}