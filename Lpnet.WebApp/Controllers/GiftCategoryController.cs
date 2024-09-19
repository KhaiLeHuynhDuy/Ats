using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Gift;
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
    public class GiftCategoryController : BaseController
    {
        private IGiftCategoryService _giftcategoryservice;
        private IConfiguration _config;
        public GiftCategoryController(UserManager<User> userManage, IGiftCategoryService service, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _giftcategoryservice = service;
            _config = config;
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Gift Category Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

           GiftCategorySearchViewModel model = new GiftCategorySearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var giftcat = _giftcategoryservice.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.GiftCategorys = giftcat;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = giftcat.Count();



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
        public ActionResult Create(GiftCategorySearchViewModel model)
        {
            _logger.LogDebug($"Create (Name: {model.GiftCategory.GiftName})");
            _giftcategoryservice.Create(model.GiftCategory);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index");
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            _logger.LogDebug($"Gift Category Detail(Id: {id})");
            var model = new GiftCategorySearchViewModel();
            var entity = _giftcategoryservice.Get(id);
            if (entity != null) model.GiftCategory = entity;
            return PartialView("_Detail", model);
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(GiftCategorySearchViewModel model)
        {
            _logger.LogDebug($"Edit Gift category(Id: {model.GiftCategory.Id})");
            var entity = _giftcategoryservice.Get(model.GiftCategory.Id);
            if (entity != null)
            {
                _giftcategoryservice.Update(model.GiftCategory);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }            
            return RedirectToAction("Index");
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Gift Category(Id: {id})");
            var entity = _giftcategoryservice.Get(id);
            if (entity != null)
            {
                _giftcategoryservice.Delete(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }             
            return RedirectToAction("Index");
        }
    }
}