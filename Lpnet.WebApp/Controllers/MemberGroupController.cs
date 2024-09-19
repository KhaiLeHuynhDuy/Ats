using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ats.Domain.Identity.Models;
using Ats.Domain.Member.Models;
using Ats.Models;
using Ats.Models.Member;
using Ats.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Ats.Commons.Constants;
using Microsoft.AspNetCore.Authorization;
using Ats.Security.WebSecurity;
using Ats.Commons;

namespace Lpnet.WebApp.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class MemberGroupController : BaseController
    {
        private IMemberGroupService _membergroupservice;
        private IMemberTagService _memberTagService;

        private IConfiguration _config;
        public MemberGroupController(UserManager<User> userManage, IMemberGroupService membergroupservice, IMemberTagService memberTagService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _membergroupservice = membergroupservice;
            _memberTagService = memberTagService;
            _config = config;
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, int? MemberGroupId, int? MemberTagId, string field, bool sort, int page, int size = 0)
        {
            //_logger.LogDebug($"Gift Category Index Search={search}, Page={page}");

            if (page == 0) page += 1;

            int pageSize = _config.GetValue<int>("PageSize");

            MemberGroupSearchViewModel model = new MemberGroupSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            int count = 0;
            var membergroup = _membergroupservice.Search(model.Keyword, MemberGroupId, MemberTagId, field, sort, page, pageSize, out count);
            model.MemberGroupViewModels = membergroup;
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
            _logger.LogDebug($"Member Group Detail(Id: {id})");

            List<MemberTagViewModel> tags = _memberTagService.GetMemberTags();

            MemberGroupViewModel model = new MemberGroupViewModel();

            if (id != Guid.Empty)
            {
                model = _membergroupservice.GetMemberGroup(id);
                var allTags = tags.Select(x => new Ats.Models.DisplayItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.TagName,
                    Selected = false,
                }).ToList();

                foreach(DisplayItem selectedItem in model.Tags)
                {
                    DisplayItem item = allTags.Where(x => String.Compare(x.Value, selectedItem.Value, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();
                    if(item != null)
                    {
                        item.Selected = true;
                    }
                }

                model.Tags = allTags;

                return View(model);
            }

            model.Tags = tags.Select(x => new Ats.Models.DisplayItem() { 
                Value = x.Id.ToString(),
                Text = x.TagName,
                Selected = false
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MemberGroupViewModel model)
        {
            _logger.LogDebug($"Edit Member Group(Id: {model.Id})");

            if(model.Id == Guid.Empty)
            {
                // create new
                var entity = _membergroupservice.Create(model);
            }
            else
            {
                // update group
                var entity = _membergroupservice.GetMemberGroup(model.Id);
                if (entity != null)
                {
                    _membergroupservice.Update(model);
                    TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                }
            }

            return RedirectToAction("Index", new { pageIndex = 0 });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete MemberTagCategory Category(Id: {id})");
            var entity = _membergroupservice.GetMemberGroup(id);
            if (entity != null)
            {
                _membergroupservice.Delete(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
    }
}
