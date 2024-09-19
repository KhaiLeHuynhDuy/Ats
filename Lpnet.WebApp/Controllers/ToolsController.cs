using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.LoyaltyTier;
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
    public class ToolsController : BaseController
    {
        private IFileImportService _fileImportService;
        private ITransactionService _transactionService;
        private IConfiguration _config;
        public ToolsController(UserManager<User> userManage, IFileImportService fileImportService, ITransactionService transactionService, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _fileImportService = fileImportService;
            _transactionService = transactionService;
            _config = config;
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_IMPORT_MANAGEMENT_INDEX, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult FileImport()
        {
            _logger.LogDebug($"Tools.FileImport loaded");
           
            return View();
        }
        [AuthorizeRoles(RoleName.SYS_ROLE_IMPORT_MANAGEMENT_INDEX, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Transactions()
        {
            _logger.LogDebug($"Tools.Transactions loaded");

            return View();
        }        
    }
}