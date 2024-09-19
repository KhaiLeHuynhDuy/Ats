using Ats.Domain.Identity.Models;
using Ats.Models.Team;
using Ats.Services;
using Ats.Services.Extensions;
using Lpnet.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ats.Domain.Account.Models;
using Ats.Commons;
using Ats.Services.Interfaces;
using static Ats.Commons.Constants;
using Ats.Security.WebSecurity;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class TeamController : BaseController
    {
        private ITeamService _teamService;
        private IAccountService _accountService;
        private IConfiguration _config;
        public TeamController(UserManager<User> userManage, ITeamService teamService, IAccountService accountService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _teamService = teamService;
            _accountService = accountService;
            _config = config;
        }

        #region Team
        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Team Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            TeamSearchViewModel model = new TeamSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var teams = _teamService.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.Teams = teams;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = teams.Count();

            #region Status message
            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
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
        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Create(TeamSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Team (Name: {model.Team.Name})");
            _teamService.CreateTeam(model.Team);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(Guid id)
        {
            _logger.LogDebug($"Team Detail(Id: {id})");
            var model = new TeamViewModel();
            var entity = _teamService.GetTeam(id);
            if (entity != null) model = entity;
            

            #region Status message
            if (TempData["updateSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateSuccess"].ToString();
                TempData.Remove("updateSuccess");
            }
            if (TempData["addUserSuccess"] != null)
            {
                ViewBag.StatusUserMessage = TempData["addUserSuccess"].ToString();
                TempData.Remove("addUserSuccess");
            }
            if (TempData["updateUserSuccess"] != null)
            {
                ViewBag.StatusUserMessage = TempData["updateUserSuccess"].ToString();
                TempData.Remove("updateUserSuccess");
            }
            if (TempData["deleteUserSuccess"] != null)
            {
                ViewBag.StatusUserMessage = TempData["deleteUserSuccess"].ToString();
                TempData.Remove("deleteUserSuccess");
            }
            #endregion

            return View("Edit", model);
        }
        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(TeamViewModel model)
        {
            _logger.LogDebug($"Edit Team(Id: {model.Id})");
            var entity = _teamService.GetTeam(model.Id);
            if (entity != null)
            {
                _teamService.UpdateTeam(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Detail", new { Id = model.Id });
        }

        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Team(Id: {id})");
            var entity = _teamService.GetTeam(id);
            if (entity != null)
            {
                _teamService.DeleteTeam(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }              
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        #endregion

        #region Team users

        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public IActionResult AddTeamUser(Guid teamId)
        {
            _logger.LogDebug($"Team User Add (Id: {teamId})");
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            var model = new TeamUserUpdateModel()
            {
                TeamId = teamId,
                TeamRole = TEAM_ROLE.UNDEFINED,
                DateFrom = DateTime.Now,
                DateTo = null
            };
                
            #region Select list
            var users = _accountService.GetActiveUsers();
            var slUsers = users.ToList().Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
            ViewBag.ActiveUsers = new SelectList(slUsers, "Value", "Text");
            if (users.Count() == 0) ViewBag.ErrorMessage = Resource.Common_errorMessage_noUser;
            #endregion

            return View(model);
        }
        [HttpPost]
        public IActionResult AddTeamUser(TeamUserUpdateModel model, string dateFrom, string dateTo)
            {
            _logger.LogDebug($"Add Team User (userId: {model.UserId}, teamId: {model.TeamId})");
            if (!_teamService.CheckExistAddTeamUser(model.TeamId, model.UserId))
            {
                if (dateFrom != null) model.DateFrom = Ultility.StringToDate(dateFrom);
                if (dateTo != null) model.DateTo = Ultility.StringToDate(dateTo);
                //Check valid date
                if (model.DateFrom > model.DateTo)
                {
                    #region Select list
                    var users = _accountService.GetActiveUsers();
                    var slUsers = users.ToList().Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
                    ViewBag.ActiveUsers = new SelectList(slUsers, "Value", "Text");
                    #endregion

                    ViewBag.ErrorMessage = Resource.Common_errorMessage_errorCompareDate;
                    return View(model);
                }
                _teamService.AddTeamUser(model);
                TempData["addUserSuccess"] = Resource.Common_notify_addUserSuccessfully;
                return RedirectToAction("Detail", new { id = model.TeamId });
            }
            else
            {
                TempData["ErrorMessage"] = Resource.Common_errorMessage_userAlreadyExisted;
                return RedirectToAction("AddTeamUser", new { teamId = model.TeamId });
            }
        }


        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public IActionResult EditTeamUser(Guid id)
        {
            _logger.LogDebug($"Team User Edit(Id: {id})");
            var model = _teamService.GetTeamUserDetail(id);

            #region Select list
            var users = _accountService.GetActiveUsers();
            var slUsers = users.ToList().Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
            ViewBag.ActiveUsers = new SelectList(slUsers, "Value", "Text");
            if (users.Count() == 0) ViewBag.ErrorMessage = Resource.Common_errorMessage_noUser;
            #endregion

            return View(model);
        }

        [HttpPost]
        public IActionResult EditTeamUser(TeamUserUpdateModel model, string dateFrom, string dateTo)
        {
            _logger.LogDebug($"Edit Team User (Id: {model.Id})");
            if (dateFrom != null) model.DateFrom = Ultility.StringToDate(dateFrom);
            if (dateTo != null) model.DateTo = Ultility.StringToDate(dateTo);
            //Check valid date
            if (model.DateFrom > model.DateTo)
            {
                #region Select list
                var users = _accountService.GetActiveUsers();
                var slUsers = users.ToList().Select(x => new SelectListItem() { Value = x.Value, Text = x.Text });
                ViewBag.ActiveUsers = new SelectList(slUsers, "Value", "Text");
                #endregion

                ViewBag.ErrorMessage = Resource.Common_errorMessage_errorCompareDate;
                return View(model);
            }
            _teamService.UpdateTeamUser(model);
            TempData["updateUserSuccess"] = Resource.Common_notify_updateUserSuccessfully;
            return RedirectToAction("Detail", new { id = model.TeamId });
        }


        [AuthorizeRoles(Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult DeleteTeamUser(Guid id, Guid teamId)
        {
            _logger.LogDebug($"Delete Team(Id: {id})");
            var entity = _teamService.GetTeamUserDetail(id);
            if (entity != null)
            {
                _teamService.DeleteTeamUser(id);
                TempData["deleteUserSuccess"] = Resource.Common_notify_deleteUserSuccessfully;
            }
            return RedirectToAction("Detail", new { id = teamId });
        }
        #endregion
    }
}