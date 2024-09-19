﻿using Ats.Commons;
using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using Ats.Models.Lead;
using Ats.Services;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

using Ats.Commons.Utilities;
using System.Security.Permissions;
using Ats.Models.JobTitle;
using Ats.Domain.Address;
using Ats.Models.Occupation;
using System.IO;
using Lpnet.WebApp.Resources;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Build.Tasks;
using OfficeOpenXml;
using Ats.Models.OriginalFile;
using Ats.Models.Document;
using Ats.Models.Comment;
using Ats.Services.Interfaces;
using Ats.Domain;

namespace Lpnet.WebApp.Controllers
{
    [Authorize]
    public class LeadController : BaseController
    {
        private ILeadService _leadService;
        private IAddressService _addressService;
        private IOccupationService _occupationService;
        private IJobTitleService _jobTitleService;
        private ILoanProductService _loanProductService;
        private IDocumentService _documentService;
        private ICommentService _commentService;
        private ILoanBookService _loanBookService;
        private ICommonService _commonService;
        private IConfiguration _config;
        public LeadController(UserManager<User> userManage, IAddressService addressService, ILeadService leadService, ICommonService commonService, ILoanBookService loanBookService,
        IOccupationService occupationService, IJobTitleService jobTitleService, ILoanProductService loanProductService, IDocumentService documentService,
                ICommentService commentService, IConfiguration config, SignInManager<User> signInManager, ILoggerManager logger) : base(userManage, signInManager, logger)
        {
            _leadService = leadService;
            _addressService = addressService;
            _occupationService = occupationService;
            _jobTitleService = jobTitleService;
            _loanProductService = loanProductService;
            _commonService = commonService;
            _documentService = documentService;
            _commentService = commentService;
            _loanBookService = loanBookService;
            _config = config;
        }

        // GET: Lead
        [HttpGet]
        public ActionResult Index(string search, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, Gender? gender, string field, bool sort, int page, int size = 0)
        {
            _logger.LogDebug($"Lead Index Search={search}, Page={page}");
            if (page == 0) page += 1;
            size = size == 0 ? _config.GetValue<int>("PageSize") : (int)size;
            SearchLeadViewModel model = new SearchLeadViewModel()
            {
                Keyword = search,
                Pager = new Ats.Models.PagerViewModel("Index", page, size)
                {
                    OrderField = field,
                    OrderSort = sort
                }
            };
            var leads = _leadService.Search(model.Keyword, province, occupation, loanProduct, loanPeriod, loanAmount, gender, field, sort, page, size, out int count);
            model.Leads = leads;
            model.Pager.TotalItem = count;
            model.Pager.TotalItemInPage = leads.Count();
            model.Pager.Size = size;
            model.LoanAmountId = loanAmount;
            model.LoanPeriodId = loanPeriod;
            model.Pager.Search = search;
            model.LoanProductId = loanProduct;
            model.OccupationId = occupation;
            model.ProvinceId = province;

            #region Select List
            List<OccupationViewModel> occupations = _occupationService.GetOccupations();
            occupations.Add(new OccupationViewModel { Id = 0, Name = "--- Chọn ---" });
            var slOccupations = occupations.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Occupations = new SelectList(slOccupations, "Value", "Text");

            List<JobTitleViewModel> jobTitles = _jobTitleService.GetJobTitles();
            jobTitles.Add(new JobTitleViewModel { Id = 0, Name = "--- Chọn ---" });
            var slJobTitles = jobTitles.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.JobTitles = new SelectList(slJobTitles, "Value", "Text");

            List<Province> addresses1 = _addressService.GetAllProvince().ToList();
            addresses1.Add(new Province { Id = 0, Name = "--- Chọn ---" });
            var slAddresses1 = addresses1.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Addresses1 = new SelectList(slAddresses1, "Value", "Text");

            List<District> addresses2 = _addressService.GetAllDistrict().ToList();
            addresses2.Add(new District { Id = 0, Name = "--- Chọn ---" });
            var slAddresses2 = addresses2.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Addresses2 = new SelectList(slAddresses2, "Value", "Text");

            List<LoanPackage> loanProducts = _loanProductService.GetLoanProducts();
            loanProducts.Add(new LoanPackage() { Id = 0, Name = "--- Chọn ---" });
            var slLoanProducts = loanProducts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.LoanProducts = new SelectList(slLoanProducts, "Value", "Text");

            ViewBag.LoanPeriod = Ultility.EnumToSelectList<LOAN_PERIOD>(false);
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

        public ActionResult Search(string Keyword, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, Gender? gender, string field, bool sort, int page, int size)
        {
            return RedirectToAction("Index", new { search = Keyword, province, occupation, loanProduct, loanPeriod, loanAmount, gender, field, sort, page, size });
        }
        [HttpGet("Lead/OnChangePagingDropdownlist")]
        public ActionResult OnChangePagingDropdownlist(string search, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, Gender? gender, string field, bool sort, int page, int size)
        {
            string url = "/Lead?search=" + search + "&province=" + province + "&occupation=" + occupation + "&loanProduct=" + loanProduct + "&loanPeriod=" + loanPeriod + "&loanAmount=" + loanAmount + "&field=" + field + "&sort=" + sort + "&page=" + page + "&size=" + size;
            return Json(new { url });
        }

        [HttpPost]
        public ActionResult Create(SearchLeadViewModel model, int pageIndex)
        {
            _logger.LogDebug($"Create Lead (Name: {model.Lead.FirstName}, Email: {model.Lead.Email})");

            #region Nullable 
            if (model.Lead.ProvinceId == 0) model.Lead.ProvinceId = null;
            if (model.Lead.DistrictId == 0) model.Lead.DistrictId = null;
            if (model.Lead.OccupationId == 0) model.Lead.OccupationId = null;
            if (model.Lead.JobTitleId == 0) model.Lead.JobTitleId = null;
            if (model.Lead.LoanProductId == 0) model.Lead.LoanProductId = null;
            #endregion

            _leadService.CreateLead(model.Lead);
            TempData["addSuccess"] = Resource.Common_notify_addSuccessfully;

            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        [HttpGet]
        public ActionResult Profile(Guid id)
        {
            _logger.LogDebug($"Lead Profile Detail(Id: {id})");
            LeadProfileViewModel model = _leadService.GetLeadProfile(id);

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

            #region Address
            List<Province> provinces = _addressService.GetAllProvince().ToList();
            provinces.Add(new Province { Id = 0, Name = "--- Chọn ---" });
            var slProvinces = provinces.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Provinces = new SelectList(slProvinces, "Value", "Text");

            List<District> districts = _addressService.GetAllDistrict().ToList();
            districts.Add(new District { Id = 0, Name = "--- Chọn ---" });
            var slDistricts = districts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.Districts = new SelectList(slDistricts, "Value", "Text");
            #endregion

            #region Enum
            ViewBag.LoanPurpose = Ultility.EnumToSelectList<LOAN_PURPOSE>(false);
            ViewBag.IncomeReceivedMethod = Ultility.EnumToSelectList<INCOME_RECEIVED_METHOD>(false);
            ViewBag.HealthInsurance = Ultility.EnumToSelectList<HEALTH_INSURANCE>(false);
            ViewBag.ElectricityBill = Ultility.EnumToSelectList<ELECTRICITY_BILL>(false);
            ViewBag.VehicleRegistrationCertificate = Ultility.EnumToSelectList<VEHICLE_REGISTRATION_CERTIFICATE>(false);
            ViewBag.LifeInsurance = Ultility.EnumToSelectList<LIFE_INSURANCE>(false);
            ViewBag.BankAccount = Ultility.EnumToSelectList<BANK_ACCOUNT>(false);
            ViewBag.InternetBill = Ultility.EnumToSelectList<INTERNET_BILL>(false);
            ViewBag.SimCard = Ultility.EnumToSelectList<SIM_CARD>(false);
            ViewBag.MaritalStatus = Ultility.EnumToSelectList<MARITAL_STATUS>(false);
            ViewBag.LeadSource = Ultility.EnumToSelectList<LEAD_SOURCE>(false);
            ViewBag.LoanPeriod = Ultility.EnumToSelectList<LOAN_PERIOD>(false);
            ViewBag.DocumentType = Ultility.EnumToSelectList<DOCUMENT_TYPE>(false);
            ViewBag.CommentType = Ultility.EnumToSelectList<COMMENT_TYPE>(false);
            ViewBag.BookType = Ultility.EnumToSelectList<BOOK_TYPE>(false);
            #endregion

            #region Original File
            var originalFiles = _commonService.GetOriginalFiles();
            originalFiles.Add(new OriginalFile { Id = 0, Name = "--- Chọn ---" });
            var slOriginalFiles = originalFiles.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.OriginalFiles = new SelectList(slOriginalFiles, "Value", "Text");

            var originalFileAdditions = _commonService.GetOriginalFileAdditions();
            originalFileAdditions.Add(new OriginalFileAddition { Id = 0, Name = "--- Chọn ---" });
            var slOriginalFileAdditions = originalFileAdditions.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.OriginalFileAdditions = new SelectList(slOriginalFileAdditions, "Value", "Text");
            #endregion

            #region Loan
            var loanProducts = _loanProductService.GetLoanProducts();
            loanProducts.Add(new LoanPackage { Id = 0, Name = "--- Chọn ---" });
            var slLoanProducts = loanProducts.OrderBy(x => x.Id).ToList().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.LoanProducts = new SelectList(slLoanProducts, "Value", "Text");
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
            if (model.LoanProductId != null)
            {
                string loanProductName = _loanProductService.GetLoanProduct(model.LoanProductId.Value).Name;
                ViewBag.LoanProductName = loanProductName;
            }
            #endregion

            #region List additional document
            var documents = _documentService.GetListDocuments(id);
            model.Documents = documents;
            #endregion

            #region Comment
            var comments = _commentService.GetListComments(id);
            model.Comments = comments;
            #endregion

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
            #endregion
         
            return View(model);
        }

        [HttpPost]
        public ActionResult EditLeadProfile(LeadProfileViewModel model, string personalIdRegisteredDate)
        {
            _logger.LogDebug($"Edit Lead Profile(Id: {model.Id})");
            #region Nullable 
            if (model.ProvinceId == 0) model.ProvinceId = null;
            if (model.DistrictId == 0) model.DistrictId = null;
            if (model.AdditionalProvinceId == 0) model.AdditionalProvinceId = null;
            if (model.AdditionalDistrictId == 0) model.AdditionalDistrictId = null;
            if (model.PersonalIdRegisterPlace == 0) model.PersonalIdRegisterPlace = null;
            #endregion

            var entity = _leadService.GetLead(model.Id);
            if (entity != null)
            {
                if (personalIdRegisteredDate != null) model.PersonalIdRegisteredDate = Ultility.StringToDate(personalIdRegisteredDate);
                _leadService.UpdateLeadProfile(model);
                TempData["updateProfileSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Profile", new { Id = model.Id });
        }
        [HttpPost]
        public ActionResult EditLeadCompanyInfo(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Edit Lead Company(Id: {model.Id})");
            #region Nullable 
            if (model.OccupationId == 0) model.OccupationId = null;
            if (model.JobTitleId == 0) model.JobTitleId = null;
            #endregion

            var entity = _leadService.GetLead(model.Id);
            if (entity != null)
            {
                _leadService.UpdateLeadCompanyInfo(model);
                TempData["updateCompanyInfoSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
                
            return RedirectToAction("Profile", new { Id = model.Id });
        }
        [HttpPost]
        public ActionResult EditLeadOtherInfo(LeadProfileViewModel model)
        {   
            _logger.LogDebug($"Edit Lead Company(Id: {model.Id})");
            #region Nullable 
            if (model.OriginalFileId == 0) model.OriginalFileId = null;
            if (model.OriginalFileAdditionId == 0) model.OriginalFileAdditionId = null;
            #endregion

            var entity = _leadService.GetLead(model.Id);
            if (entity != null)
            { 
                _leadService.UpdateLeadOtherInfo(model);
                TempData["updateOtherInfoSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Profile", new { Id = model.Id });
        }
        [HttpPost]
        public ActionResult EditLeadLoanInfo(LeadProfileViewModel model, string loanDate)
        {
            _logger.LogDebug($"Edit Lead Loan(Id: {model.Id})");
            #region Nullable 
            if (model.LoanProductId == 0) model.LoanProductId = null; ;
            #endregion

            var entity = _leadService.GetLead(model.Id);
            if (entity != null)
            {
                if (loanDate != null) model.LeadDate = Ultility.StringToDate(loanDate);
                _leadService.UpdateLeadLoanInfo(model);
                TempData["updateLoanInfoSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Profile", new { Id = model.Id });
        }
        [HttpPost]
        public ActionResult EditLeadRemarkInfo(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Edit Lead Loan(Id: {model.Id})");
            var entity = _leadService.GetLead(model.Id);
            if (entity != null)
            {
                _leadService.UpdateLeadRemarkInfo(model);
                TempData["updateRemarkInfoSuccess"] = Resource.Common_notify_updateSuccessfully;
            }
            return RedirectToAction("Profile", new { Id = model.Id });
        }
        public ActionResult Delete(Guid id, int pageIndex)
        {
            _logger.LogDebug($"Delete Lead(Id: {id})");
            var entity = _leadService.GetLead(id);
            if (entity != null)
            {
                _leadService.DeleteLead(id);
                TempData["deleteSuccess"] = Resource.Common_notify_deleteSuccessfully;
            }

            return RedirectToAction("Index", new { pageIndex = pageIndex });
        }

        #region Additional Document
        [HttpPost]
        public ActionResult AddAdditionalFile(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Add Document (Name: {model.Document.FileName}");
            _documentService.CreateDocument(model.Document);
            return RedirectToAction("Profile", new { Id = model.Document.LeadId });
        }
        public ActionResult DeleteAdditionalFile(Guid id, Guid leadId)
        {
            _logger.LogDebug($"Delete Document(Id: {id})");
            _documentService.DeleteDocument(id);
            return RedirectToAction("Profile", new { Id = leadId });
        }
        #endregion

        #region Comment
        [HttpPost]
        public ActionResult AddComment(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Add Comment");
            model.Comment.UserId = GetUserIdLogin();
            _commentService.CreateComment(model.Comment);
            return RedirectToAction("Profile", new { Id = model.Comment.LeadId });
        }
        public ActionResult CommentDetail(Guid id)
        {
            _logger.LogDebug($"Comment Detail(Id: {id})");
            var model = new CommentViewModel();
            var entity = _commentService.GetComment(id);
            if (entity != null) model = entity;
            ViewBag.CommentType = Ultility.EnumToSelectList<COMMENT_TYPE>(false);
            return PartialView("_UpdateComment", model);
        }
        [HttpPost]
        public ActionResult EditComment(CommentViewModel model)
        {
            _logger.LogDebug($"Edit Comment(Id: {model.Id})");
            var entity = _commentService.GetComment(model.Id);      
            if (entity != null) _commentService.UpdateComment(model);
            return RedirectToAction("Profile", new { Id = model.LeadId });
        }
        public ActionResult DeleteComment(Guid id, Guid leadId)
        {
            _logger.LogDebug($"Delete Comment(Id: {id})");
            _commentService.DeleteComment(id);
            return RedirectToAction("Profile", new { Id = leadId });
        }
        #endregion

        #region Book
        [HttpPost]
        public ActionResult AddLoanBook(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Add Comment");
            model.LoanBook.UserId = GetUserIdLogin();
            if (model.LoanBook.LoanProductId == 0) model.LoanBook.LoanProductId = null;
            _loanBookService.CreateLoanBook(model.LoanBook);
            return RedirectToAction("Profile", new { Id = model.LoanBook.LeadId });
        }
        #endregion
        
        #region Address JSON
        [HttpGet]
        public JsonResult UpdateDistrict(int id)
        {
            var province = _addressService.GetAllProvince().FirstOrDefault(p => p.Id == id);
            var districts = _addressService.GetAllDistrict().Where(p => province != null && p.ProvinceCode == province.ProvinceCode);
            var items = districts.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
            return Json(new { data = items.ToList() });
        }
        #endregion

        #region Import

        [HttpGet]
        public ActionResult Import()
        {
            LeadImportViewModel model = new LeadImportViewModel();
            List<SelectListItem> values = (from LEAD_IMPORT d in Enum.GetValues(typeof(LEAD_IMPORT))
                                           select new SelectListItem
                                           {
                                               Text = d.ToName(),
                                               Value = ((int)d).ToString()
                                           }).ToList();

            ViewBag.LeadImports = values;
            model.LableColA = "B";
            model.LableColB = "B";
            model.LableColC = "C";
            model.LableColD = "D";
            model.LableColE = "E";
            model.LableColF = "F";
            model.ColA = LEAD_IMPORT.FIRSTNAME;
            model.ColB = LEAD_IMPORT.LASTNAME;
            model.ColC = LEAD_IMPORT.CMND;
            model.ColD = LEAD_IMPORT.PHONE;
            model.ColE = LEAD_IMPORT.PROVINCE;
            model.ColF = LEAD_IMPORT.COMPANY;

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

        [HttpPost("ImportLeadsFromExcel")]
        public ActionResult ImportLeadsFromExcel(LeadImportViewModel model)
        {
            Guid userId = GetUserIdLogin();
            model.Leads = new List<LeadImportItemViewModel>();
            model.Leads.Add(new LeadImportItemViewModel { LableColumn = model.LableColA, ValueColumn = model.ColA });
            model.Leads.Add(new LeadImportItemViewModel { LableColumn = model.LableColB, ValueColumn = model.ColB });
            model.Leads.Add(new LeadImportItemViewModel { LableColumn = model.LableColC, ValueColumn = model.ColC });
            model.Leads.Add(new LeadImportItemViewModel { LableColumn = model.LableColD, ValueColumn = model.ColD });
            model.Leads.Add(new LeadImportItemViewModel { LableColumn = model.LableColE, ValueColumn = model.ColE });
            model.Leads.Add(new LeadImportItemViewModel { LableColumn = model.LableColF, ValueColumn = model.ColF });
            int totalRowSuccess = 0;
            int totalRowFail = 0;
            bool result = false;
            string url = _leadService.ImportLeadExcel(model, userId, out totalRowSuccess, out totalRowFail, out result);
            if (result)
                TempData["SuccessOrFail"] = Resource.Lead_import_successful;
            else
                TempData["SuccessOrFail"] = Resource.Lead_import_fail;
            TempData["TotalRowSuccess"] = totalRowSuccess;
            TempData["TotalRowFail"] = totalRowFail;

            TempData["ShowButtonDownload"] = url;
            return RedirectToAction("Import", "Lead");
        }

        [HttpPost()]
        public async Task<IActionResult> ImportLeadsFromExcelJs(PostDataAndFile DTPost)
        {
            Guid userId = GetUserIdLogin();
            LeadsViewModel leads = new LeadsViewModel();
            leads = JsonConvert.DeserializeAnonymousType(DTPost.DataInfo, leads);
            var result = await _leadService.ImportLeadExcel(leads, DTPost.ImageFile, userId);
            return Ok(result);
        }

        #endregion
    }
}