using Ats.Commons;
using Ats.Domain;
using Ats.Domain.Address;
using Ats.Domain.Channel.Models;
using Ats.Domain.Identity.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.JobTitle;
using Ats.Models.LoyaltyTier;
using Ats.Models.Member;
using Ats.Models.Occupation;
using Ats.Models.Reward;
using Ats.Services;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Lpnet.WebApp.Models;
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

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class RewardsController : BaseController
    {
        private IRewardsService _rewardsService;
        private IConfiguration _config;
        private IMembershipService _memberService;
        private IMemberTagService _memberTagService;
        private IMemberWalletService _memberWalletService;
        private IMemberChannelService _channelService;
        private IStoreService _storeService;
        //private IBrandService _brandService;
        private IAddressService _addressService;
        private IOccupationService _occupationService;
        private IJobTitleService _jobTitleService;
        private IDocumentService _documentService;
        private ICommentService _commentService;
        private ILoanBookService _loanBookService;
        private ICommonService _commonService;

        public RewardsController(UserManager<User> userManage, IRewardsService rewardsService, IConfiguration config,
                        IAddressService addressService, IMemberWalletService memberWallet, IMembershipService memberService, IMemberTagService memberTagService, ICommonService commonService, ILoanBookService loanBookService,
        IOccupationService occupationService, IProductService productService, IJobTitleService jobTitleService, IDocumentService documentService, IStoreService storeService, IMemberChannelService channelService,
        ICommentService commentService, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _rewardsService = rewardsService;
            _config = config;
            _memberService = memberService;
            _memberTagService = memberTagService;
            _addressService = addressService;
            _occupationService = occupationService;
            _jobTitleService = jobTitleService;
            _commonService = commonService;
            _documentService = documentService;
            _commentService = commentService;
            _loanBookService = loanBookService;
            _channelService = channelService;
            _storeService = storeService;
            _memberWalletService = memberWallet;
        }

        [HttpGet]
        public ActionResult Center()
        {
            _logger.LogDebug($"Rewards.Center loaded");

            return View();
        }

        [HttpGet]
        public ActionResult Redemption(SearchInfoRedemptionViewModel searchInfo, string search, int page, int size, string orderField, bool orderSort, string reset)
        {
            _logger.LogDebug($"Rewards Redemption Search={search}, Page={page}");
           
                if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            searchInfo.Pager = new PagerViewModel("Redemption", page, size)
            {
                OrderField = orderField,
                OrderSort = orderSort
            };

            var listRedemption = _rewardsService.GetListRedemption(searchInfo, out int total);
            searchInfo.Pager.TotalItem = total;
            searchInfo.Pager.TotalItemInPage = listRedemption.Count;
            var model = new SearchRedemptionViewModel
            {
                RedemptionItems = listRedemption,
                SearchInfo = searchInfo
            };
            if (!string.IsNullOrEmpty(reset))
            {
                return RedirectToAction("Redemption");
            }
            return View(model);

        }

        [HttpGet]
        public ActionResult RedemptionDetail(Guid id, GIFT_VOUCHER_COUPON type)
        {
            _logger.LogDebug($"Get Redeem Point Detail");

            var model = _rewardsService.GetRedemptionDetail(id, type);

            return PartialView("_RedemptionDetail", model);
        }

        [HttpPost]
        public IActionResult RedemptionRedeem(RedemptionItemViewModel model)
        {
            _logger.LogDebug($"Redemption Redeem");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _rewardsService.RedemptionRedeem(model);
                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult PointRedemption()
        {
            _logger.LogDebug($"Rewards.PointRedemption loaded");

            return View();
        }

        [HttpGet]
        public ActionResult CardRedemption()
        {
            _logger.LogDebug($"Rewards.CardRedemption loaded");

            return View();
        }

        [HttpGet]
        public ActionResult RedeemPoint(MemberSearchView memberSearch, string search, int? province, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId,bool? valid, string field, bool sort, int page, int size = 0)
        {
            _logger.LogDebug($"Rewards.RedeemPoint loaded");
            _logger.LogDebug($"Member Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            MemberSearchViewModel model = new MemberSearchViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var members = _memberService.Search(memberSearch, model.Keyword, province, occupation, jobtitle, district, gender, storeId, channelId, valid, field, sort, page, size, out int count);
            model.Members = members;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = members.Count();
            model.Pager.Size = size;
            model.Pager.Search = search;
            model.OccupationId = occupation;
            model.ProvinceId = province;
            model.JobTitleId = jobtitle;
            model.DistrictId = district;
            model.Valid = valid;

            #region Select List search
            List<OccupationViewModel> occupations = _occupationService.GetOccupations();
            occupations.Add(new OccupationViewModel { Id = 0, Name = "--- Chọn ---" });
            var slOccupations = occupations.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Occupations = new SelectList(slOccupations, "Value", "Text");

            List<JobTitleViewModel> jobTitles = _jobTitleService.GetJobTitles();
            jobTitles.Add(new JobTitleViewModel { Id = 0, Name = "--- Chọn ---" });
            var slJobTitles = jobTitles.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.JobTitles = new SelectList(slJobTitles, "Value", "Text");


            List<Province> provinces = _addressService.GetAllProvince().ToList();
            provinces.Add(new Province { Id = 0, Name = "--- Chọn ---" });
            var slProvinces = provinces.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Provinces = new SelectList(slProvinces, "Value", "Text");

            List<District> districts = _addressService.GetAllDistrict().ToList();
            districts.Add(new District { Id = 0, Name = "--- Chọn ---" });
            var slDistricts = districts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Districts = new SelectList(slDistricts, "Value", "Text");

            List<MemberChannel> ChannelCategories = _channelService.GetChannelCategories();
            ChannelCategories.Add(new MemberChannel() { Id = 0, ChannelName = "--- Chọn ---" });
            var slChannelCategories = ChannelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");

            List<Store> StoreCategories = _storeService.GetStoreCategories();
            StoreCategories.Add(new Store() { Id = 0, StoreName = "--- Chọn ---" });
            var slStoreCategories = StoreCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.StoreCategories = new SelectList(slStoreCategories, "Value", "Text");

            //List<Brand> BrandCategories = _brandService.GetBrandCategories();
            //BrandCategories.Add(new Brand() { Id = 0, BrandName = "--- Chọn ---" });
            //var slBrandCategories = BrandCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.BrandName });
            //ViewBag.BrandCategories = new SelectList(slBrandCategories, "Value", "Text");

            ViewBag.MaritalStatus = Ultility.EnumToSelectList<MARITAL_STATUS>(false);

            #endregion

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

        [HttpPost]
        public ActionResult SearchRedeemPoint(MemberSearchView memberSearch, string Keyword,
            int? ProvinceId, int? OccupationId, int? DistrictId, bool? gender, int? StoreId,
            int? ChannelId, bool? Valid, string field, bool sort, int page, int size, string search, string reset)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return RedirectToAction("RedeemPoint", new
                {
                    memberCode = memberSearch.MemberCode,
                    firstName = memberSearch.FirstName,
                    lastName = memberSearch.LastName,
                    email = memberSearch.Email,
                    phoneNumber = memberSearch.PhoneNumber,
                    maritalStatus = memberSearch.MaritalStatus,
                    dOB = memberSearch.DOB,
                    register = memberSearch.RegisterDate,
                    weddingDate = memberSearch.WeddingDate,
                    storeId = StoreId,
                    channelId = ChannelId,
                    valid = Valid,
                    search = Keyword,
                    province = ProvinceId,
                    occupation = OccupationId,
                    district = DistrictId,
                    gender,
                    field,
                    sort,
                    page,
                    size
                });
            }
            else
            {
                return RedirectToAction("RedeemPoint", new
                {
                    
                });
            }
        }

        [HttpGet]
        public ActionResult RedeemPointDetail(Guid memberId)
        {
            _logger.LogDebug($"Get Redeem Point Detail");

            RedeemPointDetailViewModel model = _rewardsService.GetRedeemPointDetail(memberId);

            return PartialView("_RedeemPointDetail", model);
        }

        [HttpPost]
        public IActionResult RedeemPoint(RedeemPointDetailViewModel model, int pageIndex)
        {
            _logger.LogDebug($"RedeemPoint Member: ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _rewardsService.CreateRedeemPoint(model);
                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult RedeemedDiscount(double point)
        {
            _logger.LogDebug($"Get Redeem Point Detail");

            var discount = _rewardsService.GetRedeemedDiscount(point);

            return Ok(new { Success = true, Data = discount });
        }

        [HttpPost]
        public IActionResult SendVerificationCode([FromBody] SendVerificationCodeViewModel data)
        {
            _logger.LogDebug($"RedeemedDiscount Send Verification Code");

            return Ok(new { Success = true });
        }

    }
}