using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Domain.Member.Models;
using Ats.Models.Member;
using Ats.Security.WebSecurity;
using Ats.Services;
using Ats.Services.Extensions;
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
    public class MemberLifecycleController : BaseController
    {
        private IMemberLifecycleService _memberlifecycleservice;
        private IConfiguration _config;
        public MemberLifecycleController(UserManager<User> userManage, IMemberLifecycleService service, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _memberlifecycleservice = service;
            _config = config;
        }


        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogDebug($"MemberLifecycle Index");
            MemberLifecycleSearchViewModel model = new MemberLifecycleSearchViewModel();
            var memberLifecycle = _memberlifecycleservice.Search(model.Keyword);
            model.MemberLifecycles = memberLifecycle;            
            return View(model);
        }


        public ActionResult Search( string Keyword, int? TagCagetoryId, string field, bool sort, int page, int size)//int pageIndex
        {
            return RedirectToAction("Index", new { search = Keyword,  field, sort, page, size });// pageIndex = pageIndex
        }


        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Edit()
        {
            List<MemberLifecycleViewModel> model = _memberlifecycleservice.Search(String.Empty); 
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(IEnumerable<MemberLifecycleViewModel> model)
        { 
            // update
            _memberlifecycleservice.Update(model.ToList());
            //TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;  
            return RedirectToAction("Index", new { pageIndex = 0 });
        }


        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Detail(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var model = new MemberLifecycleViewModel();
            var memberLifecycle = _memberlifecycleservice.GetMemberLifecycle(id);
            if (memberLifecycle == null)
            {
                return NotFound();
            }
            if (memberLifecycle != null) model = memberLifecycle;
            return View(model);
        }



    }
}