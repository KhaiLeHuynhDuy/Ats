using Ats.Domain.Gift.Models;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Channel;
using Ats.Models.Gift;
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
    public class GiftController : BaseController
    {
        private IGiftService _giftservice;
        private IMemberChannelService _channelservice;
        private IConfiguration _config;

        public GiftController(UserManager<User> userManage, IGiftService service, IConfiguration config, IMemberChannelService channelservice,
        SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _giftservice = service;
            _config = config;
            _channelservice = channelservice;

        }

        [HttpGet] 
            public ActionResult Index(string search, int? giftcategoryId, bool? giftType, int? channelId,int? stockFrom, int? stockTo,string field, bool sort, int page, int size = 0)
        {
            _logger.LogDebug($"Gift Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;

            GiftSearchViewModel model = new GiftSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };       
            var gifts = _giftservice.Search(model.Keyword, giftcategoryId,giftType, channelId,stockFrom,stockTo,field, sort, page, size, out int count);
            model.Gifts = gifts;        
            model.Pager.Size = size;
            model.ChannelId = channelId;
            model.GiftCategoryId = giftcategoryId;
            model.GiftType = giftType;
            model.StockFrom = stockFrom;
            model.StockTo = stockTo;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = gifts.Count();

            #region List Category
            List<GiftCategory> giftCategories = _giftservice.GetGiftCategories();
            giftCategories.Add(new GiftCategory() { Id = 0, GiftName = "--- Chọn ---" });
            var slGiftCategories = giftCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GiftName });
            ViewBag.GiftCategories = new SelectList(slGiftCategories, "Value", "Text");

            List<ChannelViewModel> channels = _channelservice.GetChannels();
            channels.Add(new ChannelViewModel() { Id = 0, ChannelName = "--- Chọn Kênh ---" });
            var slChannel = channels.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.Channel = new SelectList(slChannel, "Value", "Text");
            #endregion

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
        public ActionResult Search(string Keyword, int? giftcategoryId, bool? giftType, int? channelId, int? stockFrom, int? stockTo,
        string field, bool sort, int page, int size, string search, string reset)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index", new
                {                 
                    search = Keyword,
                    GiftcategoryId = giftcategoryId,
                    ChannelId = channelId,
                    GiftType = giftType,
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
        public ActionResult Edit(Guid id)
        {
            _logger.LogDebug($"Gift Detail(Id: {id})");       
            GiftViewModel model = new GiftViewModel();

            if (id != System.Guid.Empty)
            {
                model = _giftservice.Get(id);
            }


            List<GiftCategory> giftCategories = _giftservice.GetGiftCategories();
            giftCategories.Add(new GiftCategory() { Id = 0, GiftName = "--- Chọn ---" });
            var slGiftCategories = giftCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GiftName });
            ViewBag.GiftCategories = new SelectList(slGiftCategories, "Value", "Text");

            List<ChannelViewModel> channels = _channelservice.GetChannels();
            channels.Add(new ChannelViewModel() { Id = 0, ChannelName = "--- Chọn Kênh ---" });
            var slChannel = channels.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.Channel = new SelectList(slChannel, "Value", "Text");

            return  View("Edit", model);
        }
        [HttpPost]
        public ActionResult Edit(GiftViewModel model)
        {
            if (model.Id == System.Guid.Empty)
            {
                if (model.ChannelId == 0) model.ChannelId = null;
                if (model.GiftCategoryId == 0) model.GiftCategoryId = null;
                _giftservice.Create(model);
                TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;

            }
            else
            {
                _logger.LogDebug($"Edit Gift(Id: {model.Id})");
                var entity = _giftservice.Get(model.Id);
                if (entity != null)
                {
                    _giftservice.Update(model);
                    TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                }            
            }
            return RedirectToAction("Index");
        }

        public ActionResult Detail(Guid id)
        {
            if (id == null) { return NotFound(); }

            var model = new GiftViewModel();
            var gift = _giftservice.Get(id);
            if (gift == null) { return NotFound(); }
            if (gift != null) model = gift;
            return View(model);
        }
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Gift(Id: {id})");
            var entity = _giftservice.Get(id);
            if (entity != null)
            {
                _giftservice.Delete(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }             
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        [HttpPost]
        public ActionResult Send(Guid Id, int pageIndex)
        {
            if (Id == Guid.Empty) { return NotFound(); }
            var ckgift = _giftservice.Get(Id);
            GiftViewModel gift = new GiftViewModel();

            if (ckgift == null) { return NotFound(); }
            else { gift = ckgift; }

            _giftservice.Send(gift);

            TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
    }
}