using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Domain.Member.Models;
using Ats.Models;
using Ats.Models.Member;
using Ats.Security.WebSecurity;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Ats.Commons.Constants;

namespace Lpnet.WebApp.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class MemberSegmentController : BaseController
    {
        private IMemberSegmentService _memberSegmentService;
        private IMemberTagService _memberTagService;

        private IConfiguration _config;
        public MemberSegmentController(UserManager<User> userManage, IMemberSegmentService memberSegmentService, IMemberTagService memberTagService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _memberSegmentService = memberSegmentService;
            _memberTagService = memberTagService;
            _config = config;
        }


        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page, int size = 0)
        {
            _logger.LogDebug($"Member Segment Searach: {search}, Page: {page}, Size: {size}");

            if (page == 0) page += 1;
            if(size == 0)
                size = _config.GetValue<int>("PageSize");

            MemberSegmentSearchViewModel model = new MemberSegmentSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var membergroup = _memberSegmentService.Search(model.Keyword, field, sort, page, size, out count);
            model.Result= membergroup;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = membergroup.Count();

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

        public ActionResult Search(string Keyword, int? MemberGroupId, int? MemberTagId, string field, bool sort, int page, int size)//int pageIndex
        {
            return RedirectToAction("Index", new { search = Keyword,membergroupid = MemberGroupId,membertagid = MemberTagId,   field, sort, page, size });// pageIndex = pageIndex
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            _logger.LogDebug($"Member Segment (Id: {id})");

            MemberSegmentViewModel model = new MemberSegmentViewModel();

            if (id != Guid.Empty)
            {
                model = _memberSegmentService.GetMemberSegment(id);
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MemberSegmentViewModel model)
        {
            _logger.LogDebug($"Edit Member Segment (Id: {model.Id})");

            if(model.Id == Guid.Empty)
            {
                // create new
                var entity = _memberSegmentService.Create(model);
            }
            else
            {
                // update group
                _memberSegmentService.Update(model);
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
            }

            return RedirectToAction("Index", new { pageIndex = 0 });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Member Segment(Id: {id})");
            _memberSegmentService.Delete(id);
            TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
    }
}
