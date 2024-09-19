using Ats.Commons;
using Ats.Domain;
using Ats.Domain.Identity.Models;
using Ats.Domain.Member.Models;
using Ats.Domain.Store.Models;
using Ats.Models.Brand;
using Ats.Models.LoyaltyTier;
using Ats.Models.Member;
using Ats.Models.Product;
using Ats.Models.Store;
using Ats.Security.WebSecurity;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class MemberTagController : BaseController
    {
        private IMemberTagService _membertagservice;
        private IConfiguration _config;
        private IStoreService _storeService;
        //private IBrandService _brandService;
        private IProductService _productService;
        private ILoyaltyTierService _loyaltyTierService;


        public MemberTagController(UserManager<User> userManage, IMemberTagService service, IConfiguration config,
                         SignInManager<User> signInManager, ILoggerManager logger, IStoreService storeService
            , IProductService productService, ILoyaltyTierService loyaltyTierService) : base(userManage, signInManager, logger)
        {
            _membertagservice = service;
            _storeService = storeService;
            //_brandService = brandService;
            _productService = productService;
            _loyaltyTierService = loyaltyTierService;
            _config = config;
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Index(string search, int? TagCagetoryId, string field, bool sort, int page, int size = 0)
        {
            //_logger.LogDebug($"MemberTag Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            //int pageSize = _config.GetValue<int>("PageSize");
            MemberTagSearchViewModel model = new MemberTagSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var mebertag = _membertagservice.Search(model.Keyword, TagCagetoryId, field, sort, page, size, out int count);
            model.MemberTags = mebertag;
            model.Pager.TotalItem = count;
            model.Pager.Size = size;
            model.Pager.TotalItemInPage = mebertag.Count();

            return View(model);
        }


        public ActionResult Search(string Keyword, int? TagCagetoryId, string field, bool sort, int page, int size)//int pageIndex
        {
            return RedirectToAction("Index", new { search = Keyword, tagcagetoryid = TagCagetoryId, field, sort, page, size });// pageIndex = pageIndex
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            MemberTagViewModel model = new MemberTagViewModel();

            if (id != System.Guid.Empty)
            {
                model = _membertagservice.GetMemberTag(id);
            }

            #region List
            List<MemberTagCategory> giftCategories = await GetMemberTagCategories();
            giftCategories.Add(new MemberTagCategory() { Id = 0, TagCategoryName = "" });
            var slMemberCategories = giftCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TagCategoryName });
            ViewBag.MemberTagCategories = new SelectList(slMemberCategories, "Value", "Text");

            model.PreferredStoreList = GetStores();
            model.PreferredProductList = GetProducts();
            //model.PreferredBrandList = GetBrands();
            model.MarriedYearList = GetYear();
            model.MemberTierList = GetTiers();
            #endregion List

            #region ENUM
            model.MemberLifecycleList = Ultility.EnumToSelectList<MEMBER_LIFECYCLE>(false);
            model.BirthdayMonthList = Ultility.EnumToSelectList<NameMonths>(false);
            model.ProvinceList = Ultility.EnumToSelectList<VN_PROVINCE>(false);
            model.WeddingMonthList = Ultility.EnumToSelectList<NameMonths>(false);
            model.MarriedStatusList = Ultility.EnumToSelectList<MARITAL_STATUS>(false);


            #endregion ENUM

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MemberTagViewModel model)
        {
            if (model.Id == System.Guid.Empty)
            {
                _membertagservice.Create(model);
                TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;  

            }
            else
            {
                // update existing with member-tag-id
                _membertagservice.Update(model);
                TempData["updateProfileSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Analyst(Guid Id)
        {
            int totalMember = _membertagservice.Analyst(Id);
            TempData["totalMember"] = totalMember;
            return RedirectToAction("Edit", new { Id = Id, Total = totalMember });
        }

        private async Task<List<MemberTagCategory>> GetMemberTagCategories()
        {
            List<MemberTagCategory> giftCategories = null;
            await Task.Run(() =>
            {
                giftCategories = _membertagservice.GetMembertagCategory();
            });

            return giftCategories;
        }

        [AuthorizeRoles( RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete MemberTag(Id: {id})");
            _membertagservice.Delete(id);
            TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;

            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }
        private IList<SelectListItem> GetYear()
        {
            const int numberOfYears = 50;
            var startYear = DateTime.Now.Year;
            var endYear = startYear - numberOfYears;

            var yearList = new List<SelectListItem>();
            for (var i = startYear; i > endYear; i--)
            {
                yearList.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }

            return yearList;
        }
        private IList<SelectListItem> GetTiers()
        {
            List<LoyaltyTierViewModel> tiers = _loyaltyTierService.GetLoyaltyTiers();
            tiers.Add(new LoyaltyTierViewModel());
            var slTiers = tiers.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TierName }).ToList();

            return slTiers;

        }
        private IList<SelectListItem> GetStores()
        {
            List<StoreViewModel> stores = _storeService.GetStores();
            stores.Add(new StoreViewModel());
            var slStores = stores.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName }).ToList();

            return slStores;

        }
        private IList<SelectListItem> GetProducts()
        {
            var products = _productService.GetProducts().ToList();
            products.Add(new ProductViewModel());
            var slproducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ProductName }).ToList();

            return slproducts;

        }
        //private IList<SelectListItem> GetBrands()
        //{
        //    var brands = _brandService.GetBrands().ToList();
        //    brands.Add(new BrandViewModel());
        //    var slBrands = brands.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.BrandName }).ToList();

        //    return slBrands;

        //}

    }
}