using System.Collections.Generic;
using Ats.Domain;
using Ats.Commons;
using System;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ats.Domain.Identity.Models;
using System.Threading.Tasks;
using Ats.Models.Member;
using Ats.Domain.Address.Models;
using Ats.Domain.Member.Models;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Ats.Models.ResponeResult;
using Ats.Domain.Loyalty.Models;
using System.Diagnostics;
using Ats.Models.Account;
using Ats.Models.MemberWallet;
using System.Collections;
using System.Collections.ObjectModel;
using Ats.Commons.Utilities;
using Ats.Models.LoyaltyTier;
using System.Globalization;
using Ats.Commons.Resource;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Internal;
using System.Data;
using ClosedXML.Excel;

namespace Ats.Services.Implementation
{
    public class MembershipService : BaseService, IMembershipService
    {
        private readonly IConfiguration _config;
        private int pageSize;
        private readonly IHostingEnvironment _hostingEnvironment;
        public MembershipService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger, UserManager<User> userManager, SignInManager<User> signInManager,
                             IHostingEnvironment environment) : base(unitOfWork, logger, config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //pageSize = _config.GetValue<int>("PageSize");
            _hostingEnvironment = environment;
            this._config = config;
        }
      
        public List<MemberViewModel> Search(MemberSearchView memberSearch, string searchText, int? province, int? jobtitle, int? occupation, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Member service Search={searchText}, Page={page}");

            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();

            var query = UnitOfWork.MemberRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                                        && ((!String.IsNullOrEmpty(x.MemberCode) && x.MemberCode.ToLower().Contains(searchText.ToLower()))
                                        || (!String.IsNullOrEmpty(x.FirstName) && x.FirstName.ToLower().Contains(searchText.ToLower()))
                                        || (!String.IsNullOrEmpty(x.LastName) && x.LastName.ToLower().Contains(searchText.ToLower()))
                                        || (!String.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(searchText.ToLower()))
                                        || (!String.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.ToLower().Contains(searchText.ToLower())))))
                                        && (String.IsNullOrEmpty(memberSearch.MemberCode) ? true : x.MemberCode.ToLower().Contains(memberSearch.MemberCode.ToLower().Trim()))
                                        && (String.IsNullOrEmpty(memberSearch.FirstName) ? true : x.FirstName.ToLower().Contains(memberSearch.FirstName.ToLower().Trim()))
                                        && (String.IsNullOrEmpty(memberSearch.LastName) ? true : x.LastName.ToLower().Contains(memberSearch.LastName.ToLower().Trim()))
                                        && (String.IsNullOrEmpty(memberSearch.Email) ? true : x.Email.ToLower().Contains(memberSearch.Email.ToLower().Trim()))
                                        && (String.IsNullOrEmpty(memberSearch.PhoneNumber) ? true : x.PhoneNumber.ToLower().Contains(memberSearch.PhoneNumber.ToLower().Trim()))
                                        && (memberSearch.MaritalStatus == 0 || memberSearch.MaritalStatus == null ? true : x.MaritalStatus == memberSearch.MaritalStatus)
                                        && (gender == null ? true : x.Gender == gender)
                                        && (memberSearch.WeddingDate == null ? true : x.WeddingDate == memberSearch.WeddingDate)
                                        && (memberSearch.DOB == null ? true : x.DOB == memberSearch.DOB)
                                        && (memberSearch.RegisterDate == null ? true : x.RegisterDate == memberSearch.RegisterDate)
                                        && (province == 0 || province == null ? true : ((x == null && x.Address == null) ? false : x.Address.ProvinceId == province))
                                        && (district == 0 || district == null ? true : ((x == null && x.Address == null) ? false : x.Address.DistrictId == district))
                                        && (occupation == 0 || occupation == null ? true : x.OccupationId == occupation)
                                        && (storeId == 0 || storeId == null ? true : x.StoreId == storeId)
                                        && (channelId == 0 || channelId == null ? true : x.ChannelId == channelId)
                                        && (jobtitle == 0 || jobtitle == null ? true : x.JobTitleId == jobtitle)
                                        && (valid == null ? true : x.MemberWallets.FirstOrDefault().Active == valid)
                                    ).OrderByDescending(x => x.RegisterDate);
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "membercode":
                        query = IsAscOrder ? query.OrderBy(x => x.MemberCode) : query.OrderByDescending(x => x.MemberCode);
                        break;
                    case "firstname":
                        query = IsAscOrder ? query.OrderBy(x => x.FirstName) : query.OrderByDescending(x => x.FirstName);
                        break;
                    case "lastname":
                        query = IsAscOrder ? query.OrderBy(x => x.LastName) : query.OrderByDescending(x => x.LastName);
                        break;
                    case "email":
                        query = IsAscOrder ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email);
                        break;
                    case "phone":
                        query = IsAscOrder ? query.OrderBy(x => x.PhoneNumber) : query.OrderByDescending(x => x.PhoneNumber);
                        break;
                    case "gender":
                        query = IsAscOrder ? query.OrderBy(x => x.Gender) : query.OrderByDescending(x => x.Gender);
                        break;
                    case "yob":
                        query = IsAscOrder ? query.OrderBy(x => x.YOB) : query.OrderByDescending(x => x.YOB);
                        break;
                    case "registerdate":
                        query = IsAscOrder ? query.OrderBy(x => x.RegisterDate) : query.OrderByDescending(x => x.RegisterDate);
                        break;
                    case "province":
                        query = IsAscOrder ? query.OrderBy(x => x.Address.Province.Name) : query.OrderByDescending(x => x.Address.Province.Name);
                        break;
                    case "district":
                        query = IsAscOrder ? query.OrderBy(x => x.Address.DistrictId) : query.OrderByDescending(x => x.Address.DistrictId);
                        break;
                    case "channel":
                        query = IsAscOrder ? query.OrderBy(x => x.ChannelId) : query.OrderByDescending(x => x.ChannelId);
                        break;
                    case "point":
                        query = IsAscOrder ? query.OrderBy(x => x.MemberWallets.FirstOrDefault().Point) : query.OrderByDescending(x => x.MemberWallets.FirstOrDefault().Point);
                        break;
                    case "tier":
                        query = IsAscOrder ? query.OrderBy(x => x.MemberLoyaltyTiers.FirstOrDefault().LoyaltyTier.TierName) : query.OrderByDescending(x => x.MemberLoyaltyTiers.FirstOrDefault().LoyaltyTier.TierName);
                        break;
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberViewModel> data = new List<MemberViewModel>();


            foreach (var item in datas)
            {
                MemberViewModel itemMember = new MemberViewModel();
                {
                    itemMember.Id = item.Id;
                    itemMember.IsSelected = false;
                    itemMember.MemberCode = item.MemberCode;
                    itemMember.FirstName = item.FirstName;
                    itemMember.LastName = item.LastName;
                    itemMember.Email = item.Email;
                    itemMember.PhoneNumber = item.PhoneNumber;
                    itemMember.Gender = item.Gender;
                    itemMember.Point = item.MemberWallets.First().Point;
                    itemMember.TierName = item.MemberLoyaltyTiers.OrderByDescending(p => p.ActiveFrom <= DateTime.UtcNow && DateTime.UtcNow <= p.ActiveEnd ? p.ActiveEnd : null).FirstOrDefault().LoyaltyTier.TierName;
                    itemMember.RegisterDate = item.AddedTimestamp;//
                    itemMember.DateCreate = string.Format("{0:dd/MM/yyyy}", item.AddedTimestamp);
                    itemMember.DistrictName = item.Address.District != null ? item.Address.District.Name : "";
                    itemMember.ProvinceName = item.Address.Province != null ? item.Address.Province.Name : "";
                    itemMember.StringYOB = item.YOB != 0 ? item.YOB.ToString() : "";

                    itemMember.Active = item.MemberWallets.FirstOrDefault().Active;
                }
                data.Add(itemMember);
            }
            return data;
        }
        public MemberViewModel GetMember(Guid id)
        {
            _logger.LogDebug($"Get Member (Id: {id})");
            var entity = UnitOfWork.MemberRepo.GetById(id);


            if (entity == null)
            {
                _logger.LogDebug($"Get Member (Id: {id}) Not Found.");
                throw new NullReferenceException($"Member {id} Not Found.");
            }

            var model = new MemberViewModel()
            {
                Id = entity.Id,
                MemberCode = entity.MemberCode,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                PhoneNumber = entity.PhoneNumber

            };

            return model;
        }
        public MemberDetailViewModel GetMemberDetail(Guid id)
        {
            var entity = UnitOfWork.MemberRepo.GetById(id);
            try
            {

                MemberDetailViewModel model = new MemberDetailViewModel();
                model.Id = entity.Id;
                model.MemberCode = entity.MemberCode;
                model.ImageUrl = entity.PhotoUrl;
                model.FirstName = entity.FirstName;
                model.LastName = entity.LastName;
                model.Email = entity.Email;
                model.PhoneNumber = entity.PhoneNumber;
                model.Gender = entity.Gender;
                model.ZaloId = entity.ZaloId;
                //model.ReferralMember = entity.ReferralMember;
                model.AcceptToReceiveMarketing = entity.AcceptToReceiveMarketing;
                model.AcceptToReceiveLoyaltyInformation = entity.AcceptToReceiveLoyaltyInformation;
                model.AcceptToBeContactedViaMobilePhone = entity.AcceptToBeContactedViaMobilePhone;
                model.AcceptToBeContactedViaMobileEmail = entity.AcceptToBeContactedViaMobileEmail;
                model.NotAcceptAnyContact = entity.NotAcceptAnyContact;
                model.JobTitleId = entity.JobTitleId;
                model.JobTitle = entity.JobTitle != null ? entity.JobTitle.Name : null;
                model.MaritalStatus = entity.MaritalStatus;
                model.OccupationId = entity.OccupationId;
                model.Occupation = entity.Occupation != null ? entity.Occupation.Name : null;
                model.StoreName = entity.Store != null ? entity.Store.StoreName : null;
                model.ChannelName = entity.Channel != null ? entity.Channel.ChannelName : null;
                model.RegisterDate = entity.RegisterDate;
                model.DOBs = string.Format("{0:dd/MM/yyyy}", entity.DOB);
                model.DateCreate = string.Format("{0:dd/MM/yyyy}", entity.RegisterDate);
                model.DOB = entity.DOB;
                model.StoreId = entity.StoreId;
                model.ChannelId = entity.ChannelId;
                model.RegisterEmployee = entity.AddedUserId != null ? UnitOfWork.UserRepo.GetAll().FirstOrDefault(z => z.Id.ToString() == entity.AddedUserId).UserName : "";
                if (entity.DOB != null)
                {
                    model.Month = entity.DOB.Value.Month;
                    model.Day = entity.DOB.Value.Day;
                }
                model.YOB = entity.YOB;
                model.WeddingDate = entity.WeddingDate;
                model.FullName = $"{entity.LastName} {model.FirstName}";
                model.Point = UnitOfWork.MemberWalletRepo.GetAll().FirstOrDefault(p => p.MemberId == entity.Id).Point;
                model.PointName = entity.MemberLoyaltyTiers.OrderByDescending(p => p.ActiveFrom <= DateTime.Now && DateTime.Now <= p.ActiveEnd ? p.ActiveEnd : null).FirstOrDefault().LoyaltyTier.TierName;

                model.valid = entity.MemberWallets.FirstOrDefault().Active;
                if (entity.Address != null)
                {
                    _logger.LogDebug($"Get Member Profile Address (DistrictId: { model.DistrictId})" +
                         $"(__ProvinceId:{ model.ProvinceId})" +
                         $"(__Address1:{ model.address1})");
                    model.DistrictId = entity.Address.DistrictId;
                    model.ProvinceId = entity.Address.ProvinceId;
                    model.address1 = entity.Address.Address1;
                    model.Province = entity.Address.Province != null ? entity.Address.Province.Name : null;
                    model.District = entity.Address.District != null ? entity.Address.District.Name : null;

                    List<string> strArray = new List<string> { model.address1, model.District, model.Province };
                    string fullAddress = string.Join(", ", strArray.Where(m => !string.IsNullOrEmpty(m)).ToList());

                    model.FullAddress = fullAddress;
                }

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Member exception : {ex})");
                throw ex;
            }
        }

        public List<MemberDetailViewModel> GetMembers()
        {
            _logger.LogDebug($"GetAlls");
            var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
            {
                Id = x.Id,
                MemberCode = x.MemberCode,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.LastName + " " + x.FirstName,
                Profile = x.MemberCode + " --- " + x.LastName + " " + x.FirstName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Gender = x.Gender,
                AcceptToReceiveMarketing = x.AcceptToReceiveMarketing,
                AcceptToReceiveLoyaltyInformation = x.AcceptToReceiveLoyaltyInformation,
                AcceptToBeContactedViaMobilePhone = x.AcceptToBeContactedViaMobilePhone,
                AcceptToBeContactedViaMobileEmail = x.AcceptToBeContactedViaMobileEmail,
                NotAcceptAnyContact = x.NotAcceptAnyContact,
                JobTitleId = x.JobTitleId,
                MaritalStatus = x.MaritalStatus,
                OccupationId = x.OccupationId,
                StoreName = x.Store != null ? x.Store.StoreName : null,
                ChannelName = x.Channel != null ? x.Channel.ChannelName : null,
                RegisterDate = x.RegisterDate,
                DOB = x.DOB,
                WeddingDate = x.WeddingDate
            }).OrderBy(x => x.LastName).ToList();

            return members;
        }
        public int GetNewMembersMonths(DateTime monthStar, DateTime monthEnd)
        {

            _logger.LogDebug($"GetAlls");
            var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
            {
                Id = x.Id,
                MemberCode = x.MemberCode,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.LastName + " " + x.FirstName,
                Profile = x.MemberCode + " --- " + x.FirstName + " " + x.FirstName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreateDate = x.RegisterDate.Value,
            }).Where(m => m.CreateDate >= monthStar && m.CreateDate <= monthEnd).Count();
            return members;
        }
        public int GetAllMembersOfYear(string years)
        {
            years = DateTime.Now.Year.ToString();
            _logger.LogDebug($"GetAlls");
            var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
            {
                
                Id = x.Id,
                MemberCode = x.MemberCode,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.LastName + " " + x.FirstName,
                Profile = x.MemberCode + " --- " + x.FirstName + " " + x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreateDate = x.RegisterDate.Value,
            }).Where(m => m.CreateDate.Year == int.Parse(years)).Count();
            return members;
        }
        public int GetAllMembersInActiveOfYear(string years)
        {
            years = DateTime.Now.Year.ToString();
            _logger.LogDebug($"GetAlls");
            var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
            {

                Id = x.Id,
                MemberCode = x.MemberCode,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.LastName + " " + x.FirstName,
                Profile = x.MemberCode + " --- " + x.LastName + " " + x.FirstName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreateDate = x.RegisterDate.Value,
            }).Where(m => m.CreateDate.Year == int.Parse(years)).Count();
            return members;
        }
      
        public int GetNewMembersOfMonth(int month, string years)
        {
            //_logger.LogDebug($"GetAlls");
            if (years != null)
            {
                var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
                {
                    Id = x.Id,
                    CreateDate = x.RegisterDate.Value,
                }).Where(m => m.CreateDate.Year == int.Parse(years) && m.CreateDate.Month == month).Count();
                return members;
            }
  
            return 0;
        }
        public int GetMembersOfMonthInActive(int month, string years)
        {
            //_logger.LogDebug($"GetAlls");
            if (years != null)
            {
                var members = UnitOfWork.MemberWalletRepo.GetAll().Where(x=> x.Active ==false).Select(x => new MemberDetailViewModel()
                {
                    Id = x.Id,
                    ChangeDate = x.ChangedTimestamp.Value,
                }).Where(m => m.ChangeDate.Year == int.Parse(years) && m.ChangeDate.Month == month).Count();
                return members;
            }
          
            return 0;
        }
        public int GetStatisticsNewMembers(DateTime dayb, DateTime daye)
        {
            //DateTime dateB = DateTime.Parse(dayb);
            //DateTime dateE = DateTime.Parse(daye);
            var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
            {
                Id = x.Id,
                CreateDate = x.RegisterDate.Value,
            }).Where(m => m.CreateDate >= dayb && m.CreateDate <= daye).Count();
            return members;
        }
        public int GetStatisticsNewMembers()
        {
            var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
            {
                Id = x.Id,
            }).Count();
            return members;
        }
        public int GetStatisticsNewMembersThisMonth(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        {
            var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
            {
                Id = x.Id,
                CreateDate = x.RegisterDate.Value,
            }).Where(m => m.CreateDate > firstDayOfMonth || m.CreateDate == firstDayOfMonth && m.CreateDate < lastDayOfMonth || m.CreateDate == lastDayOfMonth).Count();
            return members;
        }


        public void CreateMember(MemberRecruitmentViewModel model, Guid registrationUser)
        {
            try
            {
                //var memberCode = UnitOfWork.MemberRepo.GetAll().FirstOrDefault(x => x.MemberCode.Equals(model.MemberCode));
                //if (memberCode != null)
                //{
                //    throw new NullReferenceException(Resource.Common_errorMessage_CreateMemberCodeNotFound);
                //}

                _logger.LogDebug($"Creating Member (Code: {model.MemberCode}. Name: {model.FirstName}. Phone: {model.PhoneNumber}. Email: {model.Email})");

                if (model.Month > 0 && model.Day > 0 && model.YOB > 0) model.DOB = new DateTime(model.YOB, model.Month, model.Day);
                Member member = new Member()
                {
                    Id = Guid.NewGuid(),
                    MemberCode = model.MemberCode,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Gender = model.Gender,
                    //YOB = model.YOB,
                    DOB = model.DOB,
                    MaritalStatus = model.MaritalStatus,
                    WeddingDate = model.WeddingDate,
                    JobTitleId = model.JobTitleId,
                    OccupationId = model.OccupationId,
                    AcceptToBeContactedViaMobileEmail = model.AcceptToBeContactedViaMobileEmail,
                    AcceptToReceiveLoyaltyInformation = model.AcceptToReceiveLoyaltyInformation,
                    AcceptToBeContactedViaMobilePhone = model.AcceptToBeContactedViaMobilePhone,
                    AcceptToReceiveMarketing = model.AcceptToReceiveMarketing,
                    NotAcceptAnyContact = model.NotAcceptAnyContact,

                    RegisterDate = DateTime.UtcNow,
                    RegisterEmployee = registrationUser
                };

                if (model.MemberCodeAutomatically)
                {
                    member.MemberCode = this.GenerateMemberCode();
                    _logger.LogDebug($"Creating Member  Code generated: {model.MemberCode}");
                }

                if (model.DOB.HasValue)
                {
                    member.YOB = model.DOB.Value.Year;
                }

                Ats.Domain.Address.Models.Address address = new Address()
                {
                    Id = Guid.NewGuid(),
                    Address1 = model.Address1,
                    ProvinceId = model.ProvinceId.HasValue && model.ProvinceId.Value > 0 ? model.ProvinceId.Value : (int?)null,
                    DistrictId = model.DistrictId.HasValue && model.DistrictId.Value > 0 ? model.DistrictId.Value : (int?)null,
                };

                member.Address = address;
                UnitOfWork.AddressRepo.Insert(address);

                var mbwallet = new MemberWallet();
                mbwallet.Id = new Guid();
                mbwallet.Active = true;
                mbwallet.MemberId = member.Id;

                member.MemberWallets.Add(mbwallet);
                UnitOfWork.MemberWalletRepo.Insert(mbwallet);

                var mbtier = new MemberLoyaltyTier();
                mbtier.Id = new Guid();
                mbtier.MemberId = member.Id;
                mbtier.ActiveFrom = DateTime.UtcNow;
                mbtier.Tier = this.UnitOfWork.LoyaltyTierRepo.GetLowestLoyaltyTierLevel(); // Find lowest Tier level

                _logger.LogDebug($"Creating Member (Name: {model.FirstName}. Phone: {model.PhoneNumber}. Email: {model.Email}) - Tier: {mbtier.Tier}");

                mbtier.Member = member;
                UnitOfWork.MemberLoyaltyTierRepo.Insert(mbtier);

                this.UnitOfWork.MemberRepo.Insert(member);
                UnitOfWork.SaveChanges();

                _logger.LogDebug($"Created Member (Name: {model.FirstName}. Phone: {model.PhoneNumber}. Email: {model.Email}) - id: {model.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create Member exception : {ex})");
                throw ex;
            }
        }

        public void UpdateMember(MemberDetailViewModel model)
        {
            _logger.LogDebug($"Update Member Profile service (Id: {model.Id})");
            if (model.Month > 0 && model.Day > 0 && model.YOB > 0) model.DOB = new DateTime(model.YOB, model.Month.Value, model.Day.Value);
            var entity = UnitOfWork.MemberRepo.GetById(model.Id);
            if (entity != null)
            {
                //General Info
                _logger.LogDebug($"Update entity Member Profile service (entity: { entity.FirstName})");
                entity.MemberCode = model.MemberCode;
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.PhoneNumber = model.PhoneNumber;
                entity.DOB = model.DOB;
                entity.YOB = model.YOB;
                entity.WeddingDate = model.WeddingDate;
                entity.Gender = model.Gender;
                entity.MaritalStatus = model.MaritalStatus;
                entity.AcceptToReceiveMarketing = model.AcceptToReceiveMarketing;
                entity.AcceptToReceiveLoyaltyInformation = model.AcceptToReceiveLoyaltyInformation;
                entity.AcceptToBeContactedViaMobilePhone = model.AcceptToBeContactedViaMobilePhone;
                entity.AcceptToBeContactedViaMobileEmail = model.AcceptToBeContactedViaMobileEmail;
                entity.NotAcceptAnyContact = model.NotAcceptAnyContact;
                entity.StoreId = model.StoreId;
                entity.ChannelId = model.ChannelId;
                ////entity.PreferredBrands = model.Id;

                entity.MemberWallets.FirstOrDefault().Active = model.valid;

                if (entity.Address == null)
                {
                    _logger.LogDebug($"Update Address Member Profile service (AddressId: {entity.HomeAddress})" +
                       $"(__Address:{ entity.Address})");

                    Address address = UnitOfWork.AddressRepo.CreateAddress(model.address1, model.ProvinceId, model.DistrictId);
                    entity.HomeAddress = address.Id;
                    entity.Address = address;
                    UnitOfWork.AddressRepo.Insert(address);
                }
                else
                {
                    _logger.LogDebug($" Update Address Member Profile service (address: { entity.Address.Address1})" +
                     $"(__DistrictId{entity.Address.DistrictId})" +
                     $"(__ProvinceId{entity.Address.ProvinceId})");

                    Address address = entity.Address;

                    address.Address1 = model.address1;
                    address.DistrictId = model.DistrictId;
                    address.ProvinceId = model.ProvinceId;
                    UnitOfWork.AddressRepo.Update(address);
                }

                UnitOfWork.MemberRepo.Update(entity);
                UnitOfWork.SaveChanges();


            }
        }

        public void UpdateMemberDetail(MemberDetailViewModel model)
        {
            _logger.LogDebug($"Update Member Profile service (Id: {model.Id})");
            if (model.Month > 0 && model.Day > 0 && model.YOB > 0) model.DOB = new DateTime(model.YOB, model.Month.Value, model.Day.Value);
            var entity = UnitOfWork.MemberRepo.GetById(model.Id);
            if (entity != null)
            {
                //General Info
                _logger.LogDebug($"Update entity Member Detail service (entity: { entity.FirstName})");
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.PhoneNumber = model.PhoneNumber;
                entity.DOB = model.DOB;
                entity.YOB = model.YOB;
                entity.WeddingDate = model.WeddingDate;
                entity.Gender = model.Gender;
                entity.OccupationId = model.OccupationId;
                entity.JobTitleId = model.JobTitleId;
                entity.MaritalStatus = model.MaritalStatus;

                entity.MemberWallets.FirstOrDefault().Active = model.valid;

                if (entity.Address == null)
                {
                    _logger.LogDebug($"Update Address Member Profile service (AddressId: {entity.HomeAddress})" +
                       $"(__Address:{ entity.Address})");

                    Address address = UnitOfWork.AddressRepo.CreateAddress(model.address1, model.ProvinceId, model.DistrictId);
                    entity.HomeAddress = address.Id;
                    entity.Address = address;
                    UnitOfWork.AddressRepo.Insert(address);
                }
                else
                {
                    _logger.LogDebug($" Update Address Member Profile service (address: { entity.Address.Address1})" +
                     $"(__DistrictId{entity.Address.DistrictId})" +
                     $"(__ProvinceId{entity.Address.ProvinceId})");

                    Address address = entity.Address;

                    address.Address1 = model.address1;
                    address.DistrictId = model.DistrictId;
                    address.ProvinceId = model.ProvinceId;
                    UnitOfWork.AddressRepo.Update(address);
                }

                UnitOfWork.MemberRepo.Update(entity);
                UnitOfWork.SaveChanges();


            }
        }
        public void DeleteMember(Guid id)
        {
            _logger.LogDebug($"Delete Member service (Id: {id})");

            var entity = UnitOfWork.MemberRepo.GetById(id);                  
                        if (entity.MemberCoupons.Count() != 0)
                        {
                            var membercoupon = entity.MemberCoupons.First();
                            UnitOfWork.MemberCouponRepo.Delete(membercoupon);
                        }

                        if (entity.MemberGifts.Count() != 0)
                        {
                            var membergift = entity.MemberGifts.First();
                            UnitOfWork.MemberGiftRepo.Delete(membergift);
                        }
                        if (entity.MemberVouchers.Count() != 0)
                        {
                            var membervoucher = entity.MemberVouchers.First();
                            UnitOfWork.MemberVoucherRepo.Delete(membervoucher);
                        }
                        UnitOfWork.MemberRepo.Delete(entity);
                        UnitOfWork.SaveChanges();
        
        }

        public async Task<object> ImportMemberExcel(MemberColumnMappingViewModel columnMapping, IFormFile formFile, Guid userId)
        {
            int totalRow = 0;
            int batchCount = 0;
            int totalSuccess = 0;
            int totalFail = 0;
            int batchCountToSave = 0;
            int.TryParse(_config.GetValue<string>("CSVMemberImportBatchSave"), out batchCountToSave);
            char csvDelimited = ';';
            char.TryParse(_config.GetValue<string>("CSVDelimited"), out csvDelimited);

            if (batchCountToSave != 0) batchCountToSave = 100;

            _logger.LogDebug($"Start import member from CSV ({csvDelimited}) {formFile.FileName}. UserId: {userId}");
            Stopwatch sw = new Stopwatch();
            sw.Start();
             
            try
            {
                //Save file csv
                string csvFileUrl = SaveFile(formFile, userId);
                var csvFilePath = Path.Combine(_hostingEnvironment.WebRootPath, csvFileUrl);
                FileInfo newFile = new FileInfo(csvFilePath);

                _logger.LogDebug($"Loading CSV file {csvFilePath}.");


                var lsmembers = UnitOfWork.MemberRepo.GetAll().Count();
                var getallmember = UnitOfWork.MemberRepo.GetAll();
                var claimsKey = UnitOfWork.ClaimsReprository.GetAll().Where(x => x.ClaimType.Equals(Constants.Claimskey.SYS_CLAIMS_KEY));
                if(claimsKey == null)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_GiftRedeemed);
                }
                if (claimsKey != null)
                {                  
                    List<String[]> rows = Ats.Commons.CSVParser.Import(csvFilePath, csvDelimited, true, false);
                    totalRow = rows.Count;
                    var claimsValue = claimsKey.FirstOrDefault().ClaimValue;
                    var countMember = totalRow + lsmembers;

                    if( countMember > Int32.Parse(claimsValue))
                    {
                        throw new ApplicationException(Resource.Common_errorMessage_GiftRedeemed);
                    }
                    _logger.LogDebug($"Loaded CSV file {csvFilePath}. Total rows: {totalRow}");

                    Dictionary<MEMBER_IMPORT, int> order = new Dictionary<MEMBER_IMPORT, int>();
                    foreach (MemberItemViewModel mapping in columnMapping.MemberColumnMap)
                    {
                        if (mapping.Col.HasValue)
                            order.Add(mapping.FieldCode, mapping.Col.Value);
                    }

                    // preparing address-provinces

                    List<Ats.Domain.Address.Province> provinces = null;
                    if (order.ContainsKey(MEMBER_IMPORT.PROVINCE))
                    {
                        provinces = this.UnitOfWork.ProvinceRepo.GetAll().ToList();
                    }
                    // preparing address-district
                    List<Ats.Domain.Address.District> districts = null;
                    if (order.ContainsKey(MEMBER_IMPORT.DISTRICT))
                    {
                        districts = this.UnitOfWork.DistrictRepo.GetAll().ToList();
                    }
                  
                    _logger.LogDebug($"Start extracting data {csvFilePath}. Batch Save: {batchCountToSave} items/save");

                    List<MemberExcelViewModel> members = new List<MemberExcelViewModel>();
                    columnMapping.MemberExport = new List<MemberExcelViewModel>();

                    foreach (String[] row in rows)
                    {

                        MemberExcelViewModel member = new MemberExcelViewModel();
                        member.LoyaltyTier = 1; // hard-code temporately

                        // code
                        if (order.ContainsKey(MEMBER_IMPORT.MEMBERCODE))
                        {
                            member.MemberCode = row[order[MEMBER_IMPORT.MEMBERCODE]];
                        }
                        else // generate member code
                        {
                            member.MemberCode = this.GenerateMemberCode();
                        }

                        // firstname
                        if (order.ContainsKey(MEMBER_IMPORT.FIRSTNAME))
                        {
                            member.FirstName = row[order[MEMBER_IMPORT.FIRSTNAME]];
                        }

                        // lastname
                        if (order.ContainsKey(MEMBER_IMPORT.LASTNAME))
                        {
                            member.LastName = row[order[MEMBER_IMPORT.LASTNAME]];
                        }
                        //// gender
                        if (order.ContainsKey(MEMBER_IMPORT.GENDER))
                        {
                            member.SEX = row[order[MEMBER_IMPORT.GENDER]];
                        }

                        // email
                        if (order.ContainsKey(MEMBER_IMPORT.EMAIL))
                        {
                            member.Email = row[order[MEMBER_IMPORT.EMAIL]];
                        }
                        //YOB
                        if (order.ContainsKey(MEMBER_IMPORT.YOB))
                        {
                            member.YOB = row[order[MEMBER_IMPORT.YOB]];
                        }
                        //DOB
                        if (order.ContainsKey(MEMBER_IMPORT.DOB))
                        {
                            member.DOB = row[order[MEMBER_IMPORT.DOB]];
                        }
                        // phone
                        if (order.ContainsKey(MEMBER_IMPORT.PHONE))
                        {
                            member.Phone = row[order[MEMBER_IMPORT.PHONE]];
                        }

                        // address
                        if (order.ContainsKey(MEMBER_IMPORT.ADDRESS1))
                        {
                            member.Address1 = row[order[MEMBER_IMPORT.ADDRESS1]];
                        }

                        // province
                        if (order.ContainsKey(MEMBER_IMPORT.PROVINCE))
                        {
                            member.Province = row[order[MEMBER_IMPORT.PROVINCE]];
                            int? provinceId = this.GetProvinceId(member.Province, provinces);
                            if (provinceId.HasValue && provinceId > 0)
                                member.ProvinceId = provinceId;
                        }

                        // district
                        if (member.ProvinceId.HasValue && order.ContainsKey(MEMBER_IMPORT.DISTRICT))
                        {
                            member.District = row[order[MEMBER_IMPORT.DISTRICT]];
                            int? districtId = this.getDistrictId(member.ProvinceId.Value, member.District, districts);
                            if (districtId.HasValue && districtId > 0)
                                member.DistrictId = districtId;
                        }
                        // validation
                        if (String.IsNullOrEmpty(member.FirstName) || (String.IsNullOrEmpty(member.Phone) && String.IsNullOrEmpty(member.Email)) 
                            || member.Phone != getallmember.FirstOrDefault().PhoneNumber || member.MemberCode != getallmember.FirstOrDefault().MemberCode 
                            || member.Email != getallmember.FirstOrDefault().Email)
                        {
                            member.Status = "Lỗi";
                            columnMapping.MemberExport.Add(member);
                            // not qualify to import
                            continue;
                        }
                        members.Add(member);
                        member.Status = "Thành Công";
                        columnMapping.MemberExport.Add(member);

                        if (members.Count % batchCountToSave == 0)
                        {
                            _logger.LogDebug($"Import Member Extracting data {csvFilePath}. Batch Count: {batchCount}");
                            SaveMembers(members);
                            members.Clear();
                         
                            _logger.LogDebug($"Imported Member data {csvFilePath}. Batch Count: {batchCount}");
                            batchCount++;
                        }
                    };

                    if (members.Count != 0)
                    {
                        _logger.LogDebug($"Import Member Extract data {csvFilePath}. Batch Count: {batchCount}");
                        SaveMembers(members);
                        totalSuccess += members.Count;
                        members.Clear();
                        _logger.LogDebug($"Imported Member data {csvFilePath}. Batch Count: {batchCount}");
                    }
                    if (columnMapping.MemberExport.Count != 0)
                    {
                        _logger.LogDebug($"Import Member Extract data {csvFilePath}. Batch Count: {batchCount}");
                       var lis = columnMapping.MemberExport;
                        totalSuccess += columnMapping.MemberExport.Count;
                      
                        _logger.LogDebug($"Imported Member data {csvFilePath}. Batch Count: {batchCount}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"CSV Member import error: {ex.Message}");
                _logger.LogError($"CSV Member import exception: {ex}");
                throw ex;

            }
            finally
            {
                sw.Stop();
                totalSuccess += batchCount * batchCountToSave;
                _logger.LogDebug($"CSV Member imported '{formFile.FileName}' completed {totalSuccess}/{totalRow} - Time (hh:mm:ss): {sw.Elapsed.Minutes}:{sw.Elapsed.Seconds}:{sw.Elapsed.Milliseconds}");
            }

            return new ResponseData(true, 200, "Success", new { TotalRow = totalRow, Success = totalSuccess, memberexport = columnMapping.MemberExport });
        }
        private void SaveMembers(List<MemberExcelViewModel> members)
        {
            try
            {

                this.UnitOfWork.BeginTransaction();

                foreach (MemberExcelViewModel model in members)
                {
                    this.createMemberEntity(model);
                }

                this.UnitOfWork.SaveChanges();
                this.UnitOfWork.CommitTransaction();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Save import member error:{ex.Message}");
                _logger.LogError($"Save import member exception:{ex}");

                this.UnitOfWork.RollbackTransaction();

                throw ex;
            }
        }

  

        private Guid createMemberEntity(MemberExcelViewModel model)
        {
            Member member = new Member()
            {
                Id = Guid.NewGuid(),
                MemberCode = model.MemberCode,
                FirstName = model.FirstName,
                LastName = model.LastName,
                //YOB = Int32.Parse(model.YOB),

                PhoneNumber = model.Phone,
                Email = model.Email,
                RegisterDate = DateTime.UtcNow,
                RegisterEmployee = model.RegisterEmployee,



            };

            if (string.IsNullOrEmpty(model.YOB))
            {
                member.YOB = 0;
            }
            else
            {
                member.YOB = Int32.Parse(model.YOB);
            }

            if (string.IsNullOrEmpty(model.DOB))
            {
                member.DOB = null;
            }
            else
            {
                var DOBS = model.DOB.StringToDate();
                member.DOB = DOBS;
                member.YOB = DOBS.Year;
                /*  model.DOB = string.Format("{0:dd/MM/yyyy}", member.DOB)*/
                ;
            }

            if (string.IsNullOrEmpty(model.SEX))
            {
                member.Gender = null;
            }
            else if (model.SEX.Trim() == "Nam" || model.SEX.Trim() == "Male" || model.SEX.Trim() == "male" || model.SEX.Trim() == "nam")
            {
                member.Gender = true;
            }
            else if (model.SEX.Trim() == "Nữ" || model.SEX.Trim() == "Female" || model.SEX.Trim() == "female" || model.SEX.Trim() == "nữ")
            {
                member.Gender = false;
            }
            Ats.Domain.Address.Models.Address address = new Address()
            {
                Id = Guid.NewGuid(),
                Address1 = model.Address1,
                ProvinceId = model.ProvinceId,
                DistrictId = model.DistrictId,
            };
            member.Address = address;
            UnitOfWork.AddressRepo.Insert(address);

            var mbwallet = new MemberWallet();
            mbwallet.Id = new Guid();
            mbwallet.MemberId = member.Id;
            mbwallet.Active = true;
            mbwallet.Point = model.LoyaltyPoint;

            member.MemberWallets.Add(mbwallet);
            UnitOfWork.MemberWalletRepo.Insert(mbwallet);

            var mbtier = new MemberLoyaltyTier();
            mbtier.Id = new Guid();
            mbtier.MemberId = member.Id;
            mbtier.ActiveFrom = DateTime.UtcNow;
            mbtier.Tier = model.LoyaltyTier;

            mbtier.Member = member;
            UnitOfWork.MemberLoyaltyTierRepo.Insert(mbtier);

            this.UnitOfWork.MemberRepo.Insert(member);

            return member.Id;
        }

        private string SaveFile(IFormFile file, Guid userId)
        {
            var path = $"{_config.GetValue<string>("FolderImportUploadTemp")}";
            var serverPath = Path.Combine(_hostingEnvironment.WebRootPath, path);

            DirectoryInfo directory = new DirectoryInfo(serverPath);
            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.ToString("ddMMyyyyhhmmss")}{extension}";
            var filePath = Path.Combine(serverPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string url = $"{path}/{fileName}";
            return url;
        }

        private string GenerateMemberCode()
        {
            var generator = new RandomGenerator();
            return generator.RandomCode();
        }


        private int? GetProvinceId(string strProvinceName, List<Ats.Domain.Address.Province> provinces)
        {
            if (!String.IsNullOrEmpty(strProvinceName) && provinces != null)
            {
                return provinces.Where(x => x.Name.Contains(strProvinceName, StringComparison.CurrentCultureIgnoreCase) || strProvinceName.Contains(x.Name, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Id).FirstOrDefault();
            }

            return null;
        }

        private int? getDistrictId(int provinceId, string strDistrictName, List<Ats.Domain.Address.District> districts)
        {
            if (provinceId > 0 && !String.IsNullOrEmpty(strDistrictName) && districts != null)
            {
                return districts.Where(x => x.Name.Contains(strDistrictName, StringComparison.CurrentCultureIgnoreCase) && x.ProvinceId == provinceId).Select(x => x.Id).FirstOrDefault();
            }

            return null;
        }

        public List<MemberViewModel> Reset(MemberSearchView memberSearch, string searchText, int? province, int? occupation, int? jobtitle, int? district, bool? gender, int? storeId, int? channelId, bool? valid, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            throw new NotImplementedException();
        }

        public List<MemberWalletHistoryViewModel> RedeemPoint(Guid id, int page, int size, out int total)
        {

            var memberpoint = UnitOfWork.MemberRepo.GetById(id);
            var memberwallet = memberpoint.MemberWallets.FirstOrDefault();
            var query = UnitOfWork.MemberLoyaltyTransactionRepo.GetAll().Where(m => m.WalletId == memberwallet.Id).ToList().OrderByDescending(x => x.TransactionDate);
            total = query.Count();

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberWalletHistoryViewModel> data = new List<MemberWalletHistoryViewModel>();
            foreach (var item in datas)
            {
                MemberWalletHistoryViewModel loyalTrans = new MemberWalletHistoryViewModel();
                loyalTrans.Id = item.Id;
                loyalTrans.PointTypeId = item.PointTypeId;
                loyalTrans.Point = item.Point;
                loyalTrans.BillNo = item.InvoiceNo;
                loyalTrans.Amount = item.Amount;
                loyalTrans.refId = item.RefId != null ? UnitOfWork.ProductRepo.GetAll().Where(x => x.Id.ToString() == item.RefId).FirstOrDefault().Name : "";



                //loyalTrans.StoreName = item.Store != null ? item.Store.StoreName : null;
                //loyalTrans.ChannelName = item.Channel != null ? item.Channel.ChannelName : null;
                loyalTrans.TransactionDate = item.TransactionDate;
                data.Add(loyalTrans);
            }
            return data;
        }

        public List<GiftVoucherCouponViewModel> GiftVoucherCoupon(Guid id, int page, int size, out int total)
        {
            _logger.LogDebug($"Show List Gift/Voucher/Coupon({id}) Page:{page}");

            var member = UnitOfWork.MemberRepo.GetById(id);
            var memberCoup = member.MemberCoupons.Where(x => x.Valid == true).Select(c => new GiftVoucherCouponViewModel
            {
                Type = GIFT_VOUCHER_COUPON.COUPON.ToName().ToString(),
                Name = c.Coupon.CouponName,
                Code = c.Coupon.CouponCode,
                TypeAmountDiscount = c.Coupon.CouponType,
                Amount = c.Coupon.Amount,
                Discount = c.Coupon.Discount,
                EffectiveFrom = c.EffectiveFrom,
                EffectiveTo = c.EffectiveTo,
                EffectiveFromString = string.Format("{0:dd/MM/yyyy}", c.EffectiveFrom),
                EffectiveToString = string.Format("{0:dd/MM/yyyy}", c.EffectiveTo),
            }).ToList();
            var memberGift = member.MemberGifts.Where(x => x.Valid == true).Select(g => new GiftVoucherCouponViewModel
            {
                Type = GIFT_VOUCHER_COUPON.GIFT.ToName().ToString(),
                Name = g.Gift.GiftName,
                Code = g.Gift.GiftCode,
                TypeAmountDiscount = g.Gift.GiftType,
                Amount = g.Gift.Amount,
                Discount = g.Gift.Discount,
                EffectiveFrom = g.EffectiveFrom,
                EffectiveTo = g.EffectiveTo,
                EffectiveFromString = string.Format("{0:dd/MM/yyyy}", g.EffectiveFrom),
                EffectiveToString = string.Format("{0:dd/MM/yyyy}", g.EffectiveTo),
            }).ToList();
            var memberVou = member.MemberVouchers.Where(x => x.Valid == true).Select(v => new GiftVoucherCouponViewModel
            {
                Type = GIFT_VOUCHER_COUPON.VOUCHER.ToName().ToString(),
                Name = v.Voucher.VoucherName,
                Code = v.Voucher.VoucherCode,
                TypeAmountDiscount = v.Voucher.VoucherType,
                Amount = v.Voucher.Amount,
                Discount = v.Voucher.Discount,
                EffectiveFrom = v.EffectiveFrom,
                EffectiveTo = v.EffectiveTo,
                EffectiveFromString = string.Format("{0:dd/MM/yyyy}", v.EffectiveFrom),
                EffectiveToString = string.Format("{0:dd/MM/yyyy}", v.EffectiveTo),
            }).ToList();
            var query = memberCoup.Union(memberVou).Union(memberGift).ToList();

            total = query.Count();

            var datas = query.Skip((page - 1) * size).Take(size).ToList();

            List<GiftVoucherCouponViewModel> data = new List<GiftVoucherCouponViewModel>();
            foreach (var item in datas)
            {
                GiftVoucherCouponViewModel entity = new GiftVoucherCouponViewModel();
                entity.Type = item.Type;
                entity.Name = item.Name;
                entity.Code = item.Code;
                entity.TypeAmountDiscount = item.TypeAmountDiscount;
                if (item.TypeAmountDiscount == true)
                {
                    entity.Amount = item.Amount;
                }
                else
                {
                    entity.Discount = item.Discount;
                }
                entity.EffectiveFrom = item.EffectiveFrom;
                entity.EffectiveTo = item.EffectiveTo;
                data.Add(entity);
            }
            return data;
        }

        public List<MemberLoyaltyDetailViewModel> Loyalty(Guid id, int page, int size, out int total)
        {
            var memberloyal = UnitOfWork.MemberRepo.GetById(id);
            var query = memberloyal.MemberLoyaltyTiers;

            total = query.Count();

            var datas = query.Skip((page - 1) * size).Take(size).OrderByDescending(p => p.ActiveFrom <= DateTime.Now && DateTime.Now <= p.ActiveEnd ? p.ActiveEnd : null).ToList();
            List<MemberLoyaltyDetailViewModel> data = new List<MemberLoyaltyDetailViewModel>();

            foreach (var item in datas)
            {
                MemberLoyaltyDetailViewModel loyalty = new MemberLoyaltyDetailViewModel();
                loyalty.Id = item.Id;
                loyalty.TierName = item.LoyaltyTier.TierName;
                loyalty.ActiveFrom = item.ActiveFrom;
                loyalty.ActiveEnd = item.ActiveEnd;

                loyalty.ActiveFromString = string.Format("{0:dd/MM/yyyy}", item.ActiveFrom);
                loyalty.ActiveEndString = string.Format("{0:dd/MM/yyyy}", item.ActiveEnd);

                loyalty.UpgradePoint = item.UpgradePoint;
                loyalty.DowngradePoint = item.DowngradePoint;
                data.Add(loyalty);
            }
            return data;

        }

        public void CreateMemberLoyalty(MemberLoyaltyDetailViewModel model, Guid MemberId)
        {
            _logger.LogDebug($"Creating Member Detail Loyalty (Tier: {model.TierId}" +
                $"MemberId: { MemberId })");
            //var tier = UnitOfWork.LoyaltyTierRepo.GetAll().FirstOrDefault();
            try
            {

                if (model.TierId == 0) return;
                MemberLoyaltyTier memberLoyalty = new MemberLoyaltyTier();
                memberLoyalty.Id = Guid.NewGuid();
                memberLoyalty.MemberId = MemberId;
                memberLoyalty.Tier = model.TierId;
                memberLoyalty.ActiveFrom = model.ActiveFrom;
                memberLoyalty.ActiveEnd = model.ActiveEnd;

                //if (tier.TierLevel == (int)TIER_LEVEL.SILVER)
                //{
                //   if(model.UpgradePoint <= tier.PointMax)
                //    {
                //        memberLoyalty.UpgradePoint = model.UpgradePoint;
                //        memberLoyalty.Tier = (int?)TIER_LEVEL.SILVER;
                //    }
                //}
                //else if(tier.TierLevel== (int)TIER_LEVEL.GOLD)
                //{
                //    if (model.UpgradePoint >= tier.PointMin && model.UpgradePoint <= tier.PointMax)
                //    {
                //        memberLoyalty.UpgradePoint = model.UpgradePoint;
                //        memberLoyalty.Tier = (int?)TIER_LEVEL.GOLD;
                //    }
                //}
                //else if (memberLoyalty.Tier == (int)TIER_LEVEL.DIAMOND)
                //{
                //    if (model.UpgradePoint >= tier.PointMin && model.UpgradePoint <= tier.PointMax)
                //    {
                //        memberLoyalty.UpgradePoint = model.UpgradePoint;
                //        memberLoyalty.Tier = (int?)TIER_LEVEL.DIAMOND;
                //    }
                //}
                UnitOfWork.MemberLoyaltyTierRepo.Insert(memberLoyalty);

                //var wallet = memberLoyalty.MemberId;
                //wallet.Point = model.UpgradePoint;
                //UnitOfWork.MemberWalletRepo.Update(wallet);
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create Member Loyalty exception : {ex})");
                throw ex;
            }

        }

        public void UpdateMemberLoyalty(MemberLoyaltyDetailViewModel model)
        {
            _logger.LogDebug($"Update Member Detail Loyalty  (Id: {model.Id})");
            var entity = UnitOfWork.MemberLoyaltyTierRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Tier = model.TierId;
                entity.ActiveFrom = model.ActiveFrom;
                entity.ActiveEnd = model.ActiveEnd;
                entity.UpgradePoint = model.UpgradePoint;
                entity.DowngradePoint = model.DowngradePoint;
            }
            UnitOfWork.MemberLoyaltyTierRepo.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public MemberLoyaltyDetailViewModel GetMemberLoyalty(Guid id)
        {
            _logger.LogDebug($"Get Member Detail Loyalty  (Id: {id})");
            var memberLoyalty = UnitOfWork.MemberLoyaltyTierRepo.GetById(id);
            if (memberLoyalty != null)
            {
                MemberLoyaltyDetailViewModel model = new MemberLoyaltyDetailViewModel();
                model.Id = id;
                model.TierId = memberLoyalty.Tier;
                model.TierName = memberLoyalty.LoyaltyTier.TierName;
                model.MemberId = memberLoyalty.MemberId;
                model.ActiveFrom = memberLoyalty.ActiveFrom;
                model.ActiveEnd = memberLoyalty.ActiveEnd;
                model.UpgradePoint = memberLoyalty.UpgradePoint;
                model.DowngradePoint = memberLoyalty.DowngradePoint;
                return model;
            }
            return null;
        }

        public List<MemberDetailViewModel> GetListMembers()
        {
            _logger.LogDebug($"Get list member");
            var members = UnitOfWork.MemberRepo.GetAll().Select(x => new MemberDetailViewModel()
            {
                Id = x.Id,
                MemberCode = x.MemberCode,
                PhoneNumber = x.PhoneNumber,

            }).OrderBy(x => x.MemberCode).ToList();

            return members;
        }
    }
}