using System;
using Ats.Api.Response;
using Ats.Service.EmailEngine;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace Ats.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IAdminManagementService _adminMngService;
        private readonly ILoggerManager _logger;
        public RoleController(UserManager<Domain.Identity.Models.User> userManage, SignInManager<Domain.Identity.Models.User> signInManager,
        IAdminManagementService adminMngService,
        ILoggerManager logger
        ) : base(userManage, signInManager)
        {
            _adminMngService = adminMngService;
            _logger = logger;
        }

        //get activity
        [HttpGet]
        [Route("getall")]
        public BaseResponse<object> GetRolesByProject(Guid projectId)
        {
            var rs = new BaseResponse<object>();
            try
            {
                //rs.Data = this._adminMngService.GetRoleOfProject(projectId);
                rs.Success = true;
                rs.Message = "Success";
               
            }
            catch(Exception ex)
            {
                rs.Success = false;
                rs.Message = ex.Message;
                _logger.LogDebug("GetRolesByProject bug:" + ex.Message);
            }

            return rs;
        }
    }
}