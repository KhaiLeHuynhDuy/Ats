using Ats.Commons.Utilities;
using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Address;
using Ats.Domain.Address.Models;
using Ats.Domain.Lead.Models;
using Ats.Models.Lead;
using Ats.Services.Extensions;
using log4net.Util;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ats.Commons;
using static Ats.Commons.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Ats.Models.ResponeResult;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class LeadService : BaseService, ILeadService
    {
        private IConfiguration _config;
        private int pageSize;
        private readonly IHostingEnvironment _hostingEnvironment;
        public LeadService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger, IHostingEnvironment hostingEnvironment) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
            _hostingEnvironment = hostingEnvironment;
        }

        public List<LeadViewModel> Search(string keyword, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, Gender? gender, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Leads service Search={keyword}, Page={page}");

            if (!String.IsNullOrEmpty(keyword)) keyword = keyword.Trim();

            var query = UnitOfWork.LeadRepo.GetAll().Where(x => (String.IsNullOrEmpty(keyword) || (!String.IsNullOrEmpty(keyword)
                                        && ((!String.IsNullOrEmpty(x.Profile.FirstName) && x.Profile.FirstName.ToLower().Contains(keyword.ToLower()))
                                        || (!String.IsNullOrEmpty(x.Profile.LastName) && x.Profile.LastName.ToLower().Contains(keyword.ToLower()))
                                        || (!String.IsNullOrEmpty(x.Profile.Email) && x.Profile.Email.ToLower().Contains(keyword.ToLower()))
                                        || (!String.IsNullOrEmpty(x.Profile.Phone) && x.Profile.Phone.ToLower().Contains(keyword.ToLower())))))
                                        && (province == 0 || province == null ? true : ((x.Profile == null && x.Profile.Address == null) ? false : x.Profile.Address.ProvinceId == province))
                                        && (occupation == 0 || occupation == null ? true : x.OccupationId == occupation)
                                        && (loanProduct == 0 || loanProduct == null ? true : x.LoanProductId == loanProduct)
                                        && (loanPeriod == 0 || loanPeriod == null ? true : (int)x.LoanPeriod == loanPeriod)
                                    );
            switch (loanAmount)
            {
                case 1: // under 5 million
                    query = query.Where(x => x.Amount <= 5000000);
                    break;
                case 2: // 5 million to 10 million
                    query = query.Where(x => x.Amount >= 5000000 && x.Amount <= 10000000);
                    break;
                case 3: // 10 million to 30 million
                    query = query.Where(x => x.Amount >= 10000000 && x.Amount <= 30000000);
                    break;
                case 4: // 30 million to 50 million
                    query = query.Where(x => x.Amount >= 30000000 && x.Amount <= 50000000);
                    break;
                case 5: // over 50 million
                    query = query.Where(x => x.Amount >= 50000000);
                    break;
                default:
                    break;
            }
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "firstname":
                        query = IsAscOrder ? query.OrderBy(x => x.Profile.FirstName) : query.OrderByDescending(x => x.Profile.FirstName);
                        break;
                    case "lastname":
                        query = IsAscOrder ? query.OrderBy(x => x.Profile.PersonalId) : query.OrderByDescending(x => x.Profile.LastName);
                        break;
                    case "cmnd":
                        query = IsAscOrder ? query.OrderBy(x => x.Profile.PersonalId) : query.OrderByDescending(x => x.Profile.PersonalId);
                        break;
                    case "email":
                        query = IsAscOrder ? query.OrderBy(x => x.Profile.Email) : query.OrderByDescending(x => x.Profile.Email);
                        break;
                    case "gender":
                        query = IsAscOrder ? query.OrderBy(x => x.Profile.Gender) : query.OrderByDescending(x => x.Profile.Gender);
                        break;
                    case "dob":
                        query = IsAscOrder ? query.OrderBy(x => x.Profile.DOB) : query.OrderByDescending(x => x.Profile.DOB);
                        break;
                    case "amonut":
                        query = IsAscOrder ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                        break;
                    case "product":
                        query = IsAscOrder ? query.OrderBy(x => x.LoanProduct) : query.OrderByDescending(x => x.LoanProduct);
                        break;
                    case "period":
                        query = IsAscOrder ? query.OrderBy(x => x.LoanPeriod) : query.OrderByDescending(x => x.LoanPeriod);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<LeadViewModel> data = new List<LeadViewModel>();
            foreach (var item in datas)
            {
                LeadViewModel itemLead = new LeadViewModel();
                itemLead.Id = item.Id;
                itemLead.FirstName = item.Profile.FirstName;
                itemLead.LastName = item.Profile.LastName;
                itemLead.Email = item.Profile.Email;
                itemLead.Phone = item.Profile.Phone;
                itemLead.DOB = item.Profile.DOB;
                itemLead.YOB = item.Profile.YOB;
                itemLead.Gender = item.Profile.Gender;
                itemLead.PersonalId = item.Profile.PersonalId;
                itemLead.Amount = item.Amount;
                itemLead.LoanProductId = item.LoanProductId;
                itemLead.LeadDate = item.LeadDate;
                itemLead.IsActive = item.IsActive;
                itemLead.LoadProductName = item.LoanProduct != null ? item.LoanProduct.Name : "";
                if (item.LoanPeriod != null && (int)item.LoanPeriod != 0)
                    itemLead.LoanPeroidName = item.LoanPeriod.ToName();
                itemLead.JobTitleName = item.JobTitle != null ? item.JobTitle.Name : "";
                itemLead.ProfileAddress = (item.Profile != null && item.Profile.Address != null) ?
                    ((!string.IsNullOrEmpty(item.Profile.Address.Address1) ? item.Profile.Address.Address1 + ", " : "")
                    + (item.Profile.Address.District != null ? item.Profile.Address.District.Name + ", " : "")
                    + (item.Profile.Address.Province != null ? item.Profile.Address.Province.Name : "")) : "";
                data.Add(itemLead);
            }
            return data;
        }

        public LeadViewModel GetLead(Guid id)
        {
            _logger.LogDebug($"GetLead (Id: {id})");
            var entity = UnitOfWork.LeadRepo.GetById(id);


            if (entity == null)
            {
                _logger.LogDebug($"GetLead (Id: {id}) Not Found.");
                throw new NullReferenceException($"Lead {id} Not Found.");
            }

            var model = new LeadViewModel()
            {
                Id = entity.Id,
                //FirstName = entity.Profile.FirstName,
                //LastName = entity.Profile.LastName,
                //Email = entity.Profile.Email,
                //Phone = entity.Profile.Phone,
                //DOB = entity.Profile.DOB.Value,
                //PersonalId = entity.Profile.PersonalId,
                //JobTitleId = entity.JobTitleId,
                //OccupationId = entity.OccupationId,
                //IsActive = entity.IsActive,
            };

            return model;
        }

        public LeadProfileViewModel GetLeadProfile(Guid id)
        {
            _logger.LogDebug($"GetLeadProfile (Id: {id})");
            var entity = UnitOfWork.LeadRepo.GetById(id);
            try
            {
                if (entity == null)
                {
                    _logger.LogDebug($"GetLead (Id: {id}) Not Found.");
                    throw new NullReferenceException($"Lead {id} Not Found.");
                }
                LeadProfileViewModel model = new LeadProfileViewModel();
                model.Id = entity.Id;
                model.LeadId = entity.LeadId;
                model.FirstName = entity.Profile.FirstName;
                model.LastName = entity.Profile.LastName;
                model.Email = entity.Profile.Email;
                model.Phone = entity.Profile.Phone;
                model.DOB = entity.Profile.DOB;
                model.Gender = entity.Profile.Gender;
                if (entity.Profile.DOB != null)
                {
                    model.Month = entity.Profile.DOB.Value.Month;
                    model.Day = entity.Profile.DOB.Value.Day;
                }
                model.YOB = entity.Profile.YOB;
                model.PersonalId = entity.Profile.PersonalId;
                model.PersonalIdRegisterPlace = entity.Profile.PersonalIdRegisterPlace;
                model.PersonalIdRegisteredDate = entity.Profile.PersonalIdRegisteredDate;
                if (entity.Profile.Address != null)
                {
                    model.DistrictId = entity.Profile.Address.DistrictId;
                    model.ProvinceId = entity.Profile.Address.ProvinceId;
                    model.Address1 = entity.Profile.Address.Address1;
                    model.FullAddress = entity.Profile.Address.Address1 + ", " + (entity.Profile.Address.District != null ? entity.Profile.Address.District.Name : "") + ", " + (entity.Profile.Address.Province != null ? entity.Profile.Address.Province.Name : "");
                    model.HalfAddress = (entity.Profile.Address.District != null ? entity.Profile.Address.District.Name : "") + ", " + (entity.Profile.Address.Province != null ? entity.Profile.Address.Province.Name : "");
                }
                if (entity.Profile.AdditionalAddress != null)
                {
                    model.AdditionalDistrictId = entity.Profile.AdditionalAddress.DistrictId;
                    model.AdditionalProvinceId = entity.Profile.AdditionalAddress.ProvinceId;
                    model.AdditionalAddress = entity.Profile.AdditionalAddress.Address1;
                    model.FullAdditionalAddress = entity.Profile.AdditionalAddress.Address1 + ", " + (entity.Profile.AdditionalAddress.District != null ? entity.Profile.AdditionalAddress.District.Name : "") + ", " + (entity.Profile.AdditionalAddress.Province != null ? entity.Profile.AdditionalAddress.Province.Name : "");
                }
                model.JobTitleId = entity.JobTitleId;
                model.OccupationId = entity.OccupationId;
                model.CompanyName = entity.CompanyName;
                model.LoanProductId = entity.LoanProductId;
                model.Amount = entity.Amount;
                model.LeadDate = entity.LeadDate;
                model.IncomeAmountId = entity.IncomeAmountId;
                model.IncomeStreamId = entity.IncomeStreamId;
                model.OriginalFileId = entity.OriginalFileId;
                model.OriginalFileAdditionId = entity.OriginalFileAdditionId;
                model.MonthyIncome = entity.MonthlyIncome;
                model.LoanPurpose = entity.LoanPurpose;
                model.MaritalStatus = entity.MaritalStatus;
                model.IncomeReceivedMethod = entity.IncomeReceivedMethod;
                model.HealthInsurance = entity.HealthInsurance;
                model.LifeInsurance = entity.LifeInsurance;
                model.ElectricityBill = entity.ElectricityBill;
                model.InternetBill = entity.InternetBill;
                model.VehicleRegistrationCertificate = entity.VehicleRegistrationCertificate;
                model.BankAccount = entity.BankAccount;
                model.SimCard = entity.SimCard;
                model.LeadSource = entity.LeadSource;
                model.LoanPeriod = entity.LoanPeriod;
                model.Position = entity.Position;
                model.ResigterNumber = entity.RegisterNumber;
                model.Remark = entity.Remark;
                model.IsActive = entity.IsActive;
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void CreateLead(LeadViewModel model)
        {
            _logger.LogDebug($"CreateLead (Name: {model.FirstName} {model.LastName})");

            Address address = this.UnitOfWork.AddressRepo.CreateAddress(model.Address1, model.ProvinceId, model.DistrictId);
            int leadIdMax = this.UnitOfWork.LeadRepo.GetMaxLeadId();
            if (model.Month > 0 && model.Day > 0 && model.YOB > 0) model.DOB = new DateTime(model.YOB, model.Month, model.Day);

            var profile = new Profile();
            profile.Id = Guid.NewGuid();
            profile.FirstName = model.FirstName;
            profile.LastName = model.LastName;
            profile.Email = model.Email;
            profile.Phone = model.Phone;
            profile.PersonalId = model.PersonalId;
            profile.Gender = model.Gender;
            profile.YOB = model.YOB;
            profile.DOB = model.DOB;
            profile.AddressId = address.Id;
            profile.Address = address;
            this.UnitOfWork.ProfileRepo.Insert(profile);

            var lead = new Lead();
            lead.Id = new Guid();
            lead.LeadId = leadIdMax + 1;
            lead.ProfileId = profile.Id;
            lead.Profile = profile;
            lead.OccupationId = model.OccupationId;
            lead.CompanyName = model.CompanyName;
            lead.JobTitleId = model.JobTitleId;
            lead.LoanProductId = model.LoanProductId;
            lead.Amount = model.Amount * 1000000;
            lead.LeadDate = DateTime.UtcNow;
            lead.LoanPeriod = model.LoanPeroid;
            lead.LeadDate = DateTime.UtcNow;


            this.UnitOfWork.LeadRepo.Insert(lead);
            this.UnitOfWork.SaveChanges();
        }
        //Update profile 25/02/2021

        public void UpdateLeadProfile(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Update Lead Profile service (Id: {model.Id})");
            Address additionalAddress = UnitOfWork.AddressRepo.CreateAddress(model.AdditionalAddress, model.AdditionalProvinceId, model.AdditionalDistrictId);
            if (model.Month > 0 && model.Day > 0 && model.YOB > 0) model.DOB = new DateTime(model.YOB, model.Month.Value, model.Day.Value);
            var entity = UnitOfWork.LeadRepo.GetById(model.Id);
            if (entity != null)
            {
                //General Info
                entity.Profile.FirstName = model.FirstName;
                entity.Profile.LastName = model.LastName;
                entity.Profile.Email = model.Email;
                entity.Profile.Phone = model.Phone;
                entity.Profile.DOB = model.DOB;
                entity.Profile.YOB = model.YOB;
                entity.Profile.Gender = model.Gender;
                entity.MaritalStatus = model.MaritalStatus;
                entity.Profile.PersonalId = model.PersonalId;
                entity.Profile.PersonalIdRegisterPlace = model.PersonalIdRegisterPlace;
                entity.Profile.PersonalIdRegisteredDate = model.PersonalIdRegisteredDate;

                //Address
                if (entity.Profile.Address == null)
                {
                    Address address = UnitOfWork.AddressRepo.CreateAddress(model.Address1, model.ProvinceId, model.DistrictId);
                    entity.Profile.AddressId = address.Id;
                    entity.Profile.Address = address;
                    UnitOfWork.LeadRepo.Update(entity);
                }
                else
                {
                    entity.Profile.Address.Address1 = model.Address1;
                    entity.Profile.Address.DistrictId = model.DistrictId;
                    entity.Profile.Address.ProvinceId = model.ProvinceId;
                }

                //Additional Address
                entity.Profile.AdditionalAddress = additionalAddress;
                entity.Profile.AdditionalAddressId = additionalAddress.Id;
                UnitOfWork.LeadRepo.Update(entity);

                entity.Profile.AdditionalAddress.Address1 = model.AdditionalAddress;
                entity.Profile.AdditionalAddress.DistrictId = model.AdditionalDistrictId;
                entity.Profile.AdditionalAddress.ProvinceId = model.AdditionalProvinceId;
                UnitOfWork.LeadRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateLeadCompanyInfo(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Update Lead Company service (Id: {model.Id})");
            var entity = UnitOfWork.LeadRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.OccupationId = model.OccupationId;
                entity.JobTitleId = model.JobTitleId;
                entity.CompanyName = model.CompanyName;
                entity.Position = model.Position;
                entity.RegisterNumber = model.ResigterNumber;
                entity.MonthlyIncome = model.MonthyIncome;
                entity.IncomeReceivedMethod = model.IncomeReceivedMethod;

                UnitOfWork.LeadRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateLeadOtherInfo(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Update Lead Other Info service (Id: {model.Id})");
            var entity = UnitOfWork.LeadRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.VehicleRegistrationCertificate = model.VehicleRegistrationCertificate;
                entity.LifeInsurance = model.LifeInsurance;
                entity.HealthInsurance = model.HealthInsurance;
                entity.ElectricityBill = model.ElectricityBill;
                entity.InternetBill = model.InternetBill;
                entity.SimCard = model.SimCard;
                entity.BankAccount = model.BankAccount;
                entity.OriginalFileId = model.OriginalFileId;
                entity.OriginalFileAdditionId = model.OriginalFileAdditionId;

                UnitOfWork.LeadRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateLeadLoanInfo(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Update Lead Loan service (Id: {model.Id})");
            var entity = UnitOfWork.LeadRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.LoanPurpose = model.LoanPurpose;
                entity.LoanProductId = model.LoanProductId;
                entity.Amount = model.Amount;
                entity.LoanPeriod = model.LoanPeriod;

                UnitOfWork.LeadRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateLeadRemarkInfo(LeadProfileViewModel model)
        {
            _logger.LogDebug($"Update Lead Remark service (Id: {model.Id})");
            var entity = UnitOfWork.LeadRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.LeadSource = model.LeadSource;
                entity.Remark = model.Remark;
                entity.LeadDate = model.LeadDate;
                entity.IsActive = model.IsActive;

                UnitOfWork.LeadRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLead(Guid id)
        {
            _logger.LogDebug($"Delete Lead service (Id: {id})");
            var entity = UnitOfWork.LeadRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.LeadRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public string ImportLeadExcel(LeadImportViewModel model, Guid userId, out int totalRowSuccess, out int totalRowFail, out bool result)
        {
            result = false;
            totalRowSuccess = 0;
            totalRowFail = 0;
            MemoryStream memory = new MemoryStream();
            string url = string.Empty;
            try
            {
                //Save file excel
                string urlExcel = SaveFile(model.FormFile, userId);
                List<LeadExcelViewModel> excelLeads = new List<LeadExcelViewModel>();
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, urlExcel);
                FileInfo newFile = new FileInfo(uploads);
                Stream stream = newFile.OpenRead();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    int lastColumns = worksheet.Dimension.Columns + 1;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var itemLeadSucces = new LeadExcelViewModel();
                            foreach (var leadItem in model.Leads)
                            {
                                switch (leadItem.ValueColumn)
                                {
                                    case LEAD_IMPORT.SELECT:
                                        break;
                                    case LEAD_IMPORT.FIRSTNAME:
                                        itemLeadSucces.FirstName = Ultility.GetFirstName(worksheet.Cells[leadItem.LableColumn + row].Value.ToString());
                                        break;
                                    case LEAD_IMPORT.LASTNAME:
                                        itemLeadSucces.LastName = Ultility.GetLastName(worksheet.Cells[leadItem.LableColumn + row].Value.ToString().Trim());
                                        break;
                                    case LEAD_IMPORT.CMND:
                                        itemLeadSucces.CMND = worksheet.Cells[leadItem.LableColumn + row].Value.ToString();
                                        break;
                                    case LEAD_IMPORT.PHONE:
                                        itemLeadSucces.Phone = worksheet.Cells[leadItem.LableColumn + row].Value.ToString();
                                        break;
                                    case LEAD_IMPORT.PROVINCE:
                                        itemLeadSucces.Province = worksheet.Cells[leadItem.LableColumn + row].Value.ToString();
                                        break;
                                    case LEAD_IMPORT.COMPANY:
                                        itemLeadSucces.Company = worksheet.Cells[leadItem.LableColumn + row].Value.ToString();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            bool exce = SaveLeadImportByExcel(itemLeadSucces);
                            if (exce)
                            {
                                totalRowSuccess++;
                                worksheet.Cells[row, lastColumns].Value = "Success";
                            }
                            else
                            {
                                totalRowFail++;
                                worksheet.Cells[row, lastColumns].Value = "Fail";
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error Import Excel Lead: at row={row}");
                            totalRowFail++;
                            worksheet.Cells[row, lastColumns].Value = "Fail";
                            continue;
                        }
                    }
                    package.Save();
                    var _stream = package.Stream;
                    _stream.Position = 0;
                    url = SaveFileExport(model.FormFile, userId, _stream);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Import Excel Lead -> " + ex.ToString());
                result = false;
            }
            return url;
        }

        public async Task<object> ImportLeadExcel(LeadsViewModel leads, IFormFile formFile, Guid userId)
        {
            bool result = false;
            int totalRowSuccess = 0;
            int totalRowFail = 0;
            string url = string.Empty;
            try
            {
                //Save file excel
                string urlExcel = SaveFile(formFile, userId);
                List<LeadExcelViewModel> excelLeads = new List<LeadExcelViewModel>();
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, urlExcel);
                FileInfo newFile = new FileInfo(uploads);
                Stream stream = newFile.OpenRead();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    int lastColumns = worksheet.Dimension.Columns + 1;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            var itemLeadSucces = new LeadExcelViewModel();
                            foreach (var leadItem in leads.LeadItems)
                            {
                                switch (leadItem.Field)
                                {
                                    case "NONE":
                                        break;
                                    case "FIRSTNAME":
                                        if (leadItem.IsDefault)
                                            itemLeadSucces.FirstName = leadItem.Value;
                                        else
                                            itemLeadSucces.FirstName = Ultility.GetFirstName(worksheet.Cells[leadItem.Col + row].Value.ToString());
                                        break;
                                    case "LASTNAME":
                                        if (leadItem.IsDefault)
                                            itemLeadSucces.LastName = leadItem.Value;
                                        else
                                            itemLeadSucces.LastName = Ultility.GetLastName(worksheet.Cells[leadItem.Col + row].Value.ToString().Trim());
                                        break;
                                    case "CMND":
                                        if (leadItem.IsDefault)
                                            itemLeadSucces.CMND = leadItem.Value;
                                        else
                                            itemLeadSucces.CMND = worksheet.Cells[leadItem.Col + row].Value.ToString();
                                        break;
                                    case "PHONE":
                                        if (leadItem.IsDefault)
                                            itemLeadSucces.Phone = leadItem.Value;
                                        else
                                            itemLeadSucces.Phone = worksheet.Cells[leadItem.Col + row].Value.ToString();
                                        break;
                                    case "PROVINCE":
                                        if (leadItem.IsDefault)
                                            itemLeadSucces.Province = leadItem.Value;
                                        else
                                            itemLeadSucces.Province = worksheet.Cells[leadItem.Col + row].Value.ToString();
                                        break;
                                    case "COMPANY":
                                        if (leadItem.IsDefault)
                                            itemLeadSucces.Company = leadItem.Value;
                                        else
                                            itemLeadSucces.Company = worksheet.Cells[leadItem.Col + row].Value.ToString();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            bool exce = SaveLeadImportByExcel(itemLeadSucces);
                            if (exce)
                            {
                                totalRowSuccess++;
                                worksheet.Cells[row, lastColumns].Value = "Success";
                            }
                            else
                            {
                                totalRowFail++;
                                worksheet.Cells[row, lastColumns].Value = "Fail";
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error Import Excel Lead: at row={row}");
                            totalRowFail++;
                            worksheet.Cells[row, lastColumns].Value = "Fail";
                            continue;
                        }
                    }
                    package.Save();
                    var _stream = package.Stream;
                    _stream.Position = 0;
                    url = SaveFileExport(formFile, userId, _stream);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Import Excel Lead -> " + ex.ToString());
                result = false;
            }
            return new ResponseData(true, 200, "Success", new { result, totalRowSuccess, totalRowFail, url });
        }

        public async Task<object> LinkDownloadImportLeadExcel(string urlExcel)
        {
            string url = string.Empty;
            try
            {
                url = Path.Combine(_hostingEnvironment.WebRootPath, urlExcel);
            }
            catch (Exception ex)
            {
               
            }
            return new ResponseData(true, 200, "Success", new { url });
        }

        public bool SaveLeadImportByExcel(LeadExcelViewModel model)
        {
            bool result = false;
            try
            {
                this.UnitOfWork.BeginTransaction();
                Province province = !string.IsNullOrEmpty(model.Province) ? this.UnitOfWork.ProvinceRepo.GetAll().FirstOrDefault(x => ("tp " + x.Name).ToLower().Trim().Contains(model.Province.ToLower().Trim())) : null;
                Profile profile = new Profile();
                profile.Id = Guid.NewGuid();
                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                if (province != null)
                {
                    Address address = new Address()
                    {
                        Id = Guid.NewGuid(),
                        ProvinceId = province.Id
                    };
                    this.UnitOfWork.AddressRepo.Insert(address);
                    profile.AddressId = address.Id;
                    profile.Address = address;
                }
                profile.Phone = model.Phone;
                profile.PersonalId = model.CMND;
                this.UnitOfWork.ProfileRepo.Insert(profile);

                int leadIdMax = this.UnitOfWork.LeadRepo.GetMaxLeadId();
                Lead lead = new Lead();
                lead.Id = new Guid();
                lead.LeadId = leadIdMax + 1;
                lead.ProfileId = profile.Id;
                lead.Profile = profile;
                lead.IsActive = false;
                lead.CompanyName = model.Company;
                lead.LeadDate = DateTime.UtcNow;
                this.UnitOfWork.LeadRepo.Insert(lead);
                this.UnitOfWork.SaveChanges();
                this.UnitOfWork.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                this.UnitOfWork.RollbackTransaction();
            }
            return result;
        }
        public string SaveFile(IFormFile file, Guid userId)
        {
            string uploadPath = "ImportExcel/" + userId.ToString();
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, uploadPath);
            DirectoryInfo allFiles = new DirectoryInfo(uploads);
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            //else
            //{
            //    Directory.Delete(uploads);
            //    foreach (var item in allFiles.GetFiles())
            //    {
            //        item.Delete();
            //    }
            //}
            var extension = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileName(DateTime.Now.ToString("ddMMyyyyhhmmss" + "V_" + allFiles.GetFiles().Count()) + extension);
            var filePath = Path.Combine(uploads, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string url = $"{uploadPath}/{fileName}";
            return url;
        }

        public string SaveFileExport(IFormFile file, Guid userId, Stream stream)
        {
            string uploadPath = "ExportExcel/" + userId.ToString();
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, uploadPath);
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            DirectoryInfo allFiles = new DirectoryInfo(uploads);
            var extension = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileName(DateTime.Now.ToString("ddMMyyyyhhmmss" + "V_" + allFiles.GetFiles().Count()) + extension);
            var filePath = Path.Combine(uploads, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }
            string url = $"/{uploadPath}/{fileName}";
            return url;
        }
    }
}
