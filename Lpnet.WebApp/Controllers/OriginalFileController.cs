using Ats.Domain.Identity.Models;
using Ats.Models.OriginalFile;
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

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class OriginalFileController : BaseController
    {
        private IOriginalFileService _originalFileService;
        private IConfiguration _config;
        public OriginalFileController(UserManager<User> userManage, IOriginalFileService originalFileService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _originalFileService = originalFileService;
            _config = config;
        }
        #region Orginal File      
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"OriginalFile Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            OriginalFileSearchViewModel model = new OriginalFileSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var originalFiles = _originalFileService.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.OriginalFiles = originalFiles;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = originalFiles.Count();

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
        [HttpPost]
        public ActionResult Create(OriginalFileSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create OriginalFile (Name: {model.OriginalFile.Name})");
            _originalFileService.CreateOriginalFile(model.OriginalFile);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        public ActionResult Detail(int id)
        {
            _logger.LogDebug($"OriginalFile Detail(Id: {id})");
            var model = new OriginalFileSearchViewModel();
            var entity = _originalFileService.GetOriginalFile(id);
            if (entity != null) model.OriginalFile = entity;
            return PartialView("_Detail", model);
        }
        [HttpPost]
        public ActionResult Edit(OriginalFileSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Edit OriginalFile(Id: {model.OriginalFile.Id})");
            var entity = _originalFileService.GetOriginalFile(model.OriginalFile.Id);
            if (entity != null)
            {
                _originalFileService.UpdateOriginalFile(model.OriginalFile);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }             
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete OriginalFile(Id: {id})");
            var entity = _originalFileService.GetOriginalFile(id);
            if (entity != null)
            {
                _originalFileService.DeleteOriginalFile(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
               
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        #endregion

        #region Orginal File Addition
        [HttpGet]
        public ActionResult IndexAddition(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"OriginalFile Addition Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            OriginalFileSearchViewModel model = new OriginalFileSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("IndexAddition", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var originalFileAdditions = _originalFileService.SearchAddition(model.Keyword, field, sort, page, pageSize, out count);
            model.OriginalFiles = originalFileAdditions;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = originalFileAdditions.Count();

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
        public ActionResult SearchAddition(string keyword, int pageIndex)
        {
            return RedirectToAction("IndexAddition", new { search = keyword, pageIndex = pageIndex });
        }
        [HttpPost]
        public ActionResult CreateAddition(OriginalFileSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create OriginalFile Addition (Name: {model.OriginalFile.Name})");
            _originalFileService.CreateOriginalFileAddition(model.OriginalFile);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("IndexAddition", new { pageIndex = pageIndex });
        }
        public ActionResult DetailAddition(int id)
        {
            _logger.LogDebug($"OriginalFile Addition Detail(Id: {id})");
            var model = new OriginalFileSearchViewModel();
            var entity = _originalFileService.GetOriginalFileAddition(id);
            if (entity != null) model.OriginalFile = entity;
            return PartialView("_DetailAddition", model);
        }
        [HttpPost]
        public ActionResult EditAddition(OriginalFileSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Edit OriginalFile Addition(Id: {model.OriginalFile.Id})");
            var entity = _originalFileService.GetOriginalFileAddition(model.OriginalFile.Id);
            if (entity != null)
            {
                _originalFileService.UpdateOriginalFileAddition(model.OriginalFile);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }               
            return RedirectToAction("IndexAddition", new { pageIndex = pageIndex });
        }
        public ActionResult DeleteAddition(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete OriginalFile Addition(Id: {id})");
            var entity = _originalFileService.GetOriginalFileAddition(id);
            if (entity != null)
            {
                _originalFileService.DeleteOriginalFileAddition(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }              
            return RedirectToAction("IndexAddition", new { pageIndex = pageIndex });
        }
        #endregion
    }
}