using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Loyalty;
using Ats.Models.MemberWallet;
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
    public class LoyaltyPointTypeController : BaseController
    {
        private ILoyaltyPointTypeService _loyaltyPointTypeservice;
        private IConfiguration _config;
        public LoyaltyPointTypeController(UserManager<User> userManage, ILoyaltyPointTypeService loyaltyPointTypeService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _loyaltyPointTypeservice = loyaltyPointTypeService;
            _config = config;
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"LoyaltyPointType Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            LoyaltyPointTypeSearchViewModel model = new LoyaltyPointTypeSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var giftcat = _loyaltyPointTypeservice.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.LoyaltyPointTypes = giftcat;
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
        public ActionResult Create(LoyaltyPointTypeSearchViewModel model)
        {
            _logger.LogDebug($"Create (Name: {model.LoyaltyPointType.Name})");
            _loyaltyPointTypeservice.CreateLoyaltyPointType(model.LoyaltyPointType);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index");
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            _logger.LogDebug($"LoyaltyPointType Detail(Id: {id})");
            var model = new LoyaltyPointTypeSearchViewModel();
            var entity = _loyaltyPointTypeservice.GetLoyaltyPointType(id);
            if (entity != null) model.LoyaltyPointType = entity;
            return PartialView("_Detail", model);
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(LoyaltyPointTypeSearchViewModel model)
        {
            _logger.LogDebug($"Edit LoyaltyPointType(Id: {model.LoyaltyPointType.Id})");
            var entity = _loyaltyPointTypeservice.GetLoyaltyPointType(model.LoyaltyPointType.Id);
            if (entity != null)
            {
                _loyaltyPointTypeservice.UpdateLoyaltyPointType(model.LoyaltyPointType);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Index");
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete LoyaltyPointType(Id: {id})");
            var entity = _loyaltyPointTypeservice.GetLoyaltyPointType(id);
            if (entity != null)
            {
                _loyaltyPointTypeservice.DeleteLoyaltyPointType(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index");
        }
    }
}