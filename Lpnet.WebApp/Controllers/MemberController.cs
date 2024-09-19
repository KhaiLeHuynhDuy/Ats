using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using Ats.Services.Extensions;
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
using Ats.Commons.Utilities;
using Ats.Models.JobTitle;
using Ats.Domain.Address;
using Ats.Models.Occupation;
using Lpnet.WebApp.Resources;
using Newtonsoft.Json;
using Ats.Models.Member;
using Ats.Domain.Channel.Models;
using Ats.Domain.Store.Models;
using Ats.Services.Interfaces;
using Ats.Models.Brand;
using Ats.Models.Product;
using Ats.Domain.Member.Models;
using Ats.Domain;
using Ats.Models;
using Ats.Services;
using Ats.Models.MemberWallet;
using Ats.Models.LoyaltyTier;
using Microsoft.AspNetCore.Routing;

using System.Security.Claims;
using Ats.Security.WebSecurity;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        private IMembershipService _memberService;
        private IMemberTagService _memberTagService;
        private IMemberWalletService _memberWalletService;

        private IAddressService _addressService;
        private IOccupationService _occupationService;
        private IJobTitleService _jobTitleService;

        private IConfiguration _config;
        private IMemberChannelService _channelService;
        private IStoreService _storeService;
        //private IBrandService _brandService;
        private IProductService _productService;
        private ILoyaltyTierService _loyaltyTierService;
        private IClaimsService _claimsService;


        public MemberController(UserManager<User> userManage, IAddressService addressService, IMemberWalletService memberWallet,
            IMembershipService memberService, IMemberTagService memberTagService,
            IOccupationService occupationService, IProductService productService,
            IJobTitleService jobTitleService, IMemberChannelService channelService, IClaimsService claimsService,
            IStoreService storeService, ILoyaltyTierService loyaltyTierService,
            IConfiguration config, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _memberService = memberService;
            _memberTagService = memberTagService;
            _addressService = addressService;
            _occupationService = occupationService;
            _jobTitleService = jobTitleService;
            _channelService = channelService;
            _storeService = storeService;
            _memberWalletService = memberWallet;
           
            _productService = productService;
            _loyaltyTierService = loyaltyTierService;
            _config = config;
            _claimsService = claimsService;

        }

        // GET: Member
        [AuthorizeRoles(Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_VIEW, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public IActionResult Index(MemberSearchView memberSearch, string search, int? province, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string field, bool sort, int page, int size = 0)
        {
            //if (User.IsInRole(RoleName.SYS_ROLE_DEPARTMENT_MANAGEMENT_VIEW))
            //{
            //    return RedirectToAction("pages-404", "Pages");
            //}
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
            model.DeleteAllInactive = "false";
            var Claims = _claimsService.GetListClaims();
            var memberRegis = _memberService.GetListMembers().Count();
            if (!Claims.Equals(Constants.Claimskey.SYS_CLAIMS_KEY))
            {
                var ClaimsValue = Claims.FirstOrDefault().ClaimValue;
                int Value = Int32.Parse(ClaimsValue);
                if (memberRegis >= Value)
                {
                    model.noti = true;
                }
                else
                {
                    model.noti = false;
                }
            }

            #region Select List search
            List<OccupationViewModel> occupations = _occupationService.GetOccupations();
            occupations.Add(new OccupationViewModel { Id = 0, Name = "--- Select ---" });
            var slOccupations = occupations.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Occupations = new SelectList(slOccupations, "Value", "Text");

            List<JobTitleViewModel> jobTitles = _jobTitleService.GetJobTitles();
            jobTitles.Add(new JobTitleViewModel { Id = 0, Name = "--- Select ---" });
            var slJobTitles = jobTitles.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.JobTitles = new SelectList(slJobTitles, "Value", "Text");


            List<Province> provinces = _addressService.GetAllProvince().ToList();
            provinces.Add(new Province { Id = 0, Name = "--- Select ---" });
            var slProvinces = provinces.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Provinces = new SelectList(slProvinces, "Value", "Text");

            List<District> districts = _addressService.GetAllDistrict().ToList();
            districts.Add(new District { Id = 0, Name = "--- Select ---" });
            var slDistricts = districts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Districts = new SelectList(slDistricts, "Value", "Text");

            List<MemberChannel> ChannelCategories = _channelService.GetChannelCategories();
            ChannelCategories.Add(new MemberChannel() { Id = 0, ChannelName = "--- Select ---" });
            var slChannelCategories = ChannelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");

            List<Store> StoreCategories = _storeService.GetStoreCategories();
            StoreCategories.Add(new Store() { Id = 0, StoreName = "--- Select ---" });
            var slStoreCategories = StoreCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.StoreCategories = new SelectList(slStoreCategories, "Value", "Text");

            //List<Brand> BrandCategories = _brandService.GetBrandCategories();
            //BrandCategories.Add(new Brand() { Id = 0, BrandName = "--- Select ---" });
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
            if (TempData["updateProfileSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["updateProfileSuccess"].ToString();
                TempData.Remove("updateProfileSuccess");
            }
           
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Search(MemberSearchView memberSearch, string Keyword, int? ProvinceId, int? OccupationId, int? DistrictId, bool? gender, int? StoreId, int? ChannelId, bool? Valid, string Field, bool Sort, int page, int size, string search, string reset)
        {
            if (!string.IsNullOrEmpty(search))
            {
                return RedirectToAction("Index", new
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
                    field =Field,
                    sort=Sort,
                    page,
                    size
                });
            }
            else
            {
                return RedirectToAction("Index", new
                {
                   
                });
            }

        }

        [HttpGet("Member/OnChangePagingDropdownlist")]
        public ActionResult OnChangePagingDropdownlist(string search, int? province, int? occupation, int? district, Gender? gender, string field, bool sort, int page, int size)
        {
            string url = "/Member?search=" + search + "&province=" + province + "&occupation=" + occupation + "&district=" + district + "&field=" + field + "&sort=" + sort + "&page=" + page + "&size=" + size;
            return Json(new { url });
        }

        [AuthorizeRoles( Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Recruit()
        {
            int dfaultProvince = 0;
            int.TryParse(_config.GetValue<string>("DfaultProvince"), out dfaultProvince);

            _logger.LogDebug($"Recruitment new memnber");
            MemberRecruitmentViewModel model = new MemberRecruitmentViewModel();
            model.Year = GetYear();

         

                #region Job
                var occupations = _occupationService.GetOccupations();
            occupations.Add(new OccupationViewModel { Id = 0, Name = "--- Select ---" });
            var slOccupations = occupations.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Occupations = new SelectList(slOccupations, "Value", "Text");

            var jobTitles = _jobTitleService.GetJobTitles();
            jobTitles.Add(new JobTitleViewModel { Id = 0, Name = "--- Select ---" });
            var slJobTitles = jobTitles.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.JobTitles = new SelectList(slJobTitles, "Value", "Text");

            List<MemberChannel> ChannelCategories = _channelService.GetChannelCategories();
            ChannelCategories.Add(new MemberChannel() { Id = 0, ChannelName = "--- Select ---" });
            var slChannelCategories = ChannelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");

            List<Store> StoreCategories = _storeService.GetStoreCategories();
            StoreCategories.Add(new Store() { Id = 0, StoreName = "--- Select ---" });
            var slStoreCategories = StoreCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.StoreCategories = new SelectList(slStoreCategories, "Value", "Text");
            #endregion

            #region Address list create
            List<Province> provinces = _addressService.GetAllProvince().ToList();
            provinces.Add(new Province { Id = 0, Name = "--- Select ---" });
            var slProvinces = provinces.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Provinces = new SelectList(slProvinces, "Value", "Text", selectedValue: dfaultProvince);

            List<District> districts = _addressService.GetAllDistrict().Where(p => p.ProvinceId == model.ProvinceId || p.ProvinceId == dfaultProvince).ToList();
            districts.Add(new District { Id = 0, Name = "--- Select ---" });
            var slDistricts = districts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Districts = new SelectList(slDistricts, "Value", "Text");

            List<Country> countries = _addressService.GetAllCountry().ToList();
            countries.Add(new Country { Id = 0, Name = "--- Select ---" });
            var slCountry = countries.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Countries = new SelectList(slCountry, "Value", "Text");
            #endregion

            #region Enum
            ViewBag.ElectricityBill = Ultility.EnumToSelectList<NameMonths>(false);
            ViewBag.MaritalStatus = Ultility.EnumToSelectList<MARITAL_STATUS>(false);

            ViewBag.DocumentType = Ultility.EnumToSelectList<DOCUMENT_TYPE>(false);
            ViewBag.CommentType = Ultility.EnumToSelectList<COMMENT_TYPE>(false);

            #endregion

            #region Get Name
            if (model.OccupationId != null)
            {
                string occupationName = _occupationService.GetOccupation(model.OccupationId.Value).Name;
                ViewBag.OccupationName = occupationName;
            }
            if (model.JobTitleId != null)
            {
                string jobTitleName = _jobTitleService.GetJobTitle(model.JobTitleId.Value).Name;
                ViewBag.JobTitleName = jobTitleName;
            }
            #endregion


            if (TempData["addSuccess"] != null)
            {
                ViewBag.StatusMessage = TempData["addSuccess"].ToString();
                TempData.Remove("addSuccess");
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Recruit(MemberRecruitmentViewModel model, int pageIndex)
        {   
            #region Nullable 
            if (model.ProvinceId == 0) model.ProvinceId = null;
            if (model.DistrictId == 0) model.DistrictId = null;
            if (model.OccupationId == 0) model.OccupationId = null;
            if (model.JobTitleId == 0) model.JobTitleId = null;
            if (model.YOB == 0) model.YOB = 0;

            #endregion
            var memberCode = _memberService.GetListMembers().Any(m => m.MemberCode.Equals(model.MemberCode));
            var memberPhone = _memberService.GetListMembers().Any(m => m.PhoneNumber.Equals(model.PhoneNumber));

            var Claims = _claimsService.GetListClaims();
            var memberRegis = _memberService.GetListMembers().Count();
            if(!Claims.Equals(Constants.Claimskey.SYS_CLAIMS_KEY))
            {
                var ClaimsValue = Claims.FirstOrDefault().ClaimValue;
                int Value = Int32.Parse(ClaimsValue);
                if (memberRegis >= Value)
                {
                    TempData["ErrorMessage"] = Resource.Commom_lable_CreateMemberNoti;
                    return RedirectToAction("Recruit", model);
                }

                if (memberCode != false)
                {

                    TempData["ErrorMessage"] = Resource.Common_errorMessage_CreateMemberCodeNotFound;
                    return RedirectToAction("Recruit", model);
                }
                if (memberPhone != false)
                {

                    TempData["ErrorMessage"] = Resource.Common_errorMessage_MemberPhoneNotFound;
                    return RedirectToAction("Recruit", model);
                }
                Guid userId = GetUserIdLogin();
                _memberService.CreateMember(model, userId);
                TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;

                return RedirectToAction("Index", new { pageIndex = pageIndex });

            }
            TempData["ErrorMessage"] = Resource.Commom_lable_CreateMemberNoti;
            return RedirectToAction("Index", new { pageIndex = pageIndex });

        }

        [AuthorizeRoles(Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult EditMember(Guid id)
        {
            int dfaultProvince = 0;
            int.TryParse(_config.GetValue<string>("DfaultProvince"), out dfaultProvince);

            _logger.LogDebug($"Member Profile Detail(Id: {id})");
            MemberDetailViewModel model = _memberService.GetMemberDetail(id);

            //var brands = _brandService.GetBrands().ToList();
            //var slBrands = brands.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.BrandName });
            //ViewBag.Brands = new SelectList(slBrands, "Value", "Text");

            var products = _productService.GetProducts().ToList();
            products.Add(new ProductViewModel { Id = 0, ProductName = "--- Select ---" });
            var slproducts = products.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ProductName });
            ViewBag.Products = new SelectList(slproducts, "Value", "Text");

            #region Job
            var occupations = _occupationService.GetOccupations();
            occupations.Add(new OccupationViewModel { Id = 0, Name = "--- Select ---" });
            var slOccupations = occupations.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Occupations = new SelectList(slOccupations, "Value", "Text");

            var jobTitles = _jobTitleService.GetJobTitles();
            jobTitles.Add(new JobTitleViewModel { Id = 0, Name = "--- Select ---" });
            var slJobTitles = jobTitles.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.JobTitles = new SelectList(slJobTitles, "Value", "Text");
            #endregion

            #region Address list edit
            if (model.Province == null)
            {
                model.ProvinceId = dfaultProvince;
            }
            List<Province> provinces = _addressService.GetAllProvince().ToList();
            provinces.Add(new Province { Id = 0, Name = "--- Select ---" });
            var slProvinces = provinces.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Provinces = new SelectList(slProvinces, "Value", "Text", selectedValue: dfaultProvince);


            List<District> districts = _addressService.GetAllDistrict().Where(p => p.ProvinceId == model.ProvinceId).ToList();
            districts.Add(new District { Id = 0, Name = "--- Select ---" });
            var slDistricts = districts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Districts = new SelectList(slDistricts, "Value", "Text");

            List<MemberChannel> channelCategories = _channelService.GetChannelCategories();
            channelCategories.Add(new MemberChannel() { Id = 0, ChannelName = "--- Select ---" });
            var slChannelCategories = channelCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.ChannelName });
            ViewBag.ChannelCategories = new SelectList(slChannelCategories, "Value", "Text");

            List<Store> storeCategories = _storeService.GetStoreCategories();
            storeCategories.Add(new Store() { Id = 0, StoreName = "--- Select ---" });
            var slStoreCategories = storeCategories.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.StoreName });
            ViewBag.StoreCategories = new SelectList(slStoreCategories, "Value", "Text");
            #endregion
            model.Year = GetYear();

            #region Enum

            ViewBag.MaritalStatus = Ultility.EnumToSelectList<MARITAL_STATUS>(false);


            #endregion

            #region Member Tags
            var tags = _memberTagService.GetMemberTagsForDisplay(id);
            ViewBag.Tags = new SelectList(tags, "Value", "Text");

            #endregion

            #region Status message
            if (TempData["updateProfileSuccess"] != null)
            {
                ViewBag.StatusMessageProfile = TempData["updateProfileSuccess"].ToString();
                TempData.Remove("updateProfileSuccess");
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult EditMember(MemberDetailViewModel model)
        {
            _logger.LogDebug($"Edit Member Profile(Id: {model.Id})");

            #region Nullable 
            if (model.ProvinceId == 0) model.ProvinceId = null;
            if (model.DistrictId == 0) model.DistrictId = null;
            if (model.BrandId == 0) model.BrandId = null;
            if (model.ProductId == 0) model.ProductId = null;
            if (model.ChannelId == 0) model.ChannelId = null;
            if (model.StoreId == 0) model.StoreId = null;
            if (model.AdditionalProvinceId == 0) model.AdditionalProvinceId = null;
            if (model.AdditionalDistrictId == 0) model.AdditionalDistrictId = null;
            #endregion
            var memberId = _memberService.GetMember(model.Id).MemberCode;
            var memberPhone = _memberService.GetMember(model.Id).PhoneNumber;

            if (memberId != model.MemberCode)
            {
                var memberCode = _memberService.GetListMembers().Any(m => m.MemberCode.Equals(model.MemberCode));
                if (memberCode != false)
                {

                    TempData["ErrorMessage"] = Resource.Common_errorMessage_CreateMemberCodeNotFound;
                    return RedirectToAction("EditMember", new { Id = model.Id });
                }
            }
            if (memberPhone != model.PhoneNumber)
            {
                var phone = _memberService.GetListMembers().Any(m => m.PhoneNumber.Equals(model.PhoneNumber));
                if (phone != false)
                {

                    TempData["ErrorMessage"] = Resource.Common_errorMessage_MemberPhoneNotFound;
                    return RedirectToAction("EditMember", new { Id = model.Id });
                }
            }
            model.RegisterDate = DateTime.UtcNow;

            _memberService.UpdateMember(model);
            TempData["updateProfileSuccess"] = Resource.Common_notify_updateSuccessfully;

            return RedirectToAction("Index", new { Id = model.Id });
        }

        [AuthorizeRoles( Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        public ActionResult Delete(Guid id)
        {
            _logger.LogDebug($"Delete Member inactive(Id: {id})");
            var entity = _memberService.GetMember(id);
            if (entity != null)
            {
                _memberService.DeleteMember(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }
            return RedirectToAction("Index");
        }

        [AuthorizeRoles(Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpPost]
        public ActionResult DeleteInactiveMember(string[] deleteInputs)
        {
           
            if (deleteInputs.Length > 0)
            {   
              foreach(var idMb in deleteInputs)
                {
                 
                   _memberService.DeleteMember(Guid.Parse(idMb));
                }
                var tong = deleteInputs.Count();
                TempData["deleteSuccess"] = $"{Resource.Common_notify_deleteSuccessfully} {tong} {Resource.commom_notifi_deleteInactiveMemberSuccessfully}";
                return RedirectToAction("Index");
                //return Json(new { url = "/Member/Index", message = $"{Resource.Common_notify_deleteSuccessfully} {tong} {Resource.commom_notifi_deleteInactiveMemberSuccessfully}" });
            }
            return RedirectToAction("Index");
        }

        [AuthorizeRoles( Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Detail(Guid id, int page, int pageTow, int pageThree, int size)
        {
            _logger.LogDebug($"Member Profile Detail(Id: {id})");
            int dfaultProvince = 0;
            int.TryParse(_config.GetValue<string>("DfaultProvince"), out dfaultProvince);
            if (page == 0) page += 1;
            if (pageTow == 0) pageTow += 1;
            if (pageThree == 0) pageThree += 1;
            size = size == 0 ? _config.GetValue<int>("PageSizeDetail") : (int)size;
            var model = new MemberSearchDetailViewModel()
            {

                Pager = new Ats.Models.PagerViewModel("Detail", page, size)
                {

                },
                PagerTow = new Ats.Models.PagerTowViewModel("Detail", pageTow, size)
                {

                },
                PagerThree = new Ats.Models.PagerThreeViewModel("Detail", pageThree, size)
                {

                }
            };

            var listredeem = _memberService.RedeemPoint(id, pageTow, size, out int countRedeem);
            model.MemberWalletsHistorys = listredeem;
            model.PagerTow.TotalItem = countRedeem;
            model.PagerTow.TotalItemInPage = listredeem.Count();
            model.PagerTow.Size = size;

            var listgiftvouchercoupon = _memberService.GiftVoucherCoupon(id, pageThree, size, out int count);
            model.GiftVoucherCoupons = listgiftvouchercoupon;
            model.PagerThree.TotalItem = count;
            model.PagerThree.TotalItemInPage = listgiftvouchercoupon.Count();
            model.PagerThree.Size = size;

            var listmemberloyal = _memberService.Loyalty(id, page, size, out int countLoyal);
            model.MemberLoyaltyDetails = listmemberloyal;
            model.Pager.TotalItem = countLoyal;
            model.Pager.TotalItemInPage = listmemberloyal.Count();
            model.Pager.Size = size;


            var entity = _memberService.GetMemberDetail(id);
            if (entity != null)
                model.MemberDetailViewModel = entity;
            model.Month = entity.Month;
            model.Day = entity.Day;
            //model.ProvinceId = entity.ProvinceId;
            //model.DistrictId = entity.DistrictId;
            //model.Month = entity.Month;
            //model.Day=entity.Day;
            #region Job
            var occupations = _occupationService.GetOccupations();
            occupations.Add(new OccupationViewModel { Id = 0, Name = "--- Chọn ---" });
            var slOccupations = occupations.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Occupations = new SelectList(slOccupations, "Value", "Text");

            var jobTitles = _jobTitleService.GetJobTitles();
            jobTitles.Add(new JobTitleViewModel { Id = 0, Name = "--- Chọn ---" });
            var slJobTitles = jobTitles.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.JobTitles = new SelectList(slJobTitles, "Value", "Text");
            #endregion

            if (model.MemberDetailViewModel.Province == null)
            {
                model.MemberDetailViewModel.ProvinceId = dfaultProvince;
            }
            List<Province> provinces = _addressService.GetAllProvince().ToList();
            provinces.Add(new Province { Id = 0, Name = "--- Select ---" });
            var slProvinces = provinces.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Provinces = new SelectList(slProvinces, "Value", "Text", selectedValue: dfaultProvince);


            List<District> districts = _addressService.GetAllDistrict().Where(p => p.ProvinceId == model.MemberDetailViewModel.ProvinceId).ToList();
            districts.Add(new District { Id = 0, Name = "--- Select ---" });
            var slDistricts = districts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Districts = new SelectList(slDistricts, "Value", "Text");


            List<LoyaltyTierViewModel> tiers = _loyaltyTierService.GetLoyaltyTiers();
            tiers.Add(new LoyaltyTierViewModel { Id = 0, TierName = "--- Select ---" });
            var slTiers = tiers.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TierName }).ToList();
            ViewBag.Tiers = new SelectList(slTiers, "Value", "Text");

            model.Year = GetYear();

            #region Status message
            if (TempData["updateProfileSuccess"] != null)
            {
                ViewBag.StatusMessageProfile = TempData["updateProfileSuccess"].ToString();
                TempData.Remove("updateProfileSuccess");
            }
            if (TempData["updateCompanyInfoSuccess"] != null)
            {
                ViewBag.StatusMessageCompanyInfo = TempData["updateCompanyInfoSuccess"].ToString();
                TempData.Remove("updateCompanyInfoSuccess");
            }
            if (TempData["updateOtherInfoSuccess"] != null)
            {
                ViewBag.StatusMessageOtherInfo = TempData["updateOtherInfoSuccess"].ToString();
                TempData.Remove("updateOtherInfoSuccess");
            }
            if (TempData["updateLoanInfoSuccess"] != null)
            {
                ViewBag.StatusMessageLoanInfo = TempData["updateLoanInfoSuccess"].ToString();
                TempData.Remove("updateLoanInfoSuccess");
            }
            if (TempData["updateRemarkInfoSuccess"] != null)
            {
                ViewBag.StatusMessageRemarkInfo = TempData["updateRemarkInfoSuccess"].ToString();
                TempData.Remove("updateRemarkInfoSuccess");
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                TempData.Remove("ErrorMessage");
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult EditDetailMember(MemberSearchDetailViewModel model)
        {
            _logger.LogDebug($"Edit Member Detail Member(Id: {model.MemberDetailViewModel.Id})");

            #region Nullable 
            if (model.MemberDetailViewModel.ProvinceId == 0) model.MemberDetailViewModel.ProvinceId = null;
            if (model.MemberDetailViewModel.DistrictId == 0) model.MemberDetailViewModel.DistrictId = null;
            if (model.MemberDetailViewModel.BrandId == 0) model.MemberDetailViewModel.BrandId = null;
            if (model.MemberDetailViewModel.ProductId == 0) model.MemberDetailViewModel.ProductId = null;
            if (model.MemberDetailViewModel.JobTitleId == 0) model.MemberDetailViewModel.JobTitleId = null;
            if (model.MemberDetailViewModel.OccupationId == 0) model.MemberDetailViewModel.OccupationId = null;

            if (model.MemberDetailViewModel.AdditionalProvinceId == 0) model.MemberDetailViewModel.AdditionalProvinceId = null;
            if (model.MemberDetailViewModel.AdditionalDistrictId == 0) model.MemberDetailViewModel.AdditionalDistrictId = null;
            #endregion

            model.MemberDetailViewModel.RegisterDate = DateTime.UtcNow;
            var entity = _memberService.GetMemberDetail(model.MemberDetailViewModel.Id);
            if (entity != null)
            {
                model.MemberDetailViewModel.Month = model.Month;
                model.MemberDetailViewModel.Day = model.Day;

                var memberId = _memberService.GetMember(model.MemberDetailViewModel.Id).MemberCode;
                var memberPhone = _memberService.GetMember(model.MemberDetailViewModel.Id).PhoneNumber;

                if (memberId != model.MemberDetailViewModel.MemberCode)
                {
                    var memberCode = _memberService.GetListMembers().Any(m => m.MemberCode.Equals(model.MemberDetailViewModel.MemberCode));
                    if (memberCode != false)
                    {

                        TempData["ErrorMessage"] = Resource.Common_errorMessage_CreateMemberCodeNotFound;
                        return RedirectToAction("Detail", new { Id = model.MemberDetailViewModel.Id });
                    }
                }
                if (memberPhone != model.MemberDetailViewModel.PhoneNumber)
                {
                    var phone = _memberService.GetListMembers().Any(m => m.PhoneNumber.Equals(model.MemberDetailViewModel.PhoneNumber));
                    if (phone != false)
                    {

                        TempData["ErrorMessage"] = Resource.Common_errorMessage_MemberPhoneNotFound;
                        return RedirectToAction("Detail", new { Id = model.MemberDetailViewModel.Id });
                    }
                }
                _memberService.UpdateMemberDetail(model.MemberDetailViewModel);
                TempData["updateProfileSuccess"] = Resource.Common_notify_updateSuccessfully;
            }


            return RedirectToAction("Detail", new { Id = model.MemberDetailViewModel.Id });
        }

        public ActionResult EditLoyalty(Guid id)
        {
            var model = new MemberSearchDetailViewModel();

            var entity = _memberService.GetMemberLoyalty(id);
            if (entity != null) model.MemberLoyaltyDetail = entity;
            List<LoyaltyTierViewModel> tiers = _loyaltyTierService.GetLoyaltyTiers();
            tiers.Add(new LoyaltyTierViewModel { Id = 0, TierName = "--- Select ---" });
            var slTiers = tiers.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.TierName }).ToList();
            ViewBag.Tiers = new SelectList(slTiers, "Value", "Text");
            return PartialView("_EditLoyalty", model);
        }

        [HttpGet]
        public ActionResult ViewLoyalty(Guid id)
        {
             MemberLoyaltyDetailViewModel model = _memberService.GetMemberLoyalty(id); 

            return PartialView("_ViewLoyalty", model);
        }
        [HttpPost]
        public ActionResult CreateLoyalty(MemberSearchDetailViewModel model)
        {
            if (model.MemberLoyaltyDetail.Id == System.Guid.Empty)
            {

                _memberService.CreateMemberLoyalty(model.MemberLoyaltyDetail, model.MemberDetailViewModel.Id);
                TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;
                return RedirectToAction("Detail", new { Id = model.MemberDetailViewModel.Id });

            }
            else
            {
                var entity = _memberService.GetMemberLoyalty(model.MemberLoyaltyDetail.Id);
                if (entity != null)
                {
                    _memberService.UpdateMemberLoyalty(model.MemberLoyaltyDetail);
                    TempData["updateSuccess"] = Resource.Common_notify_updateSuccessfully;
                }
                return RedirectToAction("Detail", new { Id = model.MemberLoyaltyDetail.MemberId });
            }

        }

        [HttpPost]
        public JsonResult SearchMemberTag(string Prefix)
        {
            List<MemberTagViewModel> tags = _memberTagService.GetMemberTags();

            //Searching records from list using LINQ query  
            var Name = (from N in tags
                        where N.TagName.StartsWith(Prefix)
                        select new { N.TagName });
            return Json(Name);
        }

        #region Address JSON
        [HttpGet]
        public JsonResult UpdateDistrict(int id)
        {
            var province = _addressService.GetAllProvince().FirstOrDefault(p => p.Id == id);
            var districts = _addressService.GetAllDistrict().Where(p => p.ProvinceCode == province.ProvinceCode).ToList();
            districts.Add(new District { Id = 0, Name = "--- Chọn ---" });
            var items = districts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            return Json(new { data = items.ToList() });
        }
        #endregion

        #region Import

        [AuthorizeRoles( RoleName.SYS_ROLE_IMPORT_MANAGEMENT_INDEX, Constants.SYS_ROLE_SYSTEM_ADMINISTRATION)]
        [HttpGet]
        public ActionResult Import()
        {
            MemberImportViewModel model = new MemberImportViewModel();
            List<SelectListItem> values = (from MEMBER_IMPORT d in Enum.GetValues(typeof(MEMBER_IMPORT))
                                           select new SelectListItem
                                           {
                                               Text = d.ToName(),
                                               Value = ((int)d).ToString()
                                           }).ToList();

            ViewBag.MemberImports = values;


            if (TempData["SuccessOrFail"] != null)
            {
                ViewBag.StatusMessage = TempData["SuccessOrFail"].ToString();
                TempData.Remove("SuccessOrFail");
            }

            if (TempData["TotalRowSuccess"] != null)
            {
                ViewBag.TotalRowSuccess = TempData["TotalRowSuccess"].ToString();
                TempData.Remove("TotalRowSuccess");
            }

            if (TempData["TotalRowFail"] != null)
            {
                ViewBag.TotalRowFail = TempData["TotalRowFail"].ToString();
                TempData.Remove("TotalRowFail");
            }
            if (TempData["ShowButtonDownload"] != null)
            {
                ViewBag.ShowButtonDownload = TempData["ShowButtonDownload"];
            }
            return View(model);
        }

        [HttpPost()]
        public async Task<IActionResult> ImportMembersFromExcelJs(PostDataAndFileMember DTPost, int pageIndex)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Guid userId = GetUserIdLogin();
                MemberColumnMappingViewModel mapping = new MemberColumnMappingViewModel();
                mapping = JsonConvert.DeserializeAnonymousType(DTPost.DataInfo, mapping);
                var result = await _memberService.ImportMemberExcel(mapping, DTPost.ImageFile, userId);

                this.Ok(result);

                return Export(mapping.MemberExport);
                //return Ok(result);


            }
            catch (Exception ex)
            {
                TempData["TotalRowFail"] = ex.Message;
                return RedirectToAction("Import" ,new { index = pageIndex});
            }
          
           
        }

        public ActionResult Export(List<MemberExcelViewModel> model)
        {

                DataTable dt = new DataTable("Member");
                dt.Columns.AddRange(new DataColumn[11] { new DataColumn("Mã Thành Viên"),
                                        new DataColumn("Tên"),
                                        new DataColumn("Họ"),
                                        new DataColumn("Giới Tính"),
                                        new DataColumn("Năm Sinh"),
                                        new DataColumn("Cấp Bậc"),
                                        new DataColumn("Điểm"),
                                        new DataColumn("Ngày Đăng Ký"),
                                        new DataColumn("Tỉnh/Thành Phố"),
                                        new DataColumn("Số Điện Thoại"),
                                        new DataColumn("Trạng Thái")

                                        });

                if (model.Count > 0)
                {
                    foreach (var member in model)
                    {
                        dt.Rows.Add(member.MemberCode, member.FirstName, member.LastName, member.SEX, member.YOB, member.LoyaltyTier,
                            member.Province, member.District, member.Phone, member.Status);
                    }

                  
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);

                     return File(stream.ToArray(), "application/vnd.ms-excel", "Member.xlsx");
                       
                     }
                }

        }


        #endregion
        private IEnumerable<SelectListItem> GetYear()
        {
            const int numberOfYears = 100;
            var startYear = DateTime.Now.Year;
            var endYear = startYear - numberOfYears;

            var yearList = new List<SelectListItem>();
            for (var i = startYear; i > endYear; i--)
            {
                yearList.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }
            return yearList;
        }
    }
}