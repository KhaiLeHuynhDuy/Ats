using Ats.Commons;
using Ats.Domain;
using Ats.Domain.Channel.Models;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Identity.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Coupon;
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
    public class CouponController : BaseController
    {
        private ICouponService _service;
        private IConfiguration _config;
        private IMemberChannelService _channelService;
        private IStoreService _storeService;
        public CouponController(UserManager<User> userManage, ICouponService service, IConfiguration config, IMemberChannelService channelService, IStoreService storeService,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _service = service;
            _config = config;
            _channelService = channelService;
            _storeService = storeService;
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_COUPON_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index( string search, string field, bool sort,bool? couponType, int? channelId, string? fromEffectiveDateBegin,
            string? toEffectiveDateBegin, string? fromEffectiveDateEnd,
            string? toEffectiveDateEnd, EXPIRY_COUPON? expirycoupon , int page)
        {
            _logger.LogDebug($"Coupon Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            CouponSearchViewModel model = new CouponSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0;
            var coupons = _service.Search(model.Keyword, field, sort, couponType, channelId, fromEffectiveDateBegin, toEffectiveDateBegin, fromEffectiveDateEnd,toEffectiveDateEnd, expirycoupon, page, pageSize, out count);
            model.Coupons = coupons;
            model.fromEffectiveDateBegin = fromEffectiveDateBegin;
            model.toEffectiveDateBegin = toEffectiveDateBegin;
            model.fromEffectiveDateEnd = fromEffectiveDateEnd;
            model.ChannelID = channelId;
            model.couponType = couponType;
            model.toEffectiveDateEnd = toEffectiveDateEnd;
            model.expiryCoupon = expirycoupon;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = coupons.Count();

            #region List Category
            List<CouponCategory> couponCategories = _service.GetCouponCategories();
            couponCategories.Add(new CouponCategory() { Id = 0, CouponName = "--- Chọn ---" });
            var slCouponCategories = couponCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CouponName });
            ViewBag.CouponCategories = new SelectList(slCouponCategories, "Value", "Text");

            List<MemberChannel> channelCategories = _channelService.GetChannelCategories();
            channelCategories.Add(new MemberChannel() { Id = 0, ChannelName = "--- Chọn ---" });
            var slChannelCategories = channelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");

            List<Store> storeCategories = _storeService.GetStoreCategories();
            storeCategories.Add(new Store() { Id = 0, StoreName = "--- Chọn ---" });
            var slStoreCategories = storeCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.StoreCategories = new SelectList(slStoreCategories, "Value", "Text");
            #endregion List Category

            ViewBag.Expiry_Coupon = Ultility.EnumToSelectList<EXPIRY_COUPON>(false);
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
        public ActionResult Search(string keyword, bool? couponType, int? channelId, string? fromEffectiveDateBegin,
            string? toEffectiveDateBegin, string? fromEffectiveDateEnd,
            string? toEffectiveDateEnd, EXPIRY_COUPON? expirycoupon, int pageIndex, string search, string reset)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index", new
                {
                    search = keyword,
                    couponType,
                    ChannelId = channelId,
                    FromEffectiveDateBegin = fromEffectiveDateBegin,
                    ToEffectiveDateBegin = toEffectiveDateBegin,
                    FromEffectiveDateEnd = fromEffectiveDateEnd,
                    ToEffectiveDateEnd = toEffectiveDateEnd,
                    Expirycoupon = expirycoupon,
                    pageIndex = pageIndex
                });
            }
            else
            {
                return RedirectToAction("Index", new
                {
                    
                });
            }
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_COUPON_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            #region List Category
            List<CouponCategory> couponCategories = _service.GetCouponCategories();
            couponCategories.Add(new CouponCategory() { Id = 0, CouponName = "--- Chọn Loại Phiếu Mua Hàng---" });
            var slCouponCategories = couponCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CouponName });
            ViewBag.CouponCategories = new SelectList(slCouponCategories, "Value", "Text"); 

            List<MemberChannel> channelCategories = _channelService.GetChannelCategories();
            channelCategories.Add(new MemberChannel() { Id = 0, ChannelName = "--- Chọn Kênh ---" });
            var slChannelCategories = channelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");

            List<Store> storeCategories = _storeService.GetStoreCategories(); 
            storeCategories.Add(new Store() { Id = 0, StoreName = "--- Chọn cửa hàng ---" });
            var slStoreCategories = storeCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.StoreCategories = new SelectList(slStoreCategories, "Value", "Text");
            #endregion List Category
            return View();
        }
        [HttpPost]
        public ActionResult Create(CouponViewModel model, int pageIndex)
        {
            if (model.ChannelId == 0) model.ChannelId = null;
            if (model.StoreId == 0) model.StoreId = null;
            if (model.CouponCategoryId == 0) model.CouponCategoryId = null;
            _logger.LogDebug($"Create Coupon (Name: {model.CouponName})");
            _service.Create(model);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }


        [AuthorizeRoles( RoleName.SYS_ROLE_COUPON_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(Guid id)
        {
            if (id == null) { return NotFound(); }

            var model = new CouponViewModel();
            var coupon = _service.Get(id);
            if (coupon == null) {return NotFound();}
            if (coupon != null) model = coupon;
            return View(model);
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_COUPON_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            if (id == null) { return NotFound(); }

            var model = new CouponViewModel();
            var coupon = _service.Get(id);
            if (coupon == null) {return NotFound();}
            else {model = coupon;}
            #region List Category
            List<CouponCategory> couponCategories = _service.GetCouponCategories();
            couponCategories.Add(new CouponCategory() { Id = 0, CouponName = "--- Chọn Loại Phiếu Mua Hàng---" });
            var slCouponCategories = couponCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CouponName });
            ViewBag.CouponCategories = new SelectList(slCouponCategories, "Value", "Text");
            List<MemberChannel> channelCategories = _channelService.GetChannelCategories();
            channelCategories.Add(new MemberChannel() { Id = 0, ChannelName = "--- Chọn Kênh ---" });
            var slChannelCategories = channelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");
            List<Store> storeCategories = _storeService.GetStoreCategories();
            storeCategories.Add(new Store() { Id = 0, StoreName = "--- Chọn ---" });
            var slStoreCategories = storeCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.StoreCategories = new SelectList(slStoreCategories, "Value", "Text");
            #endregion List Category
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CouponViewModel model, int pageIndex)
        {

            if (model.ChannelId == 0) model.ChannelId = null;
            if (model.StoreId == 0) model.StoreId = null;
            if (model.CouponCategoryId == 0) model.CouponCategoryId = null;

            _logger.LogDebug($"Edit Coupon(Id: {model.Id})");
            _service.Update(model);
            TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;

            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_COUPON_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Coupon(Id: {id})");
            var entity = _service.Get(id);
            if (entity != null)
            {
                _service.Delete(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }


        

    }
}