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
using Ats.Models.Borrower;
using Ats.Domain.Loan.Models;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class BorrowerService : BaseService, IBorrowerService
    {
        private IConfiguration _config;
        private int pageSize;
        private readonly IHostingEnvironment _hostingEnvironment;
        public BorrowerService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger, IHostingEnvironment hostingEnvironment) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
            _hostingEnvironment = hostingEnvironment;
        }

        public List<BorrowerViewModel> Search(string keyword, int? province, int? occupation, int? loanProduct, int? loanPeriod, int? loanAmount, Gender? gender, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Borrowers service Search={keyword}, Page={page}");

            if (!String.IsNullOrEmpty(keyword)) keyword = keyword.Trim();

            var query = UnitOfWork.BorrowerRepo.GetAll().Where(x => (String.IsNullOrEmpty(keyword) || (!String.IsNullOrEmpty(keyword)
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
            List<BorrowerViewModel> data = new List<BorrowerViewModel>();
            foreach (var item in datas)
            {
                BorrowerViewModel itemBorrower = new BorrowerViewModel();
                itemBorrower.Id = item.Id;
                itemBorrower.FirstName = item.Profile.FirstName;
                itemBorrower.LastName = item.Profile.LastName;
                itemBorrower.Email = item.Profile.Email;
                itemBorrower.Phone = item.Profile.Phone;
                itemBorrower.DOB = item.Profile.DOB;
                itemBorrower.YOB = item.Profile.YOB;
                itemBorrower.Gender = item.Profile.Gender;
                itemBorrower.PersonalId = item.Profile.PersonalId;
                itemBorrower.Amount = item.Amount;
                itemBorrower.LoanProductId = item.LoanProductId;
                itemBorrower.BorrowerDate = item.BorrowerDate;
                itemBorrower.IsActive = item.IsActive;
                itemBorrower.LoadProductName = item.LoanProduct != null ? item.LoanProduct.Name : "";
                if (item.LoanPeriod != null && (int)item.LoanPeriod != 0)
                    itemBorrower.LoanPeroidName = item.LoanPeriod.ToName();
                itemBorrower.JobTitleName = item.JobTitle != null ? item.JobTitle.Name : "";
                itemBorrower.ProfileAddress = (item.Profile != null && item.Profile.Address != null) ?
                    ((!string.IsNullOrEmpty(item.Profile.Address.Address1) ? item.Profile.Address.Address1 + ", " : "")
                    + (item.Profile.Address.District != null ? item.Profile.Address.District.Name + ", " : "")
                    + (item.Profile.Address.Province != null ? item.Profile.Address.Province.Name : "")) : "";
                data.Add(itemBorrower);
            }
            return data;
        }

        public BorrowerViewModel GetBorrower(Guid id)
        {
            _logger.LogDebug($"Get Borrower (Id: {id})");
            var entity = UnitOfWork.BorrowerRepo.GetById(id);


            if (entity == null)
            {
                _logger.LogDebug($"Get Borrower (Id: {id}) Not Found.");
                throw new NullReferenceException($"Lead {id} Not Found.");
            }

            var model = new BorrowerViewModel()
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

        public BorrowerProfileViewModel GetBorrowerProfile(Guid id)
        {
            _logger.LogDebug($"Get Borrower Profile (Id: {id})");
            var entity = UnitOfWork.BorrowerRepo.GetById(id);
            try
            {
                if (entity == null)
                {
                    _logger.LogDebug($"Get Borrower (Id: {id}) Not Found.");
                    throw new NullReferenceException($"Borrower {id} Not Found.");
                }
                BorrowerProfileViewModel model = new BorrowerProfileViewModel();
                model.Id = entity.Id;
                model.LeadId = entity.LeadId;
                model.BorrowerId = entity.BorrowerId;
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
                model.BorrowerDate = entity.BorrowerDate;
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
                model.BorrowerSource = entity.BorrowerSource;
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

        public void CreateBorrower(BorrowerViewModel model)
        {
            _logger.LogDebug($"Create Borrower (Name: {model.FirstName} {model.LastName})");

            Address address = this.UnitOfWork.AddressRepo.CreateAddress(model.Address1, model.ProvinceId, model.DistrictId);
            int borrowerIdMax = this.UnitOfWork.BorrowerRepo.GetMaxBorrowerId();
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
            UnitOfWork.ProfileRepo.Insert(profile);

            var borrower = new Borrower();
            borrower.Id = new Guid();
            borrower.BorrowerId = borrowerIdMax + 1;
            borrower.ProfileId = profile.Id;
            borrower.Profile = profile;
            borrower.OccupationId = model.OccupationId;
            borrower.CompanyName = model.CompanyName;
            borrower.JobTitleId = model.JobTitleId;
            borrower.LoanProductId = model.LoanProductId;
            borrower.Amount = model.Amount * 1000000;
            borrower.BorrowerDate = DateTime.UtcNow;
            borrower.LoanPeriod = model.LoanPeroid;
            borrower.BorrowerDate = DateTime.UtcNow;


            UnitOfWork.BorrowerRepo.Insert(borrower);
            UnitOfWork.SaveChanges();
        }
        //Update profile 25/02/2021

        public void UpdateBorrowerProfile(BorrowerProfileViewModel model)
        {
            _logger.LogDebug($"Update Lead Profile service (Id: {model.Id})");
            Address additionalAddress = UnitOfWork.AddressRepo.CreateAddress(model.AdditionalAddress, model.AdditionalProvinceId, model.AdditionalDistrictId);
            if (model.Month > 0 && model.Day > 0 && model.YOB > 0) model.DOB = new DateTime(model.YOB, model.Month.Value, model.Day.Value);
            var entity = UnitOfWork.BorrowerRepo.GetById(model.Id);
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
                    UnitOfWork.BorrowerRepo.Update(entity);
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
                UnitOfWork.BorrowerRepo.Update(entity);

                entity.Profile.AdditionalAddress.Address1 = model.AdditionalAddress;
                entity.Profile.AdditionalAddress.DistrictId = model.AdditionalDistrictId;
                entity.Profile.AdditionalAddress.ProvinceId = model.AdditionalProvinceId;
                UnitOfWork.BorrowerRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateBorrowerCompanyInfo(BorrowerProfileViewModel model)
        {
            _logger.LogDebug($"Update Borrower Company service (Id: {model.Id})");
            var entity = UnitOfWork.BorrowerRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.OccupationId = model.OccupationId;
                entity.JobTitleId = model.JobTitleId;
                entity.CompanyName = model.CompanyName;
                entity.Position = model.Position;
                entity.RegisterNumber = model.ResigterNumber;
                entity.MonthlyIncome = model.MonthyIncome;
                entity.IncomeReceivedMethod = model.IncomeReceivedMethod;

                UnitOfWork.BorrowerRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateBorrowerOtherInfo(BorrowerProfileViewModel model)
        {
            _logger.LogDebug($"Update Borrower Other Info service (Id: {model.Id})");
            var entity = UnitOfWork.BorrowerRepo.GetById(model.Id);
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

                UnitOfWork.BorrowerRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateBorrowerLoanInfo(BorrowerProfileViewModel model)
        {
            _logger.LogDebug($"Update Lead Loan service (Id: {model.Id})");
            var entity = UnitOfWork.BorrowerRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.LoanPurpose = model.LoanPurpose;
                entity.LoanProductId = model.LoanProductId;
                entity.Amount = model.Amount;
                entity.LoanPeriod = model.LoanPeriod;

                UnitOfWork.BorrowerRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void UpdateBorrowerRemarkInfo(BorrowerProfileViewModel model)
        {
            _logger.LogDebug($"Update Borrower Remark service (Id: {model.Id})");
            var entity = UnitOfWork.BorrowerRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.BorrowerSource = model.BorrowerSource;
                entity.Remark = model.Remark;
                entity.BorrowerDate = model.BorrowerDate;
                entity.IsActive = model.IsActive;

                UnitOfWork.BorrowerRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteBorrower(Guid id)
        {
            _logger.LogDebug($"Delete Borrower service (Id: {id})");
            var entity = UnitOfWork.BorrowerRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.BorrowerRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
