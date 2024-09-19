using Ats.Commons;
using Ats.Commons.Utilities;
using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Gift.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Member.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Member;
using Ats.Models.Product;
using Ats.Models.ResponeResult;
using Ats.Services.Extensions;
using Ats.Services.Implementation;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Ats.Services
{
    public class MemberTagService : BaseService, IMemberTagService
    {
        private IConfiguration _config;
        private int pageSize;

        public MemberTagService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<MemberTagViewModel> Search(string searchText, int? TagCagetoryId, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Member tag service Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.MemberTagRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                           && ((!String.IsNullOrEmpty(x.TagName) && x.TagName.ToLower().Contains(searchText.ToLower()))
                            || (!String.IsNullOrEmpty(x.MemberTagCategory.TagCategoryName) && x.MemberTagCategory.TagCategoryName.ToLower().Contains(searchText.ToLower()))
                            || (!String.IsNullOrEmpty(x.Remark) && x.Remark.ToLower().Contains(searchText.ToLower()))))));

            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active);
                        break;
                    case "tagname":
                        query = IsAscOrder ? query.OrderBy(x => x.TagName) : query.OrderByDescending(x => x.TagName);
                        break;
                    case "membertagcategory":
                        query = IsAscOrder ? query.OrderBy(x => x.TagCagetoryId) : query.OrderByDescending(x => x.TagCagetoryId);
                        break;
                    case "tagtype":
                        query = IsAscOrder ? query.OrderBy(x => x.TagType) : query.OrderByDescending(x => x.TagType);
                        break;
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberTagViewModel> data = new List<MemberTagViewModel>();
            foreach (var item in datas)
            {
                MemberTagViewModel itemMembertag = new MemberTagViewModel();
                itemMembertag.Id = item.Id;
                itemMembertag.TagCagetoryId = item.TagCagetoryId;
                itemMembertag.TagCategoryName = item.MemberTagCategory != null ? item.MemberTagCategory.TagCategoryName : "";
                itemMembertag.TagType = item.TagType;
                itemMembertag.TagName = item.TagName;
                itemMembertag.Active = item.Active;
                itemMembertag.Remark = item.Remark;
                itemMembertag.CreationDate = item.AddedTimestamp;
                itemMembertag.LastUpdates = string.Format("{0:dd/MM/yyyy}", item.AddedTimestamp);
                itemMembertag.TotalMember = item.TotalMember;
                data.Add(itemMembertag);
            }
            return data;
        }
        public List<MemberTagViewModel> GetMemberTags()
        {
            _logger.LogDebug($"Get MemberTags");
            var memberTag = UnitOfWork.MemberTagRepo.GetAll().Where(x => x.Active).Select(x => new MemberTagViewModel()
            {
                Id = x.Id,
                TagName = x.TagName,
                TagType = x.TagType,
            }).OrderBy(x => x.TagName).ToList();

            return memberTag;
        }
        public MemberTagViewModel GetMemberTag(Guid id)
        {
            _logger.LogDebug($"MemberTag Detail service (Id: {id})");
            var entity = UnitOfWork.MemberTagRepo.GetById(id);
            if (entity != null)
            {

                var model = new MemberTagViewModel();
                model.Id = entity.Id;
                model.TagCagetoryId = entity.TagCagetoryId;
                model.TagName = entity.TagName;
                model.TagType = entity.TagType;
                model.TotalMember = entity.TotalMember;
                model.LastUpdates = string.Format("{0:dd/MM/yyyy}", entity.LastUpdate);
              
                model.Remark = entity.Remark;
                model.Active = entity.Active;

                #region member tag values

                List<MemberTagValue> tags = entity.MemberTagValues.ToList();

                //Gender
                MemberTagValue tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.GENDER).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag GENDER (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.GenderSelected = true;
                    model.GenderMale = bool.Parse(vals[0]);
                    model.GenderFemale = bool.Parse(vals[1]);

                }

                //Age
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.AGE).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag AGE (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.AgeSelected = true;
                    model.AgeFrom = vals[0];
                    model.AgeTo = vals[1];

                }

                //Birthday
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.BIRTHDAY).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag BIRTHDAY (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    model.BirthdaySelected = true;
                    model.BirthdayValue = tag.Values;
                }

                // Birthday Month
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.BIRTHDAY_MONTH).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag BIRTHDAY (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.BirthdayMonthSelected = true;
                    model.BirthdayMonthValue = vals;
                    model.BirthdayMonthList = Ultility.EnumToSelectList<NameMonths>(false);
                }

                // City
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.CITY).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag CITY (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.ProvinceSelected = true;
                    model.ProvinceValue = vals;
                }

                // WEDDING_ANIVERSATY
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.WEDDING_ANIVERSATY).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag WEDDING_ANIVERSATY (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.WeddingAnniversarySelected = true;
                    model.WeddingAnniValue = tag.Values;
                }

                // WEDDING_MONTH
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.WEDDING_MONTH).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag WEDDING_MONTH (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.WeddingMonthSelected = true;
                    model.WeddingMonthValue = vals;
                }

                // MARRIRD_STATUS
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.MARRIRD_STATUS).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag MARRIRD_STATUS (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.MarriedStatusSelected = true;
                    model.MarriedStatusValue = vals;
                }

                // MERRIED_YEARS
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.MARRIED_YEARS).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag MERRIED_YEARS (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.MarriedYearSelected = true;
                    model.MarriedYearValue = vals;
                }

                // PREFERRED_BRAND
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.PREFERRED_BRAND).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag PREFERRED_BRAND (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.PreferredBrandSelected = true;
                    model.PreferredBrandValue = vals;
                }

                // PREFERRED_STORE
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.PREFERRED_STORE).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag PREFERRED_STORE (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.PreferredStoreSelected = true;
                    model.PreferredStoreValue = vals;
                }

                // PREFERRER_PRODUCT
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.PREFERRER_PRODUCT).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag PREFERRER_PRODUCT (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.PreferredProductSelected = true;
                    model.PreferredProductValue = vals;
                }

                // TIER
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.TIER).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag TIER (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.MemberTierValue = vals;
                    model.MemberTierSelected = true;
                }

                // REGISTER_DATE
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.REGISTER_DATE).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag REGISTER_DATE (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.RegistrationDateSelected = true;
                    model.RegistrationDateValue = tag.Values;
                }

                // AVAILABLE_POINT
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.AVAILABLE_POINT).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag AVAILABLE_POINT (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.AvailablePointSelected = true;
                    model.PointFrom = vals[0];
                    model.PointTo = vals[1];
                }

                // FIRST_TRANSACTION
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.FIRST_TRANSACTION).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag FIRST_TRANSACTION (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.FirstTransactionDateSelected = true;
                    model.FirstTransactionDateValue = tag.Values;
                }

                // LAST_TRANSACTION
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.LAST_TRANSACTION).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag LAST_TRANSACTION (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.LastTransactionDateSelected = true;
                    model.LastTransactionDateValue = tag.Values;
                }

                // NUMBER_TRANSACTION
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.NUMBER_TRANSACTION).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag NUMBER_TRANSACTION (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.NumberOfTransactionSelected = true;
                    model.NumberOfTransactionValue = vals[0];
                    model.NumberOfTransactionFrom = vals[1];
                    model.NumberOfTransactionTo = vals[2];
                }

                // AMOUNT_TRANSACTION
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.AMOUNT_TRANSACTION).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag AMOUNT_TRANSACTION (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.AmountOfTransactionSelected = true;
                    model.AmountOfTransactionValue = vals[0];
                    model.AmountOfTransactionFrom = vals[1];
                    model.AmountOfTransactionTo = vals[2];
                }

                // MEMBER_LIFECYCLE
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.MEMBER_LIFECYCLE).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag MEMBER_LIFECYCLE (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    model.MemberLifecycleSelected = true;
                    model.MemberLifecycleValue = vals;
                }

                // REGISTER_CHANNEL
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.REGISTER_CHANNEL).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag REGISTER_CHANNEL (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");
                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    model.RegistationChannelSelected = true;
                    model.RegistationChannelMegapointCloud = bool.Parse(vals[0]);
                    model.RegistationChannelCorporateWebsite = bool.Parse(vals[1]);
                    model.RegistationChannelStorePos = bool.Parse(vals[2]);
                    model.RegistationChannelZalo = bool.Parse(vals[3]);
                    model.RegistationChannelECommerce = bool.Parse(vals[4]);
                    model.RegistationChannelMemberPortal = bool.Parse(vals[5]);
                }

                // COMMUNICATION_PREFERENCES
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.COMMUNICATION_PREFERENCES).FirstOrDefault();
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag COMMUNICATION_PREFERENCES (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");
                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    model.CommunicationPreferenceSelected = true;
                    model.CommunicationPreferenceSms = bool.Parse(vals[0]);
                    model.CommunicationPreferenceEmail = bool.Parse(vals[1]);
                    model.CommunicationPreferenceMobile = bool.Parse(vals[2]);
                    model.CommunicationPreferenceMarketing = bool.Parse(vals[3]);
                    model.CommunicationPreferenceLoayalty = bool.Parse(vals[4]);
                }

                #endregion

                return model;
            }

            return null;
        }
        public Guid Create(MemberTagViewModel model)
        {
            _logger.LogDebug($"Creating (Name: {model.TagName})");
            var entity = new MemberTag
            {
                Id = Guid.NewGuid(),
                TagCagetoryId = model.TagCagetoryId,
                TagName = model.TagName,
                Remark = model.Remark,
                Active = model.Active,
                TagType = model.TagType

            };
            UnitOfWork.MemberTagRepo.Insert(entity);

            #region membber tag values
            _logger.LogDebug($"Create (Name: {model.TagName}), (Id: {entity.Id})");

            // Gender
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.GENDER.ToName()})");
            if (model.GenderSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.GENDER;
                tagValue.Values = $"{model.GenderMale}; {model.GenderFemale}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }

            // Age
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.AGE.ToName()})");
            if (model.AgeSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.AGE;
                tagValue.Values = $"{model.AgeFrom}; {model.AgeTo}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }

            // Birthday
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.BIRTHDAY.ToName()})");
            if (model.BirthdaySelected)
            {

                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.BIRTHDAY;
                // create new
                tagValue.Values = $"{model.BirthdayValue}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            // Birthday Month
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.BIRTHDAY_MONTH.ToName()})");
            if (model.BirthdayMonthSelected)
            {

                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.BIRTHDAY_MONTH;
                tagValue.Values = model.BirthdayMonthValue.JoinToString();
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            // Province
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.CITY.ToName()})");
            if (model.ProvinceSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.CITY;
                tagValue.Values = model.ProvinceValue.JoinToString();
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            // WeddingAnni
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.WEDDING_ANIVERSATY.ToName()})");
            if (model.WeddingAnniversarySelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.WEDDING_ANIVERSATY;
                tagValue.Values = $"{model.WeddingAnniValue}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Merried Year
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.MARRIED_YEARS.ToName()})");
            if (model.MarriedYearSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.MARRIED_YEARS;
                string Array = model.MarriedYearValue.JoinToString();
                tagValue.Values = Array;
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Wedding Month
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.WEDDING_MONTH.ToName()})");
            if (model.WeddingMonthSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.WEDDING_MONTH;
                tagValue.Values = model.WeddingMonthValue.JoinToString();
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Married Status
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.MARRIRD_STATUS.ToName()})");
            if (model.MarriedStatusSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.MARRIRD_STATUS;
                tagValue.Values = model.MarriedStatusValue.JoinToString();
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Preferred Brand
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.PREFERRED_BRAND.ToName()})");
            if (model.PreferredBrandSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.PREFERRED_BRAND;
                tagValue.Values = model.PreferredBrandValue.JoinToString();
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Preferred Store
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.PREFERRED_STORE.ToName()})");
            if (model.PreferredStoreSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.PREFERRED_STORE;
                string Array = model.PreferredStoreValue.JoinToString();
                tagValue.Values = Array;
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Preferred product
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.PREFERRER_PRODUCT.ToName()})");
            if (model.PreferredProductSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.PREFERRER_PRODUCT;
                tagValue.Values = model.PreferredProductValue.JoinToString();
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Tier

            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.TIER.ToName()})");
            if (model.MemberTierSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.TIER;
                tagValue.Values = model.MemberTierValue.JoinToString();
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Registration Date
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.REGISTER_DATE.ToName()})");
            if (model.RegistrationDateSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.REGISTER_DATE;
                tagValue.Values = $"{model.RegistrationDateValue}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Available Point
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.AVAILABLE_POINT.ToName()})");
            if (model.AvailablePointSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.AVAILABLE_POINT;
                tagValue.Values = $"{model.PointFrom} ; {model.PointTo}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //Registration Channel
            _logger.LogDebug($"Create MemberTag({model.TagName},{entity.Id}) - TagValue ({TAG_KEY.REGISTER_CHANNEL.ToName()})");
            if (model.RegistationChannelSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.REGISTER_CHANNEL;
                tagValue.Values = $"{model.RegistationChannelMegapointCloud}; {model.RegistationChannelCorporateWebsite}; {model.RegistationChannelStorePos}; {model.RegistationChannelZalo}; {model.RegistationChannelECommerce}; {model.RegistationChannelMemberPortal}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }

            if (model.FirstTransactionDateSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.FIRST_TRANSACTION;
                tagValue.Values = $"{model.FirstTransactionDateValue}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            if (model.LastTransactionDateSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.LAST_TRANSACTION;
                tagValue.Values = $"{model.LastTransactionDateValue}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            if (model.NumberOfTransactionSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.NUMBER_TRANSACTION;
                tagValue.Values = $"{model.NumberOfTransactionValue}; {model.NumberOfTransactionFrom}; {model.NumberOfTransactionTo}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            if (model.AmountOfTransactionSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.AMOUNT_TRANSACTION;
                tagValue.Values = $"{model.AmountOfTransactionValue}; {model.AmountOfTransactionFrom}; {model.AmountOfTransactionTo}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }
            //// Member Lifecycle
            if (model.MemberLifecycleSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.MEMBER_LIFECYCLE;
                tagValue.Values = model.MemberLifecycleValue.JoinToString();
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }

            ////Communication Preference 
            if (model.CommunicationPreferenceSelected)
            {
                MemberTagValue tagValue = new MemberTagValue();
                tagValue.Id = Guid.NewGuid();
                tagValue.MemberTagId = entity.Id;
                tagValue.TagKeyId = (int)TAG_KEY.COMMUNICATION_PREFERENCES;
                tagValue.Values = $"{model.CommunicationPreferenceSms}; {model.CommunicationPreferenceEmail}; {model.CommunicationPreferenceMobile}; {model.CommunicationPreferenceMarketing}; {model.CommunicationPreferenceLoayalty}";
                entity.MemberTagValues.Add(tagValue);
                UnitOfWork.MemberTagValueRepo.Insert(tagValue);
            }

            #endregion

            UnitOfWork.SaveChanges();

            _logger.LogDebug($"Created (Name: {entity.TagName}, Id: {entity.Id})");

            return entity.Id;
        }
        public void Update(MemberTagViewModel model)
        {
            _logger.LogDebug($"Edit MemberTag (Id: {model.Id})");
            var entity = UnitOfWork.MemberTagRepo.GetById(model.Id);

            if (entity != null)
            {
                entity.TagCagetoryId = model.TagCagetoryId;
                entity.TagName = model.TagName;
                entity.TagType = model.TagType;
                entity.Remark = model.Remark;
                entity.Active = model.Active;
                UnitOfWork.MemberTagRepo.Update(entity);

                List<MemberTagValue> tags = entity.MemberTagValues.ToList();
                #region List<MemberTagValue> 

                ////Gender
                this.UpdateMemberTagValue(TAG_KEY.GENDER, model, tags, entity, model.GenderSelected, $"{model.GenderMale}; {model.GenderFemale}");
                ////Age
                this.UpdateMemberTagValue(TAG_KEY.AGE, model, tags, entity, model.AgeSelected, $"{model.AgeFrom}; {model.AgeTo}");
                ////Birthday
                this.UpdateMemberTagValue(TAG_KEY.BIRTHDAY, model, tags, entity, model.BirthdaySelected, $"{model.BirthdayValue}");
                ////BirthdayMonth
                string arrayBtm = model.BirthdayMonthSelected ? model.BirthdayMonthValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.BIRTHDAY_MONTH, model, tags, entity, model.BirthdayMonthSelected, $"{arrayBtm}");
                ////Province               
                string arrayPro = model.ProvinceSelected ? model.ProvinceValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.CITY, model, tags, entity, model.ProvinceSelected, $"{arrayPro}");
                ////WeddingAnniversary   
                this.UpdateMemberTagValue(TAG_KEY.WEDDING_ANIVERSATY, model, tags, entity, model.WeddingAnniversarySelected, $"{model.WeddingAnniValue}");
                ////WeddingMonth                 
                string arrayWeddMonth = model.WeddingMonthSelected ? model.WeddingMonthValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.WEDDING_MONTH, model, tags, entity, model.WeddingMonthSelected, $"{arrayWeddMonth}");
                ////MarriedStatus   
                string arrayMarStatus = model.MarriedStatusSelected ? model.MarriedStatusValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.MARRIRD_STATUS, model, tags, entity, model.MarriedStatusSelected, $"{arrayMarStatus}");
                ////MarriedYear   
                string arrayMarYears = model.MarriedYearSelected ? model.MarriedYearValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.MARRIED_YEARS, model, tags, entity, model.MarriedYearSelected, $"{arrayMarYears}");
                ////PreferredBrand   
                string arrayPreBrand = model.PreferredBrandSelected ? model.PreferredBrandValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.PREFERRED_BRAND, model, tags, entity, model.PreferredBrandSelected, $"{arrayPreBrand}");
                ////PreferredStore   
                string arrayPreStore = model.PreferredStoreSelected ? model.PreferredStoreValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.PREFERRED_STORE, model, tags, entity, model.PreferredStoreSelected, $"{arrayPreStore}");
                ////PreferredProduct   
                string arrayPreProduct = model.PreferredProductSelected ? model.PreferredProductValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.PREFERRER_PRODUCT, model, tags, entity, model.PreferredProductSelected, $"{arrayPreProduct}");
                ////MemberTier   
                string arrayMbTier = model.MemberTierSelected ? model.MemberTierValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.TIER, model, tags, entity, model.MemberTierSelected, $"{arrayMbTier}");
                ////RegistrationDate   
                this.UpdateMemberTagValue(TAG_KEY.REGISTER_DATE, model, tags, entity, model.RegistrationDateSelected, $"{model.RegistrationDateValue}");
                this.UpdateMemberTagValue(TAG_KEY.AVAILABLE_POINT, model, tags, entity, model.AvailablePointSelected, $"{model.PointFrom}; {model.PointTo}");
                this.UpdateMemberTagValue(TAG_KEY.FIRST_TRANSACTION, model, tags, entity, model.RegistrationDateSelected, $"{model.FirstTransactionDateValue}");
                this.UpdateMemberTagValue(TAG_KEY.LAST_TRANSACTION, model, tags, entity, model.AvailablePointSelected, $"{model.LastTransactionDateValue}");
                this.UpdateMemberTagValue(TAG_KEY.NUMBER_TRANSACTION, model, tags, entity, model.AvailablePointSelected, $"{model.NumberOfTransactionValue}");
                this.UpdateMemberTagValue(TAG_KEY.AMOUNT_TRANSACTION, model, tags, entity, model.AmountOfTransactionSelected, $"{model.AmountOfTransactionValue}; {model.AmountOfTransactionFrom}; {model.AmountOfTransactionTo}");
                ////Member Lifecycle
                string arrayMbLifecycle = model.MemberLifecycleSelected ? model.MemberLifecycleValue.JoinToString() : null;
                this.UpdateMemberTagValue(TAG_KEY.MEMBER_LIFECYCLE, model, tags, entity, model.MemberLifecycleSelected, $"{arrayMbLifecycle}");
                ////Communication Preferrences
                this.UpdateMemberTagValue(TAG_KEY.COMMUNICATION_PREFERENCES, model, tags, entity, model.CommunicationPreferenceSelected, $"{model.CommunicationPreferenceSms}; {model.CommunicationPreferenceEmail}; {model.CommunicationPreferenceMobile}; {model.CommunicationPreferenceMarketing}; {model.CommunicationPreferenceLoayalty}");
                ////Registration Channel
                this.UpdateMemberTagValue(TAG_KEY.REGISTER_CHANNEL, model, tags, entity, model.RegistationChannelSelected, $"{model.RegistationChannelMegapointCloud}; {model.RegistationChannelCorporateWebsite}; {model.RegistationChannelStorePos}; {model.RegistationChannelZalo}; {model.RegistationChannelECommerce}; {model.RegistationChannelMemberPortal}");
                ////Number Of Transaction
                this.UpdateMemberTagValue(TAG_KEY.NUMBER_TRANSACTION, model, tags, entity, model.NumberOfTransactionSelected, $"{model.NumberOfTransactionValue}; {model.NumberOfTransactionFrom}; {model.NumberOfTransactionTo}");

                #endregion
            }
            UnitOfWork.SaveChanges();
        }

        private void UpdateMemberTagValue(TAG_KEY tagkey, MemberTagViewModel model, List<MemberTagValue> tags, MemberTag entity, bool selected, string values)
        {
            MemberTagValue tag = tags.Where(x => x.TagKeyId == (int)tagkey).FirstOrDefault();
            if (selected)
            {
                if (tag == null)
                {
                    MemberTagValue tagValue = new MemberTagValue();
                    tagValue.Id = Guid.NewGuid();
                    tagValue.MemberTagId = entity.Id;
                    tagValue.TagKeyId = (int)tagkey;
                    tagValue.Values = values;
                    entity.MemberTagValues.Add(tagValue);
                    this.UnitOfWork.MemberTagValueRepo.Insert(tagValue);
                }
                else
                {
                    tag.Values = values;
                    this.UnitOfWork.MemberTagValueRepo.Update(tag, x => x.Values);
                }
            }
            else
            {
                if (tag != null)
                {
                    this.UnitOfWork.MemberTagValueRepo.Delete(tag.Id);
                }
            }
        }

        public void Delete(Guid id)
        {
            _logger.LogDebug($"Delete  MemberTag(Id: {id})");
            var entity = UnitOfWork.MemberTagRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.MemberTagRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<MemberTagCategory> GetMembertagCategory()
        {
            _logger.LogDebug($"Get MemberTag Categories Enter.");
            var membertagCategory = UnitOfWork.MemberTagCategoryRepo.GetAll().Where(x => x.Active).Select(x => new MemberTagCategory()
            {
                Id = x.Id,
                TagCategoryName = x.TagCategoryName
            }).OrderBy(x => x.TagCategoryName).ToList();
            return membertagCategory;
        }

        public List<MemberTag> GetMemberTag()
        {
            _logger.LogDebug($"Get MemberTag Enter.");
            var memberTag = UnitOfWork.MemberTagRepo.GetAll().Where(x => x.Active).Select(x => new MemberTag()
            {
                Id = x.Id,
                TagName = x.TagName
            }).OrderBy(x => x.TagName).ToList();
            return memberTag;
        }
        
        public int Analyst(Guid id)
        {
            _logger.LogDebug($"Member Analyst Enter.");
            int total = 0;
            var entity = this.UnitOfWork.MemberTagRepo.GetById(id);

            if (entity != null)
            {
                _logger.LogDebug($"Member Analyst Found '{entity.TagName}' ({id})");
                List<MemberTagValue> tags = entity.MemberTagValues.ToList();
                _logger.LogDebug($"Member Analyst Found '{entity.TagName}' ({id}) with {tags.Count} tags");
                IQueryable<Member> memberQuery = this.UnitOfWork.MemberRepo.GetAll();

                //Member mb = tags.Where(x=> x.)
                ParameterExpression memberExpr = Expression.Parameter(typeof(Member), "m");     

                MemberExpression activeId = Expression.Property(memberExpr, "Id");
                var activeClause = Expression.NotEqual(activeId, Expression.Constant(Guid.Empty, typeof(Guid)));

                var ftFullQueryExpression = activeClause;

                // Gender
                MemberTagValue tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.GENDER).FirstOrDefault();
                #region GENDER
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag GENDER (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    var selectMale = bool.Parse(vals[0]);
                    var selectFemale = bool.Parse(vals[1]);

                    MemberExpression gender = Expression.Property(memberExpr, "Gender");
                    if (selectMale && selectFemale)
                    {

                        var genderClause = Expression.NotEqual(gender, Expression.Constant(null, typeof(Nullable<bool>)));
                        ftFullQueryExpression = Expression.AndAlso(ftFullQueryExpression, genderClause);
                    }
                    else if (selectMale)
                    {
                        var genderClause = Expression.Equal(gender, Expression.Constant(true, typeof(Nullable<bool>)));
                        ftFullQueryExpression = Expression.AndAlso(ftFullQueryExpression, genderClause);

                    }
                    else if (selectFemale)
                    {
                        var genderClause = Expression.Equal(gender, Expression.Constant(false, typeof(Nullable<bool>)));
                        ftFullQueryExpression = Expression.AndAlso(ftFullQueryExpression, genderClause);
                    }
                }
                #endregion

                // Age
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.AGE).FirstOrDefault();
                #region AGE
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag AGE (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    int ageFrom = 0;
                    int.TryParse(vals[0], out ageFrom);

                    int ageTo = 0;
                    int.TryParse(vals[1], out ageTo);

                    MemberExpression yob = Expression.Property(memberExpr, "YOB");

                    if (ageFrom > 0)
                    {
                        var clause = Expression.LessThanOrEqual(yob, Expression.Constant(DateTime.Now.Year - ageFrom, typeof(int)));
                        ftFullQueryExpression = Expression.AndAlso(ftFullQueryExpression, clause);
                    }

                    if (ageTo > 0)
                    {
                        var clause = Expression.GreaterThanOrEqual(yob, Expression.Constant(DateTime.Now.Year - ageTo, typeof(int)));
                        ftFullQueryExpression = Expression.AndAlso(ftFullQueryExpression, clause);

                    }
                }
                #endregion
                // Birthday
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.BIRTHDAY).FirstOrDefault();
                #region BIRTHDAY
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag BIRTHDAY (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    int lastBirthday = 0;
                    int.TryParse(value, out lastBirthday);

                    if (lastBirthday >= 7)
                    {
                        memberQuery= memberQuery.Where(m =>  m.DOB.Value.DayOfYear >= DateTime.UtcNow.AddDays(-lastBirthday).DayOfYear && m.DOB.Value.DayOfYear <= DateTime.UtcNow.DayOfYear);

                    }else if(lastBirthday == 1)
                    {
                        memberQuery = memberQuery.Where(m => m.DOB.Value.DayOfYear == DateTime.UtcNow.DayOfYear);

                    }
                }
                #endregion

                //Birthday months
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.BIRTHDAY_MONTH).FirstOrDefault();
                #region BIRTHDAY_MONTH
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag BIRTHDAY_MONTH (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int birthday = 0;
                    var birthdays = Array.ConvertAll(vals, s => int.TryParse(s, out birthday) ? birthday : 0);

                    if (birthdays.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => birthdays.Contains(m.DOB.Value.Month));
                    }
                }
                #endregion

                //Provice
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.CITY).FirstOrDefault();
                #region PROVICE(CITY)
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag PROVICE(CITY) (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int province = 0;
                    var provinces = Array.ConvertAll(vals, s => int.TryParse(s, out province) ? province : 0).Where(x => x != 0).ToArray();

                    if (provinces.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => provinces.Contains(m.Address.ProvinceId.Value));
                    }
                }
                #endregion

                //Wedding anniversary
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.WEDDING_ANIVERSATY).FirstOrDefault();
                #region WEDDING ANIVERSATY
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag WEDDING ANIVERSATY (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    int lastWeddingAnniversary = 0;
                    int.TryParse(value, out lastWeddingAnniversary);

                    if (lastWeddingAnniversary >= 7)
                    {
                        
                        memberQuery = memberQuery.Where(m => m.WeddingDate.Value.DayOfYear <= DateTime.Now.DayOfYear && m.WeddingDate.Value.DayOfYear >= DateTime.Now.AddDays(-lastWeddingAnniversary).DayOfYear);
                    }
                    else if(lastWeddingAnniversary == 1)
                    {
                        memberQuery = memberQuery.Where(m => m.WeddingDate.Value.DayOfYear == DateTime.Now.DayOfYear);
                    }
                }
                #endregion

                //Married Year
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.MARRIED_YEARS).FirstOrDefault();
                #region MARRIED_YEARS
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag MARRIED_YEARS (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int marriedYear = 0;
                    var marriedYears = Array.ConvertAll(vals, s => int.TryParse(s, out marriedYear) ? marriedYear : 0).Where(x => x != 0).ToArray();

                    if (marriedYears.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => marriedYears.Contains(m.WeddingDate.Value.Year));
                    }
                }
                #endregion

                //Wedding Month
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.WEDDING_MONTH).FirstOrDefault();
                #region WEDDING_MONTH
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag WEDDING_MONTH (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int weddingMonth = 0;
                    var weddingMonths = Array.ConvertAll(vals, s => int.TryParse(s, out weddingMonth) ? weddingMonth : 0).Where(x => x != 0).ToArray();

                    if (weddingMonths.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => weddingMonths.Contains(m.WeddingDate.Value.Month));
                    }
                }
                #endregion

                //Maratial Status
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.MARRIRD_STATUS).FirstOrDefault();
                #region MARRIRD_STATUS
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag MARRIRD_STATUS (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int marriedStatus = 0;
                    var marriedStatuses = Array.ConvertAll(vals, s => int.TryParse(s, out marriedStatus) ? marriedStatus : 0).ToArray();

                    if (marriedStatuses.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => marriedStatuses.Contains((int)m.MaritalStatus.Value));
                    }
                }
                #endregion

                //Preferred brand
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.PREFERRED_BRAND).FirstOrDefault();
                #region PREFERRED_BRAND
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag PREFERRED_BRAND (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int preferredBrand = 0;
                    var preferredBrands = Array.ConvertAll(vals, s => int.TryParse(s, out preferredBrand) ? preferredBrand : 0).Where(x => x != 0).ToArray();


                }
                #endregion

                //Preferred store
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.PREFERRED_STORE).FirstOrDefault();
                #region PREFERRED_STORE
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag PREFERRED_STORE (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int preferredStore = 0;
                    var preferredStores = Array.ConvertAll(vals, s => int.TryParse(s, out preferredStore) ? preferredStore : 0).Where(x => x != 0).ToArray();

                    if (preferredStores.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => preferredStores.Contains(m.StoreId.Value));
                    }
                }
                #endregion

                //Preferred product
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.PREFERRER_PRODUCT).FirstOrDefault();
                #region PREFERRER_PRODUCT
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag PREFERRER_PRODUCT (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int preferredProduct = 0;
                    var preferredProducts = Array.ConvertAll(vals, s => int.TryParse(s, out preferredProduct) ? preferredProduct : 0).Where(x => x != 0).ToArray();

                    if (preferredProducts.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.PreferredProducts.Any(pp => preferredProducts.Contains(pp.ProductID.Value)));
                    }
                }
                #endregion

                //Tier
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.TIER).FirstOrDefault();
                #region TIER
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag TIER (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int tier = 0;
                    var tiers = Array.ConvertAll(vals, s => int.TryParse(s, out tier) ? tier : 0).Where(x => x != 0).ToArray();

                    if (tiers.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.MemberLoyaltyTiers.Any(pp => tiers.Contains(pp.Tier.Value)));
                    }
                }
                #endregion

                ////Loyalty Tier
                //tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.TIER).FirstOrDefault();
                //#region TIER
                //if (tag != null)
                //{
                //    _logger.LogDebug($"MemberTag TIER (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                //    string value = tag.Values ?? "";
                //    string[] vals = value.Split(';');
                //    int tier = 0;
                //    var tiers = Array.ConvertAll(vals, s => int.TryParse(s, out tier) ? tier : 0).Where(x => x != 0).ToArray();

                //    if (tiers.Length > 0)
                //    {
                //        memberQuery = memberQuery.Where(m => m.MemberLoyaltyTiers.Any(pp => tiers.Contains(pp.Tier.Value)));
                //    }
                //}
                //#endregion

                // Register Date
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.REGISTER_DATE).FirstOrDefault();
                #region REGISTER_DATE
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag TIER (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    DateTime date = new DateTime();
                    DateTime? registerDate = DateTime.TryParse(value, out date) ? (DateTime?)date : null;

                    if (registerDate.HasValue)
                    {
                        memberQuery = memberQuery.Where(m => m.RegisterDate.Value.Date.Equals(registerDate.Value.Date));
                    }
                }
                #endregion

                //Available Point
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.AVAILABLE_POINT).FirstOrDefault();
                #region AVAILABLE_POINT
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag TIER (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');

                    int pointFrom = 0;
                    int.TryParse(vals[0], out pointFrom);

                    int pointTo = 0;
                    int.TryParse(vals[1], out pointTo);

                    if (pointFrom > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.MemberWallets.Select(mw => mw.Point).FirstOrDefault() >= pointFrom);
                    }
                    if (pointTo > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.MemberWallets.Select(mw => mw.Point).FirstOrDefault() <= pointTo);
                    }
                }
                #endregion

                //Channel
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.REGISTER_CHANNEL).FirstOrDefault();
                #region REGISTER_CHANNEL
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag TIER (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    bool registationChannelMegapointCloud = bool.Parse(vals[0]);
                    bool registationChannelCorporateWebsite = bool.Parse(vals[1]);
                    bool registationChannelStorePos = bool.Parse(vals[2]);
                    bool registationChannelZalo = bool.Parse(vals[3]);
                    bool registationChannelECommerce = bool.Parse(vals[4]);
                    bool registationChannelMemberPortal = bool.Parse(vals[5]);
                }
                #endregion

                //First Transaction Date
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.FIRST_TRANSACTION).FirstOrDefault();
                #region FIRST_TRANSACTION
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag FIRST_TRANSACTION (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    int month = 0;
                    int.TryParse(value, out month);

                    if (month > 0)
                    {
                        memberQuery = memberQuery.Where(x => x.PurchasedTransactions.Min(pt => pt.PurchasedDate.Date) <= DateTime.Now.AddMonths(-month).Date);
                    }
                }
                #endregion

                //Last Transaction Date
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.LAST_TRANSACTION).FirstOrDefault();
                #region LAST_TRANSACTION
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag LAST_TRANSACTION (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    int month = 0;
                    int.TryParse(value, out month);

                    if (month > 0)
                    {
                        if (month > 0)
                        {
                            memberQuery = memberQuery.Where(x => x.PurchasedTransactions.Max(pt => pt.PurchasedDate.Date) <= DateTime.Now.AddMonths(-month).Date);
                        }
                    }
                }
                #endregion

                //Number Of Transaction
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.NUMBER_TRANSACTION).FirstOrDefault();
                #region NUMBER_TRANSACTION
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag LAST_TRANSACTION (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int month, numberTranFrom, numberTranTo = 0;
                    int.TryParse(vals[0], out month);
                    int.TryParse(vals[1], out numberTranFrom);
                    int.TryParse(vals[2], out numberTranTo);
                  
                    if (numberTranFrom > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.PurchasedTransactions.Count(pt => month == 0 || pt.PurchasedDate.Date <= DateTime.Now.AddMonths(-month).Date) >= numberTranFrom);
                    }
                    if (numberTranTo > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.PurchasedTransactions.Count(pt => month == 0 || pt.PurchasedDate.Date <= DateTime.Now.AddMonths(-month).Date) <= numberTranTo);
                    }
                }
                #endregion

                //Amount Of Transaction
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.AMOUNT_TRANSACTION).FirstOrDefault();
                #region AMOUNT_TRANSACTION
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag AMOUNT_TRANSACTION (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int month, amountTranFrom, amountTranTo = 0;
                    int.TryParse(vals[0], out month);
                    int.TryParse(vals[1], out amountTranFrom);
                    int.TryParse(vals[2], out amountTranTo);                  
                   
                    if (amountTranFrom > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.PurchasedTransactions.Where(pt => month == 0 || pt.PurchasedDate.Date <= DateTime.Now.AddMonths(-month).Date).Sum(pt => pt.Amount) >= amountTranFrom);
                    }
                    if (amountTranTo > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.PurchasedTransactions.Where(pt => month == 0 || pt.PurchasedDate.Date <= DateTime.Now.AddMonths(-month).Date).Sum(pt => pt.Amount) <= amountTranTo);
                    }
                }
                #endregion


                //Member Life Cycle Opt-in
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.MEMBER_LIFECYCLE).FirstOrDefault();
                #region MEMBER_LIFECYCLE
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag MEMBER_LIFECYCLE (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    int lifecycle = 0;
                    var lifecycles = Array.ConvertAll(vals, s => int.TryParse(s, out lifecycle) ? lifecycle : 0).Where(x => x != 0).ToArray();
                    if (lifecycles.Length > 0)
                    {
                        memberQuery = memberQuery.Where(m => m.MemberLifeCycleLinks.Any(mc => lifecycles.Contains(mc.MemberLifeCycleId)));
                    }
                }
                #endregion

                //Communication Preferrences
                tag = tags.Where(x => x.TagKeyId == (int)TAG_KEY.COMMUNICATION_PREFERENCES).FirstOrDefault();
                #region COMMUNICATION_PREFERENCES
                if (tag != null)
                {
                    _logger.LogDebug($"MemberTag COMMUNICATION_PREFERENCES (Id tag value: {tag.Values}, id tag key: {tag.TagKeyId})");

                    string value = tag.Values ?? "";
                    string[] vals = value.Split(';');
                    bool communicationPreferenceSms = bool.Parse(vals[0]);
                    bool communicationPreferenceEmail = bool.Parse(vals[1]);
                    bool communicationPreferenceMobile = bool.Parse(vals[2]);
                    bool communicationPreferenceMarketing = bool.Parse(vals[3]);
                    bool communicationPreferenceLoayalty = bool.Parse(vals[4]);

                    //Todo update query late

                }
                #endregion

                var ftLambda = Expression.Lambda<Func<Member, bool>>(ftFullQueryExpression, memberExpr);

                memberQuery = memberQuery.Where(ftLambda);

                List<Guid> members = memberQuery.Select(x => x.Id).ToList();
                total = members.Count;

                this.UpdateMemberTagging(entity.Id, members);
            }

            return total;
        }

        private void UpdateMemberTagging(Guid memberTagId, List<Guid> memberIds)
        {
            this.UnitOfWork.MemberTagRepo.UpdateMemberTagging(memberTagId, memberIds);

            this.UnitOfWork.SaveChanges();
        }

        public List<DisplayItem> GetMemberTagsForDisplay(Guid memberId)
        {
            return this.UnitOfWork.MemberTagRepo.GetMemberTagLabels(memberId).Select(x => new DisplayItem() { Text = x.TagName, Value = x.Id.ToString() }).ToList();
        }
    }
}
