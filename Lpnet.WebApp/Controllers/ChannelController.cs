using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Channel;
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
    public class ChannelController : BaseController
    {
        private IMemberChannelService _channelservice;
        private IConfiguration _config;
        public ChannelController(UserManager<User> userManage, IMemberChannelService channelService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _channelservice = channelService;
            _config = config;
        }
        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Channel Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            ChannelSearchViewModel model = new ChannelSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            //var results = _service.Search(model.Keyword, field, sort, page, pageSize, out count);
            //model.Result = results;
            //model.Pager.TotalItem = count;
            //model.Pager.TotalItemInPage = results.Count();
            var channels = _channelservice.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.Channels = channels;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = channels.Count();

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
        public ActionResult Create(ChannelSearchViewModel model)
        {
            _logger.LogDebug($"Create Channel (Name: {model.Channel.ChannelName})");
            _channelservice.CreateChannel(model.Channel);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index");
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            _logger.LogDebug($"Channel Detail(Id: {id})");
            var model = new ChannelSearchViewModel();
            var entity = _channelservice.GetChannel(id);
            if (entity != null) model.Channel = entity;
            return PartialView("_Detail", model);
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(ChannelSearchViewModel model)
        {
            _logger.LogDebug($"Edit Channel(Id: {model.Channel.Id})");
            var entity = _channelservice.GetChannel(model.Channel.Id);
            if (entity != null)
            {
                _channelservice.UpdateChannel(model.Channel);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }            
            return RedirectToAction("Index");
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Channel(Id: {id})");
            var entity = _channelservice.GetChannel(id);
            if (entity != null)
            {
                _channelservice.DeleteChannel(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }             
            return RedirectToAction("Index");
        }
    }
}