using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models.JobTitle;
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
    public class JobTitleController : BaseController
    {
        private IJobTitleService _jobTitleService;
        private IConfiguration _config;
        public JobTitleController(UserManager<User> userManage, IJobTitleService jobTitleService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _jobTitleService = jobTitleService;
            _config = config;
        }

        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"Job Title Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            JobTitleSearchViewModel model = new JobTitleSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var jobTitles = _jobTitleService.Search(model.Keyword, field, sort, page, pageSize, out int count);
            model.JobTitles = jobTitles;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = jobTitles.Count();

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
        public ActionResult Create(JobTitleSearchViewModel model)
        {
            _logger.LogDebug($"Create Job Title (Name: {model.JobTitle.Name})");
            _jobTitleService.CreateJobTitle(model.JobTitle);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
            return RedirectToAction("Index");
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            _logger.LogDebug($"Job Title Detail(Id: {id})");
            var model = new JobTitleSearchViewModel();
            var entity = _jobTitleService.GetJobTitle(id);
            if (entity != null) model.JobTitle = entity;
            return PartialView("_Detail", model);
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult Edit(JobTitleSearchViewModel model)
        {
            _logger.LogDebug($"Edit Job Title(Id: {model.JobTitle.Id})");
            var entity = _jobTitleService.GetJobTitle(model.JobTitle.Id);
            if (entity != null)
            {
                _jobTitleService.UpdateJobTitle(model.JobTitle);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }               
            return RedirectToAction("Index");
        }


        [AuthorizeRoles(RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(int id, int pageIndex)
        {
            _logger.LogDebug($"Delete Job Title(Id: {id})");
            var entity = _jobTitleService.GetJobTitle(id);
            if (entity != null)
            {
                _jobTitleService.DeleteJobTitle(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }              
            return RedirectToAction("Index");
        }
    }
}