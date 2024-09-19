using Ats.Commons;
using Ats.Commons.Utilities;
using Ats.Domain.Identity.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Store;
using Ats.Models.Team;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class StoreController : BaseController
    {
        private IStoreService _storeservice;
        private ITeamService _teamService;
        private IConfiguration _config;
        public StoreController(UserManager<User> userManage, IStoreService storeService, ITeamService teamService,
        IConfiguration config, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _storeservice = storeService;
            _teamService = teamService;
            _config = config;
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_STORE_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Store Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            StoreSearchViewModel model = new StoreSearchViewModel()
            {
                Keyword = search,
                Pager = new PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var stores = _storeservice.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.Stores = stores;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = stores.Count();
            model.Store = new StoreViewModel()
            {
                Address = " ",
                City = " "
            };

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

        [AuthorizeRoles( RoleName.SYS_ROLE_STORE_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            _logger.LogDebug($"Store Detail(Id: {id})");
            var model = new StoreSearchViewModel();
            var entity = _storeservice.GetStore(id);
            if (entity != null) model.Store = entity;
            return PartialView("_Detail", model);
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_STORE_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            StoreViewModel model = new StoreViewModel();

            if (id != 0)
            {
                model = _storeservice.GetStore(id);
            }

            var teams = _teamService.GetTeams();
            teams.Add(new TeamViewModel { Id = Guid.Empty, Name = "--- Chọn ---" });
            var slTeams = teams.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Teams = new SelectList(slTeams, "Value", "Text");

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(StoreViewModel model, int pageIndex)
        {
            if (model.TeamId == Guid.Empty) model.TeamId = null;
            if (model.Id == 0)
            {
                _logger.LogDebug($"Create Store (Name: {model.StoreName})");
                _storeservice.CreateStore(model);
                TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            }
            else
            {
                _logger.LogDebug($"Edit Sore (Id: {model.Id})");
                _storeservice.UpdateStore(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Index",new { pageIndex = pageIndex });
          }

        [AuthorizeRoles( RoleName.SYS_ROLE_STORE_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Store (Id: {id})");
            var entity = _storeservice.GetStore(id);
            if (entity != null)
            {
                _storeservice.DeleteStore(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        #region Import

        [AuthorizeRoles( RoleName.SYS_ROLE_IMPORT_MANAGEMENT_INDEX)]
        [HttpGet]
        public ActionResult ImportPurchasedTrans()
        {
            PurchasedTransImportViewModel model = new PurchasedTransImportViewModel();

            List<SelectListItem> values = (from PURCHASEDTRANS_IMPORT d in Enum.GetValues(typeof(PURCHASEDTRANS_IMPORT))
                                           select new SelectListItem
                                           {
                                               Text = d.ToName(),
                                               Value = ((int)d).ToString()
                                           }).ToList();
            ViewBag.PurchasedTransImports = values;


            if (TempData["SuccessOrFail"] != null)
            {
                ViewBag.StatusMessage = TempData["SuccessOrFail"].ToString();
                TempData.Remove("SuccessOrFail");
            }

            if (TempData["TotalRowSuccess"] != null)
            {
                ViewBag.TotalRowSuccess = TempData["TotalRowSuccess"].ToString();
                TempData.Remove("TotalRowSuccess");
            }

            if (TempData["TotalRowFail"] != null)
            {
                ViewBag.TotalRowFail = TempData["TotalRowFail"].ToString();
                TempData.Remove("TotalRowFail");
            }
            if (TempData["ShowButtonDownload"] != null)
            {
                ViewBag.ShowButtonDownload = TempData["ShowButtonDownload"];
            }
            return View(model);
        }

        [HttpPost()]
        public async Task<IActionResult> ImportPurchasedTransFromExcelJs(PostDataAndFilePurchasedTrans DTPost)
        {
            Guid userId = GetUserIdLogin();
            PurchasedTransColumnMappingViewModel mapping = new PurchasedTransColumnMappingViewModel();
            mapping = JsonConvert.DeserializeAnonymousType(DTPost.DataInfo, mapping);
            var result = await _storeservice.ImportPurchasedTransCSV(mapping, DTPost.ImageFile, userId);
            return Ok(result);
        }

        #endregion
    }
}