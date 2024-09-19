using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Product;
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
    public class ProductCategoryController : BaseController
    {
        private IProductCategoryService _service;
        private IConfiguration _config;
        public ProductCategoryController(UserManager<User> userManage, IProductCategoryService productCatService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _service = productCatService;
            _config = config;
        }


        [AuthorizeRoles(RoleName.SYS_PRODUCT_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Product Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            ProductCategorySearchViewModel model = new ProductCategorySearchViewModel()
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
            model.ProductCategories = productCategorys;
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
        [AuthorizeRoles(RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Create(ProductCategorySearchViewModel model, int pageIndex)
        {
            //_logger.LogDebug($"Create (Name: {model.ProductCategory.Name})");
            _service.CreateProductCategory(model.ProductCategory);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [AuthorizeRoles(RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            //_logger.LogDebug($"Occupation Detail(Id: {id})");
            var model = new ProductCategorySearchViewModel();
            var entity = _service.GetProductCategory(id);
            if (entity != null) model.ProductCategory = entity;
            return PartialView("_Detail", model);
        }
        [AuthorizeRoles(RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(ProductCategorySearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Edit Occupation(Id: {model.ProductCategory.Id})");
            var entity = _service.GetProductCategory(model.ProductCategory.Id);
            if (entity != null)
            {
                _service.UpdateProductCategory(model.ProductCategory);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }            
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [AuthorizeRoles(RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Occupation(Id: {id})");
            var entity = _service.GetProductCategory(id);
            if (entity != null)
            {
                _service.DeleteProductCategory(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }             
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
    }
}