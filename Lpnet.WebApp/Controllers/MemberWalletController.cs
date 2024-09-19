using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Models.Account;
using Ats.Models.MemberWallet;
using Ats.Security.WebSecurity;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using static Ats.Commons.Constants;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class MemberWalletController : BaseController
    {
        private IMemberWalletService _memberwalletService;
        private IConfiguration _config;
        public MemberWalletController(UserManager<User> userManage, IAddressService addressService, IMemberWalletService memberwalletService,                
                IConfiguration config, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _memberwalletService = memberwalletService;            
            _config = config;
        }


        [AuthorizeRoles( RoleName.SYS_ROLE_WALLETS_MANAGEMENT_INDEX, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, string field, bool sort, int page)
        {
            _logger.LogDebug($"WalletController={search}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");

            MemberWalletsSearchViewModel model = new MemberWalletsSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0;
            var memberwallets = _memberwalletService.Search(model.Keyword, field, sort, page, pageSize, out count);
            model.MemberWalletsHistory = memberwallets;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = memberwallets.Count();

            return View(model);
        }


        public ActionResult Search(string keyword, int pageIndex)
        {
            return RedirectToAction("Index", new { search = keyword, pageIndex = pageIndex });
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_WALLETS_MANAGEMENT_INDEX, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Wallet(Guid Id, string keyword, string field, bool sort, int page, int size )
        {
            _logger.LogDebug($"Wallet ={keyword}, Page={page}");
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;

            MemberWalletsSearchViewModel model = new MemberWalletsSearchViewModel()
            {
                Keyword = keyword,
                Pager = new Ats.Models.PagerViewModel("Wallet", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            int count = 0;
            var memberwalletss = _memberwalletService.GetMemberWalletHistory(Id, keyword, field, sort, page, size, out count);
            model.MemberWalletsHistory = memberwalletss;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = memberwalletss.Count();

            return View(model);
        }


       

        [HttpPost]
        public ActionResult Create(MemberWalletsSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create MemberWallet (Id: {model.MemberWallets})");
            _memberwalletService.CreateMemberWallet(model.MemberWallet);
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        

        [HttpPost]
        public ActionResult Edit(MemberWalletsSearchViewModel model, int pageIndex)
        {
            //_logger.LogDebug($"Edit MemberWallet(Id: {model.MemberWallet.Id})");
            //var entity = _memberwalletService.GetMemberWallet(model.MemberWallet);
            //if (entity != null)
            //{

            #region Nullable 
            //if (model.MemberLoyalty.StoreId == 0) model.MemberLoyalty.StoreId = null;
            //if (model.MemberLoyalty.ChannelId == 0) model.MemberLoyalty.ChannelId = null;
            //if (model.MemberLoyalty.PointTypeId == 0) model.MemberLoyalty.PointTypeId = null;
            //if (model.MemberLoyalty.CompaignId == Guid.Empty) model.MemberLoyalty.CompaignId = Guid.Empty;
            //if (model.MemberLoyalty.CouponId == Guid.Empty) model.MemberLoyalty.CouponId = null;



            #endregion
           int i= _memberwalletService.UpdateMemberWallet(model.MemberWallet);
                
            if(i==0)
            {
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                return RedirectToAction("Index", new { pageIndex = pageIndex });
            }
            else
            {
                TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                return RedirectToAction("Index", new { pageIndex = pageIndex });
            }    
            //}
            
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_WALLETS_MANAGEMENT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete MemberWallet(Id: {id})");
            var entity = _memberwalletService.GetMemberWallet(id);
            if (entity != null) _memberwalletService.DeleteMemberWallet(id);
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [HttpGet]
        public ActionResult MemberWallet(Guid id, int page)
        {
            _logger.LogDebug($"Wallet ({id}) page: {page}");

            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");

            MemberWalletTransactionViewModel model = new MemberWalletTransactionViewModel()
            {
                Pager = new Ats.Models.PagerViewModel("MemberWallet", page, pageSize)
                {
                }
            };

            var memberwallet = _memberwalletService.GetMemberWallet(id);
            model.MemberWallet = memberwallet;

            int count = 0;
            var transactions = _memberwalletService.GetTransactions(id, page, pageSize, out count);
            model.Transations = transactions;
            model.Pager.TotalItem = count;

            return View(model);
        }

  
        [HttpPost]
        public ActionResult Transfer(MemberWalletsSearchViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Transfer (From: {model.Transfer.EmailFrom}, To: {model.Transfer.EmailTo}, Amount: {model.Transfer.Amount})");
            _memberwalletService.Transfer(model.Transfer.EmailFrom, model.Transfer.EmailTo, model.Transfer.Amount);
            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

    }
}