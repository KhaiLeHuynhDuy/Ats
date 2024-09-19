using Ats.Commons;
using Ats.Domain.Channel.Models;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Identity.Models;
using Ats.Domain.Store.Models;
using Ats.Domain.Voucher.Models;
using Ats.Models;
using Ats.Models.Channel;
using Ats.Models.Voucher;
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
    public class VoucherController : BaseController
    {
        private IVoucherService _voucherservice;
        private IMemberChannelService _channelService;
        private IConfiguration _config;
        public VoucherController(UserManager<User> userManage, IVoucherService service, IMemberChannelService channelService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _voucherservice = service;
            _channelService = channelService;
            _config = config;
        }

        [HttpGet]
        public ActionResult Index(string search, int? voucherCateid, int? channelid, bool? voucherType, int? stockFrom, int? stockTo, string field, bool sort, int page)
        {
            _logger.LogDebug($"Voucher Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            VoucherSearchViewModel model = new VoucherSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var vouchers = _voucherservice.Search(model.Keyword, voucherCateid, channelid, voucherType, stockFrom, stockTo, field, sort, page, pageSize, out count);
            model.Vouchers = vouchers;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = vouchers.Count();
            model.voucherType = voucherType;
            model.channelid = channelid;
            model.voucherCateid = voucherCateid;
            model.stockFrom = stockFrom;
            model.stockTo = stockTo;
            #region List Category
            List<VoucherCategory> voucherCategories = _voucherservice.GetVoucherCategories();
            voucherCategories.Add(new VoucherCategory() { Id = 0, VoucherName = "--- Chọn ---" });
            var slVoucherCategories = voucherCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.VoucherName });
            ViewBag.VoucherCategories = new SelectList(slVoucherCategories, "Value", "Text");

            List<ChannelViewModel> channelCategories = _channelService.GetChannels();
            channelCategories.Add(new ChannelViewModel() { Id = 0, ChannelName = "--- Chọn ---" });
            var slChannelCategories = channelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");
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
        public ActionResult Search(string keyword, int? voucherCateid, int? channelid, bool? voucherType, int? stockFrom, int? stockTo, int pageIndex, string search, string reset)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index", new
                {
                    search = keyword,
                    VoucherCateId= voucherCateid,
                    ChannelId = channelid,
                    VoucherType = voucherType,
                    StockFrom = stockFrom,
                    StockTo = stockTo,
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



        public ActionResult Detail(Guid id)
        {
            if (id == null) { return NotFound(); }

            var model = new VoucherViewModel();
            var coupon = _voucherservice.GetVoucher(id);
            if (coupon == null) { return NotFound(); }
            if (coupon != null) model = coupon;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            VoucherViewModel model = new VoucherViewModel();

            if (id != System.Guid.Empty)
            {
                model = _voucherservice.GetVoucher(id);
            }
            #region List Category 
            List<VoucherCategory> voucherCategories = _voucherservice.GetVoucherCategories();
            voucherCategories.Add(new VoucherCategory() { Id = 0, VoucherName = "--- Chọn ---" });
            var slVoucherCategories = voucherCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.VoucherName });
            ViewBag.VoucherCategories = new SelectList(slVoucherCategories, "Value", "Text");
            List<MemberChannel> channelCategories = _channelService.GetChannelCategories();
            channelCategories.Add(new MemberChannel() { Id = 0, ChannelName = "--- Chọn Kênh ---" });
            var slChannelCategories = channelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");
            #endregion List Category
            #region ENUM
            ViewBag.VoucherTemplate = Ultility.EnumToSelectList<VOUCHER_TEMPLATE>(false);
            #endregion
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(VoucherViewModel model)
        {

            if (model.Id == System.Guid.Empty)
            {
                _voucherservice.CreateVoucher(model);
                TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;

            }
            else
            {
                _logger.LogDebug($"Edit Voucher(Id: {model.Id})");
                var entity = _voucherservice.GetVoucher(model.Id);
                if (entity != null)
                {
                    _voucherservice.UpdateVoucher(model);
                    TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Voucher(Id: {id})");
            var entity = _voucherservice.GetVoucher(id);
            if (entity != null)
            {
                _voucherservice.DeleteVoucher(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }



        [HttpPost]
        public ActionResult Send(Guid Id, int pageIndex)
        {
            if ( Id == Guid.Empty) { return NotFound(); }  
            var ckvoucher = _voucherservice.GetVoucher( Id);
            VoucherViewModel voucher = new VoucherViewModel();

            if (ckvoucher == null) { return NotFound(); } 
            else { voucher = ckvoucher; }

            _voucherservice.Send(voucher);

            TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }




    }
}
