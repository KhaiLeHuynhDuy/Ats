using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Member;
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
    public class MemberTagCategoryController : BaseController
    {
        private IMemberTagCategoryService _membertagcategoryservice;
        private IConfiguration _config;
        public MemberTagCategoryController(UserManager<User> userManage, IMemberTagCategoryService service, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _membertagcategoryservice = service;
            _config = config;
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            //_logger.LogDebug($"Gift Category Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

           MemberTagCategorySearchViewModel model = new MemberTagCategorySearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var membettagcat = _membertagcategoryservice.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.MemberTagCats = membettagcat;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = membettagcat.Count();
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
        public ActionResult Create(MemberTagCategorySearchViewModel model)
        {
            _logger.LogDebug($"Create (Name: {model.MemberTagCat.TagCategoryName})");
            _membertagcategoryservice.Create(model.MemberTagCat);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index");
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            _logger.LogDebug($"Gift Category Detail(Id: {id})");
            var model = new MemberTagCategorySearchViewModel();
            var entity = _membertagcategoryservice.Get(id);
            if (entity != null) model.MemberTagCat = entity;
            return PartialView("_Detail", model);
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(MemberTagCategorySearchViewModel model)
        {
            _logger.LogDebug($"Edit Gift category(Id: {model.MemberTagCat.Id})");
            var entity = _membertagcategoryservice.Get(model.MemberTagCat.Id);
            if (entity != null)
            {
                _membertagcategoryservice.Update(model.MemberTagCat);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }            
            return RedirectToAction("Index");
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete MemberTagCategory Category(Id: {id})");
            var entity = _membertagcategoryservice.Get(id);
            if (entity != null)
            {
                _membertagcategoryservice.Delete(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }             
            return RedirectToAction("Index");
        }
    }
}