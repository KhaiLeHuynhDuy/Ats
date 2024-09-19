using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Channel.Models;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Coupon;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Ats.Commons;

namespace Ats.Services.Implementation
{
    public class CouponService : BaseService, ICouponService
    {
        private IConfiguration _config;
        private int pageSize;

        public CouponService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<CouponViewModel> Search(string searchText, string orderField, bool IsAscOrder, bool? couponType, int? channelId,
            string? fromEffectiveDateBegin, string? toEffectiveDateBegin, string? fromEffectiveDateEnd, string? toEffectiveDateEnd, EXPIRY_COUPON? expirycoupon,
            int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            DateTime? _fromEffectiveDateBegin, _toEffectiveDateBegin, _fromEffectiveDateEnd, _toEffectiveDateEnd;

            if (fromEffectiveDateBegin != null) { _fromEffectiveDateBegin = DateTime.Parse(fromEffectiveDateBegin).EarlyInDay(); }
            else { _fromEffectiveDateBegin = null; }

            if (toEffectiveDateBegin != null) { _toEffectiveDateBegin = DateTime.Parse(toEffectiveDateBegin).EndInDay(); }
            else { _toEffectiveDateBegin = null; }

            if (fromEffectiveDateEnd != null) { _fromEffectiveDateEnd = DateTime.Parse(fromEffectiveDateEnd + " 00:00"); }
            else { _fromEffectiveDateEnd = null; }

            if (toEffectiveDateEnd != null) { _toEffectiveDateEnd = DateTime.Parse(toEffectiveDateEnd + " 23:59"); }
            else { _toEffectiveDateEnd = null; }

            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();

            var query = UnitOfWork.CouponRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                                   && ((!String.IsNullOrEmpty(x.CouponCode) && x.CouponCode.ToLower().Contains(searchText.ToLower()))
                                   || (!String.IsNullOrEmpty(x.CouponName) && x.CouponName.ToLower().Contains(searchText.ToLower())))))
                                   && (couponType == null ? true : x.CouponType == couponType)
                                   && (channelId == 0 || channelId == null ? true : x.ChannelId == channelId)
                                   && (
                                        (_fromEffectiveDateBegin == null && _toEffectiveDateBegin == null) ||
                                        (_fromEffectiveDateBegin == null && _toEffectiveDateBegin != null && x.EffectiveDateBegin <= _toEffectiveDateBegin) ||
                                        (_toEffectiveDateBegin == null && _fromEffectiveDateBegin != null && x.EffectiveDateBegin >= _fromEffectiveDateBegin) ||
                                        (_fromEffectiveDateBegin != null && _toEffectiveDateBegin != null && x.EffectiveDateBegin >= _fromEffectiveDateBegin && x.EffectiveDateBegin <= _toEffectiveDateBegin))
                                   && (
                                        (_fromEffectiveDateEnd == null && _toEffectiveDateEnd == null) ||
                                        (_fromEffectiveDateEnd == null && _toEffectiveDateEnd != null && x.EffectiveDateEnd <= _toEffectiveDateEnd) ||
                                        (_toEffectiveDateEnd == null && _fromEffectiveDateEnd != null && x.EffectiveDateEnd >= _fromEffectiveDateEnd) ||
                                        (_fromEffectiveDateEnd != null && _toEffectiveDateEnd != null && x.EffectiveDateEnd >= _fromEffectiveDateEnd && x.EffectiveDateEnd <= _toEffectiveDateEnd))
                                    && (
                                       expirycoupon == EXPIRY_COUPON.NOT_MAP || expirycoupon == null ? true :
                                       expirycoupon == EXPIRY_COUPON.EXPIRED ?
                                        x.EffectiveDateEnd <= DateTime.Today :
                                        x.EffectiveDateEnd >= DateTime.Today || x.EffectiveDateEnd== null));

            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "type":
                        query = IsAscOrder ? query.OrderBy(x => x.CouponType) : query.OrderByDescending(x => x.CouponType);
                        break;
                    case "couponname":
                        query = IsAscOrder ? query.OrderBy(x => x.CouponName) : query.OrderByDescending(x => x.CouponName);
                        break;
                    case "couponcode":
                        query = IsAscOrder ? query.OrderBy(x => x.CouponCode) : query.OrderByDescending(x => x.CouponCode);
                        break;
                    case "effectivedatebegin":
                        query = IsAscOrder ? query.OrderBy(x => x.EffectiveDateBegin) : query.OrderByDescending(x => x.EffectiveDateBegin);
                        break;
                    case "effectivedateend":
                        query = IsAscOrder ? query.OrderBy(x => x.EffectiveDateEnd) : query.OrderByDescending(x => x.EffectiveDateEnd);
                        break;
                    case "channel":
                        query = IsAscOrder ? query.OrderBy(x => x.Channel.ChannelName) : query.OrderByDescending(x => x.Channel.ChannelName);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<CouponViewModel> data = new List<CouponViewModel>();
            foreach (var item in datas)
            {
                CouponViewModel coupon = new CouponViewModel();
                coupon.Id = item.Id;
                coupon.CouponCode = item.CouponCode;
                coupon.CouponName = item.CouponName;
                if (item.CouponCategory != null) coupon.CouponCategoryName = item.CouponCategory.CouponName;
                if (item.ChannelId != null) coupon.ChannelName = item.Channel.ChannelName;
                if (item.StoreId != null) coupon.StoreName = item.Store.StoreName;
                coupon.CashAmount = item.Amount;
                coupon.Remark = item.Remark;
                coupon.Active = item.Active;
                coupon.TermAndConditions = item.TermAndConditions;
                coupon.ValidityPeriodType = item.ValidityPeriodType;
                coupon.CouponType = item.CouponType;
                coupon.AfterEffectiveDay = item.AfterEffectiveDay;
                coupon.AfterReceptionDay = item.AfterReceptionDay;
                coupon.EffectualDateBegin = string.Format("{0:dd/MM/yyyy}", item.EffectiveDateBegin);//
                coupon.EffectualDateEnd = string.Format("{0:dd/MM/yyyy}", item.EffectiveDateEnd);

                data.Add(coupon);
            }
            return data;
        }

        public List<CouponViewModel> Get()
        {
            _logger.LogDebug($"Get all product category");
            var occupations = UnitOfWork.CouponRepo.GetAll().Where(x => x.Active).Select(x => new CouponViewModel()
            {
                Id = x.Id,
                CouponName = x.CouponName
            }).OrderBy(x => x.CouponName).ToList();

            return occupations;
        }

        public CouponViewModel Get(Guid id)
        {
            _logger.LogDebug($"Detail service (Id: {id})");
            var entity = UnitOfWork.CouponRepo.GetById(id);
            if (entity != null)
            {
                var model = new CouponViewModel()
                {
                    Id = entity.Id,
                    CouponCode = entity.CouponCode,
                    CouponName = entity.CouponName,
                    CouponCategoryId = entity.CouponCategoryId,
                    ChannelId = entity.ChannelId,
                    StoreId = entity.StoreId,

                    CouponType = entity.CouponType,
                    CashAmount = entity.Amount,
                    Discount = entity.Discount,
                    ValidityPeriodType = entity.ValidityPeriodType,

                    EffectiveDateBegin = entity.EffectiveDateBegin,
                    EffectiveDateEnd = entity.EffectiveDateEnd,
                    Active = entity.Active,
                    TermAndConditions = entity.TermAndConditions,
                    Remark = entity.Remark,

                    AfterEffectiveDay = entity.AfterEffectiveDay,
                    AfterReceptionDay = entity.AfterReceptionDay,
                    MinimumPurchase = entity.MinimumPurchase,
                };
                if (entity.ChannelId == null)
                { model.ChannelName = ""; model.ChannelId = 0; }
                else { model.ChannelName = entity.Channel.ChannelName; }

                return model;
            }
            return null;
        }
        public void Create(CouponViewModel model)
        {


            _logger.LogDebug($"Creating Coupon (Code: {model.CouponCode}. Name: {model.CouponName})");
            Coupon entity = new Coupon();
            entity.Id = Guid.NewGuid();
            entity.CouponCode = model.CouponCode;
            entity.CouponName = model.CouponName;
            entity.CouponCategoryId = model.CouponCategoryId;
            entity.CouponType = model.CouponType;
            entity.ChannelId = model.ChannelId;
            entity.StoreId = model.StoreId;
            entity.MinimumPurchase = model.MinimumPurchase;
            entity.TermAndConditions = model.TermAndConditions;
            entity.Remark = model.TermAndConditions;
            entity.Active = true;

            if (model.CouponType == true)
            {
                entity.Amount = model.CashAmount;
            }
            else
            {
                entity.Discount = model.Discount;
            }

            if (model.ValidityPeriodType == false)
            {
                if (model.EffectiveDateBegin == null)
                {
                    entity.EffectiveDateBegin = DateTime.Now;
                }
                entity.ValidityPeriodType = false;
                entity.EffectiveDateBegin = model.EffectiveDateBegin;
                entity.EffectiveDateEnd = model.EffectiveDateEnd;

            }
            else
            {
                entity.ValidityPeriodType = true;
                if (model.EffectiveDateBegin == null)
                {
                    entity.EffectiveDateBegin = DateTime.Now;
                }
                entity.AfterEffectiveDay = model.AfterEffectiveDay;
                entity.AfterReceptionDay = model.AfterReceptionDay;
            }

            if (model.CouponCodeAutomatically)
            {
                entity.CouponCode = this.GenerateMemberCode();
                _logger.LogDebug($"Creating Coupon (CodeAuto: {model.CouponCode})");
            }

            UnitOfWork.CouponRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void Update(CouponViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.CouponRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Id = model.Id;
                entity.CouponCode = model.CouponCode;
                entity.CouponName = model.CouponName;
                entity.CouponCategoryId = model.CouponCategoryId;
                entity.ChannelId = model.ChannelId;
                entity.StoreId = model.StoreId;
                entity.Remark = model.Remark;
                entity.Active = true;
                entity.TermAndConditions = model.TermAndConditions;
                entity.ValidityPeriodType = model.ValidityPeriodType;
                entity.CouponType = model.CouponType;
                entity.TermAndConditions = model.TermAndConditions;
                entity.Remark = model.Remark;
                entity.MinimumPurchase = model.MinimumPurchase;
                if (model.CouponType == true)
                {
                    entity.CouponType = true;
                    entity.Amount = model.CashAmount;
                    entity.Discount = 0;
                }
                else
                {
                    entity.CouponType = false;
                    entity.Discount = model.Discount;
                    entity.Amount = 0;
                }

                if (model.ValidityPeriodType == false)
                {
                    entity.ValidityPeriodType = false;
                    entity.EffectiveDateBegin = model.EffectiveDateBegin;
                    entity.EffectiveDateEnd = model.EffectiveDateEnd;
                    entity.AfterEffectiveDay = 0;
                    entity.AfterReceptionDay = 0;

                }
                else
                {
                    entity.ValidityPeriodType = true;
                    entity.AfterEffectiveDay = model.AfterEffectiveDay;
                    entity.AfterReceptionDay = model.AfterReceptionDay;
                    entity.EffectiveDateBegin = null;
                    entity.EffectiveDateEnd = null;
                }
                UnitOfWork.CouponRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void Delete(Guid id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.CouponRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.CouponRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<CouponCategory> GetCouponCategories()
        {
            _logger.LogDebug($"Get Coupon Categories Enter.");
            var couponCategories = UnitOfWork.CouponCategoryRepo.GetAll().Where(x => x.Active).Select(x => new CouponCategory()
            {
                Id = x.Id,
                CouponName = x.CouponName
            }).OrderBy(x => x.CouponName).ToList();

            return couponCategories;
        }

        public List<Coupon> GetCoupon()
        {
            _logger.LogDebug($"Get Coupon Enter.");
            var coupon = UnitOfWork.CouponRepo.GetAll().Where(x => x.Active).Select(x => new Coupon()
            {
                Id = x.Id,
                CouponName = x.CouponName
            }).OrderBy(x => x.CouponName).ToList();

            return coupon;
        }

        private string GenerateMemberCode()
        {
            var generator = new RandomGenerator();
            return generator.RandomCode();
        }

        public List<CouponViewModel> Reset(string searchText, string orderField, bool IsAscOrder, bool? couponType, int? channelId, string fromEffectiveDateBegin, string toEffectiveDateBegin, string fromEffectiveDateEnd, string toEffectiveDateEnd, EXPIRY_COUPON? expiry_coupon, int page, int size, out int total)
        {
            throw new NotImplementedException();
        }
    }
}
