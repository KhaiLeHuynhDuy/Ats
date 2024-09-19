using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.LoyaltyTier;
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
    public class LoyaltyTierController : BaseController
    {
        private ILoyaltyTierService _loyaltyTierservice;
        private IConfiguration _config;
        public LoyaltyTierController(UserManager<User> userManage, ILoyaltyTierService loyaltyTierCatService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _loyaltyTierservice = loyaltyTierCatService;
            _config = config;
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"LoyaltyTier Index Search={search}, Page={page}");

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
        public ActionResult Search(string keyword, int pageIndex)
        {
            return RedirectToAction("Index", new { search = keyword, pageIndex = pageIndex });
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Create(LoyaltyTierSearchViewModel model)
        {
            _logger.LogDebug($"Create LoyaltyTier (Name: {model.LoyaltyTier.TierName})");
            _loyaltyTierservice.CreateLoyaltyTier(model.LoyaltyTier);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index");
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            _logger.LogDebug($"LoyaltyTier Detail(Id: {id})");
            var model = new LoyaltyTierSearchViewModel();
            var entity = _loyaltyTierservice.GetLoyaltyTier(id);
            if (entity != null) model.LoyaltyTier = entity;
            return PartialView("_Detail", model);
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(LoyaltyTierSearchViewModel model)
        {
            _logger.LogDebug($"Edit LoyaltyTier (Id: {model.LoyaltyTier.Id})");
            var entity = _loyaltyTierservice.GetLoyaltyTier(model.LoyaltyTier.Id);
            if (entity != null)
            {
                _loyaltyTierservice.UpdateLoyaltyTier(model.LoyaltyTier);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Index");
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete LoyaltyTier(Id: {id})");
            var entity = _loyaltyTierservice.GetLoyaltyTier(id);
            if (entity != null)
            {
                _loyaltyTierservice.DeleteLoyaltyTier(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index");
        }
    }
}
