using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models.Account;
using Ats.Models.Lead;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
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
    public class WalletController : BaseController
    {
        private IWalletService _walletService;        
        private IConfiguration _config;
        public WalletController(UserManager<User> userManage, IAddressService addressService, IWalletService walletService,                
                IConfiguration config, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _walletService = walletService;            
            _config = config;
        }

        [HttpGet]
        public ActionResult Index(string keyword, string field, bool sort, int page)
        {
            _logger.LogDebug($"WalletController={keyword}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");

            WalletSearchViewModel model = new WalletSearchViewModel()
            {
                Keyword = keyword,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            var wallets = _walletService.Search(model.Keyword, field, sort, page, pageSize, out int count);
            model.Wallets = wallets;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = wallets.Count();

            return View(model);
        }

        public ActionResult Search(string keyword)
        {
            return RedirectToAction("Index", new { search = keyword });
        }

        [HttpPost]
        public ActionResult Create(WalletSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Wallet (Username: {model.Wallet.UserName})");
            _walletService.CreateWallet(model.Wallet.UserName);
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        
        [HttpGet]
        public ActionResult Wallet(Guid id, int page)
        {
            _logger.LogDebug($"Wallet ({id}) page: {page}");

            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");

            WalletTransactionViewModel model = new WalletTransactionViewModel()
            {
                Pager = new Ats.Models.PagerViewModel("Wallet", page, pageSize)
                {
                }
            };

            var wallet = _walletService.GetWallet(id);
            model.Wallet = wallet;

            int count = 0;
            var transactions = _walletService.GetTransactions(id, page, pageSize, out count);
            model.Transations = transactions;
            model.Pager.TotalItem = count;

            return View(model);
        }

        [HttpPost]
        public ActionResult Transfer(WalletSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Transfer (From: {model.Transfer.EmailFrom}, To: {model.Transfer.EmailTo}, Amount: {model.Transfer.Amount})");
            _walletService.Transfer(model.Transfer.EmailFrom, model.Transfer.EmailTo, model.Transfer.Amount);
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Wallet(Id: {id})");
            var entity = _walletService.GetWallet(id);
            if (entity != null) _walletService.DeleteWallet(id);
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
    }
}