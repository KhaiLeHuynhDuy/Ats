using Ats.Commons;
using Ats.Commons.Utilities;
using Ats.Domain;
using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using Ats.Models.Lead;
using Ats.Models.Occupation;
using Ats.Models.Search;
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
using System.Net;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class SearchController : BaseController
    {
        private ILeadService _leadService;
        private IAddressService _addressService;
        private IOccupationService _occupationService;
        private IJobTitleService _jobTitleService;
        private ILoanProductService _loanProductService;
        private IOriginalFileService _originalFileService;
        private IConfiguration _config;
        public SearchController(UserManager<User> userManage, IAddressService addressService, ILeadService leadService, IOriginalFileService originalFileService,
                IOccupationService occupationService, IJobTitleService jobTitleService, ILoanProductService loanProductService,
                IConfiguration config, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _leadService = leadService;
            _addressService = addressService;
            _occupationService = occupationService;
            _jobTitleService = jobTitleService;
            _loanProductService = loanProductService;
            _originalFileService = originalFileService;
            _config = config;
        }

        // GET: Lead
        [HttpGet]
        public ActionResult Index(string search, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, Gender? gender, string field, bool sort, int page)
        {
            _logger.LogDebug($"Lead Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");
            Ats.Models.Search.SearchLeadViewModel model = new Ats.Models.Search.SearchLeadViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var leads = _leadService.Search(model.Keyword, province, occupation, loanProduct, loanPeriod, loanAmount, gender, field, sort, page, pageSize, out int count);
            model.Leads = leads;
            model.Pager.TotalItem = count;

            //ViewBag.Genders = Ultility.EnumToSelectList<Gender>();

            var occupations = _occupationService.GetOccupations();
            var slOccupations = occupations.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Occupations = new SelectList(slOccupations, "Value", "Text");

            var jobTitles = _jobTitleService.GetJobTitles();
            var slJobTitles = jobTitles.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.JobTitles = new SelectList(slJobTitles, "Value", "Text");

            var addresses1 = _addressService.GetAllProvince();
            var slAddresses1 = addresses1.Select(x => new SelectListItem { Value = x.ProvinceCode.ToString(), Text = x.Name });
            ViewBag.Addresses1 = new SelectList(slAddresses1, "Value", "Text");

            var addresses2 = _addressService.GetAllDistrict();
            var slAddresses2 = addresses2.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Addresses2 = new SelectList(slAddresses2, "Value", "Text");

            var loanProducts = _loanProductService.GetLoanProducts();
            var slLoanProducts = loanProducts.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.LoanProducts = new SelectList(slLoanProducts, "Value", "Text");

            return View(model);
        }

        [HttpGet]
        public ActionResult Leads(string keyword, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, string field, bool sort, int page)
        {
            _logger.LogDebug($"Lead Index Search={keyword}, Page={page}");
            if (page == 0) page += 1;
            int pageSize = _config.GetValue<int>("PageSize");

            Ats.Models.Search.SearchLeadViewModel model = new Ats.Models.Search.SearchLeadViewModel()
            {
                Keyword = keyword,
                Pager = new Ats.Models.PagerViewModel("Index", page, pageSize)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };

            var leads = _leadService.Search(model.Keyword, province, occupation, loanProduct, loanPeriod, loanAmount, null, field, sort, page, pageSize, out int count);
            model.Leads = leads;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = leads.Count();

            #region Select list
            var occupations = _occupationService.GetOccupations();
            var slOccupations = occupations.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            slOccupations = slOccupations.Append(new SelectListItem { Value = "0", Text = "--- Chọn ---" });
            ViewBag.Occupations = new SelectList(slOccupations.OrderBy(x => x.Value), "Value", "Text");

            var addresses1 = _addressService.GetAllProvince();
            var slAddresses1 = addresses1.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            slAddresses1 = slAddresses1.Append(new SelectListItem { Value = "0", Text = "--- Chọn ---" });
            ViewBag.Addresses1 = new SelectList(slAddresses1.OrderBy(x => x.Text), "Value", "Text");

            var loanProducts = _loanProductService.GetLoanProducts();
            var slLoanProducts = loanProducts.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            slLoanProducts = slLoanProducts.Append(new SelectListItem { Value = "0", Text = "--- Chọn ---" });
            ViewBag.LoanProducts = new SelectList(slLoanProducts.OrderBy(x => x.Value), "Value", "Text");

            ViewBag.LoanPeriod = Ultility.EnumToSelectList<LOAN_PERIOD>(false);
            #endregion

            model.LoanAmount = loanAmount;
            model.ProvinceId = province;
            model.LoanProductId = loanProduct;
            model.OccupationId = occupation;
            model.LoanPeriod = loanPeriod;
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchLeads(Ats.Models.Search.SearchLeadViewModel model)
        {
            return RedirectToAction("Leads", new { keyword = model.Keyword, province = model.ProvinceId, occupation = model.OccupationId, loanProduct = model.LoanProductId, loanPeriod = model.LoanPeriod, loanAmount = model.LoanAmount });
        }

        [HttpGet]
        public ActionResult View(Guid id)
        {
            LeadProfileViewModel model = _leadService.GetLeadProfile(id);

            #region Get Name
            if (model.OccupationId != null)
            {
                string occupationName = _occupationService.GetOccupation(model.OccupationId.Value).Name;
                model.Occupation = occupationName;
            }
            if (model.JobTitleId != null)
            {
                string jobTitleName = _jobTitleService.GetJobTitle(model.JobTitleId.Value).Name;
                model.JobTitle = jobTitleName;
            }
            if (model.LoanProductId != null)
            {
                string loanProductName = _loanProductService.GetLoanProduct(model.LoanProductId.Value).Name;
                model.LoanProduct = loanProductName;
            }
            if (model.OriginalFileId != null)
            {
                string originalFileName = _originalFileService.GetOriginalFile(model.OriginalFileId.Value).Name;
                model.OriginalFile = originalFileName;
            }
            if (model.OriginalFileAdditionId != null)
            {
                string originalFileAdditionName = _originalFileService.GetOriginalFileAddition(model.OriginalFileAdditionId.Value).Name;
                model.OriginalFileAddition = originalFileAdditionName;
            }
            if (model.PersonalIdRegisterPlace != null)
            {
                string personalIdRegisterPlace = _addressService.GetProvinceName(model.PersonalIdRegisterPlace.Value);
                model.PersonalIdRegisteredPlace = personalIdRegisterPlace;
            }
            #endregion

            #region ViewBag 
            if (model.MaritalStatus != null) if (model.MaritalStatus != MARITAL_STATUS.NULL) ViewBag.MaritalStatus = model.MaritalStatus.ToName();
            if (model.IncomeReceivedMethod != null) if (model.IncomeReceivedMethod != INCOME_RECEIVED_METHOD.NULL) ViewBag.IncomeReceivedMethod = model.IncomeReceivedMethod.ToName();
            if (model.VehicleRegistrationCertificate != null) if (model.VehicleRegistrationCertificate != VEHICLE_REGISTRATION_CERTIFICATE.NULL) ViewBag.VehicleRegistrationCertificate = model.VehicleRegistrationCertificate.ToName();
            if (model.HealthInsurance != null) if (model.HealthInsurance != HEALTH_INSURANCE.NULL) ViewBag.HealthInsurance = model.HealthInsurance.ToName();
            if (model.LifeInsurance != null) if (model.LifeInsurance != LIFE_INSURANCE.NULL) ViewBag.LifeInsurance = model.LifeInsurance.ToName();
            if (model.ElectricityBill != null) if (model.ElectricityBill != ELECTRICITY_BILL.NULL) ViewBag.ElectricityBill = model.ElectricityBill.ToName();
            if (model.InternetBill != null) if (model.InternetBill != INTERNET_BILL.NULL) ViewBag.InternetBill = model.InternetBill.ToName();
            if (model.SimCard != null) if (model.SimCard != SIM_CARD.NULL) ViewBag.SimCard = model.SimCard.ToName();
            if (model.BankAccount != null) if (model.BankAccount != BANK_ACCOUNT.NULL) ViewBag.BankAccount = model.BankAccount.ToName();
            if (model.LoanPurpose != null) if (model.LoanPurpose != LOAN_PURPOSE.NULL) ViewBag.LoanPurpose = model.LoanPurpose.ToName();             
            if (model.LoanPeriod != null) if(model.LoanPeriod != LOAN_PERIOD.NULL) ViewBag.LoanPeriod = model.LoanPeriod.ToName();
            if (model.PersonalIdRegisteredDate != null) ViewBag.PersonalIdRegisteredDate = Ultility.DateToString(model.PersonalIdRegisteredDate.Value);
            if (model.LeadDate != null) ViewBag.LeadDate = Ultility.DateToString(model.LeadDate);
            #endregion

            return View(model);
        }

        
    }
}