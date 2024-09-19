using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Store;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Ats.Models.Reward;
using Microsoft.EntityFrameworkCore;
using Ats.Domain.Member.Models;
using Ats.Commons.Resource;
using Ats.Models.Member;
using Ats.Commons.Utilities;

namespace Ats.Services.Implementation
{
    public class RewardsService : BaseService, IRewardsService
    {
        private IConfiguration _config;
        private int pageSize;

        public RewardsService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public RedeemPointDetailViewModel GetRedeemPointDetail(Guid memberId)
        {
            _logger.LogDebug($"Get Redeem Point Detail By Member (Id: {memberId})");
            var entity = UnitOfWork.MemberRepo.GetById(memberId);
            try
            {
                if (entity == null)
                {
                    _logger.LogDebug($" Redeem Point Detail By Member (Id: {memberId}) Not Found.");
                    throw new NullReferenceException($"Lead {memberId} Not Found.");
                }
                RedeemPointDetailViewModel model = new RedeemPointDetailViewModel();
                model.MemberId = entity.Id;
                model.MemberCode = entity.MemberCode;
                model.FirstName = entity.FirstName;
                model.LastName = entity.LastName;
                model.Email = entity.Email;
                model.PhoneNumber = entity.PhoneNumber;
                model.AvailablePoints = UnitOfWork.MemberWalletRepo.GetAll().FirstOrDefault(p => p.MemberId == entity.Id).Point;
                model.TierName = entity.MemberLoyaltyTiers != null ? UnitOfWork.MemberLoyaltyTierRepo.GetAll().FirstOrDefault(z => z.MemberId == entity.Id).LoyaltyTier.TierName : "";
                model.RedeemedRate = UnitOfWork.LoyaltyPointSettingRepo.GetAll().FirstOrDefault(x => x.Active)?.RedeemedRate ?? 0;

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get Member exception : {ex})");
                throw ex;
            }
        }

        public void CreateRedeemPoint(RedeemPointDetailViewModel model)
        {
            try
            {
                if (!VerificationCode(model.VerificationCode)) return;  
                model.InvoiceNo = model.InvoiceNo.ToLower().Trim();
                var transByInvoiceNo = UnitOfWork.MemberLoyaltyTransactionRepo.GetAll().FirstOrDefault(m => m.InvoiceNo.ToLower().Equals(model.InvoiceNo));
                if (transByInvoiceNo != null)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_redeemPointInvoiceNoUsed);
                }

                var memberWallet = UnitOfWork.MemberWalletRepo.GetAll().FirstOrDefault(x => x.MemberId.Equals(model.MemberId) && x.Active);
                if (memberWallet == null)
                {
                    throw new NullReferenceException(Resource.Common_errorMessage_redeemPointWalletNotFound);
                }

                if (memberWallet.Point < model.PointRedemption)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_redeemPointNotEnoughPoints);
                }

                var rate = UnitOfWork.LoyaltyPointSettingRepo.GetAll().FirstOrDefault(x => x.Active);
                if (rate == null)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_redeemPointRedeemedRateNotFound);
                }

                var loyaltyPointType = UnitOfWork.LoyaltyPointTypeRepo.GetAll().FirstOrDefault(x => x.Active && x.Name.Equals(LoyaltyPointTypeNames.RedemptionPoints));

                memberWallet.Point = memberWallet.Point - model.PointRedemption.Value;
                var memberLoyaltyTrans = new MemberLoyaltyTransaction
                {
                    InvoiceNo = model.InvoiceNo,
                    Rate = rate.RedeemedRate,
                    Point = model.PointRedemption.Value,
                    Amount = model.PointRedemption.Value * rate.RedeemedRate,
                    Remark = model.Remark,
                    WalletId = memberWallet.Id,
                    TransactionDate = DateTime.Now,
                    PointTypeId = loyaltyPointType.Id,
                    TransactionType = false, //???
                };
                UnitOfWork.MemberLoyaltyTransactionRepo.Insert(memberLoyaltyTrans);
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create Redeem Point exception : {ex})");
                throw ex;
            }
        }

        public double GetRedeemedDiscount(double point)
        {
            var redeemedRate = UnitOfWork.LoyaltyPointSettingRepo.GetAll().FirstOrDefault(x => x.Active)?.RedeemedRate ?? 0;
            return point * redeemedRate;
        }

        public List<RedemptionItemViewModel> GetListRedemption(SearchInfoRedemptionViewModel searchInfo, out int total)
        {
            var giftsQuery = UnitOfWork.MemberGiftRepo.GetAll().Select(x => new RedemptionItemViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Gift.GiftName,
                Amount = x.Gift.Amount,
                Discount = x.Gift.Discount,               
                EffectiveFrom = x.EffectiveFrom,
                EffectiveTo = x.EffectiveTo,
                RedeemedDate = x.RedeemedDate,
                IsExpired = x.EffectiveTo < DateTime.Now,
                Type = GIFT_VOUCHER_COUPON.GIFT,
                TypeName = GIFT_VOUCHER_COUPON.GIFT.ToName().ToString(),
                Member = new MemberViewModel
                {
                    Id = x.MemberId.Value,
                    MemberCode = x.Member.MemberCode,
                    FirstName = x.Member.FirstName,
                    LastName = x.Member.LastName,
                    PhoneNumber = x.Member.PhoneNumber,
                }
            });
            var vouchersQuery = UnitOfWork.MemberVoucherRepo.GetAll().Select(x => new RedemptionItemViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Voucher.VoucherName,
                Amount = x.Voucher.Amount,
                Discount = x.Voucher.Discount,
                EffectiveFrom = x.EffectiveFrom,
                EffectiveTo = x.EffectiveTo,
                RedeemedDate = x.RedeemedDate,
                IsExpired = x.EffectiveTo < DateTime.Now,
                Type = GIFT_VOUCHER_COUPON.VOUCHER,
                TypeName = GIFT_VOUCHER_COUPON.VOUCHER.ToName().ToString(),
                Member = new MemberViewModel
                {
                    Id = x.MemberId.Value,
                    MemberCode = x.Member.MemberCode,
                    FirstName = x.Member.FirstName,
                    LastName = x.Member.LastName,
                    PhoneNumber = x.Member.PhoneNumber,
                }
            });

            var couponQuery = UnitOfWork.MemberCouponRepo.GetAll().Select(x => new RedemptionItemViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Coupon.CouponName,
                Amount = x.Coupon.Amount,
                Discount = x.Coupon.Discount,
                EffectiveFrom = x.EffectiveFrom,
                EffectiveTo = x.EffectiveTo,
                RedeemedDate = x.RedeemedDate,
                IsExpired = x.EffectiveTo < DateTime.Now,
                Type = GIFT_VOUCHER_COUPON.COUPON,
                TypeName = GIFT_VOUCHER_COUPON.COUPON.ToName().ToString(),
                Member = new MemberViewModel
                {
                    Id = x.MemberId.Value,
                    MemberCode = x.Member.MemberCode,
                    FirstName = x.Member.FirstName,
                    LastName = x.Member.LastName,
                    PhoneNumber = x.Member.PhoneNumber,
                }
            });
            //(gender == null ? true : x.Gender == gender)
            var query = giftsQuery.Union(vouchersQuery).Union(couponQuery);
             var ss = query.Where(x => x.IsExpired != true).ToList();
            if (searchInfo.Type.HasValue)
            {
                query = query.Where(x => x.Type == searchInfo.Type.Value);
            }
            // IsExpireds.HasValue = false : expired , true : active
            if (searchInfo.IsExpireds != null)
            {
                if(searchInfo.IsExpireds.Value == true)
                { query = query.Where(x => x.IsExpired == true); }    
                else
                {
                    query = query.Where(x => x.IsExpired != true);
                }  
            }
            if (searchInfo.IsRedeemd != null)
            {
                if (searchInfo.IsRedeemd == true)
                { query = query.Where(x => x.RedeemedDate.HasValue); }
                else
                {
                    query = query.Where(x => !x.RedeemedDate.HasValue && x.IsExpired != true);
                }
            }
            if (!string.IsNullOrEmpty(searchInfo.Keyword))
            {
                searchInfo.Keyword = searchInfo.Keyword.ToLower().Trim();
                query = query.Where(x => x.Code.ToLower().Contains(searchInfo.Keyword) || x.Name.ToLower().Contains(searchInfo.Keyword));
            }
            if (!string.IsNullOrEmpty(searchInfo.MemberCode))
            {
                searchInfo.MemberCode = searchInfo.MemberCode.ToLower().Trim();
                query = query.Where(x => x.Member.MemberCode.ToLower().Contains(searchInfo.MemberCode));
            }
            if (!string.IsNullOrEmpty(searchInfo.MemberPhoneNumber))
            {
                query = query.Where(x => x.Member.PhoneNumber.Contains(searchInfo.MemberPhoneNumber));
            }

            total = query.Count();
            //var temp = query.ToList();
            
            if (!String.IsNullOrEmpty(searchInfo.Pager.OrderField))
            {
                switch (searchInfo.Pager.OrderField.ToLower())
                {
                    case "membercode":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.Member.MemberCode) : query.OrderByDescending(x => x.Member.MemberCode);
                        break;
                    case "firstname":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.Member.FirstName) : query.OrderByDescending(x => x.Member.FirstName);
                        break;
                    case "lastname":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.Member.LastName) : query.OrderByDescending(x => x.Member.LastName);
                        break;
                    case "phonenumber":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.Member.PhoneNumber) : query.OrderByDescending(x => x.Member.PhoneNumber);
                        break;
                    case "code":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.Code) : query.OrderByDescending(x => x.Code);
                        break;
                    case "name":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                    case "typename":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.TypeName) : query.OrderByDescending(x => x.TypeName);
                        break;
                    case "effectivefrom":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.EffectiveFrom) : query.OrderByDescending(x => x.EffectiveFrom);
                        break;
                    case "redeemeddate":
                        query = searchInfo.Pager.OrderSort ? query.OrderBy(x => x.RedeemedDate) : query.OrderByDescending(x => x.RedeemedDate);
                        break;
                }
            }

            var redemptionItems = query.Skip((searchInfo.Pager.Page - 1) * searchInfo.Pager.Size).Take(searchInfo.Pager.Size).ToList();

            return redemptionItems;
        }

        public RedemptionItemViewModel GetRedemptionDetail(Guid id, GIFT_VOUCHER_COUPON type)
        {
            RedemptionItemViewModel data = null;
            switch (type)
            {
                case GIFT_VOUCHER_COUPON.GIFT:
                    data = UnitOfWork.MemberGiftRepo.GetAll().Select(x => new RedemptionItemViewModel
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Gift.GiftName,
                        Amount = x.Gift.Amount,
                        Discount = x.Gift.Discount,
                        EffectiveTo = x.EffectiveTo,
                        EffectiveFrom = x.EffectiveFrom,
                        RedeemedDate = x.RedeemedDate,
                        Remark = x.Remark,
                        RedeemedCode = x.RedeemedCode,
                        IsExpired = x.EffectiveTo < DateTime.Now,
                        Type = GIFT_VOUCHER_COUPON.GIFT,
                        TypeName = GIFT_VOUCHER_COUPON.GIFT.ToName().ToString(),
                        Member = new MemberViewModel
                        {
                            Id = x.MemberId.Value,
                            MemberCode = x.Member.MemberCode,
                            FirstName = x.Member.FirstName,
                            LastName = x.Member.LastName,
                            PhoneNumber = x.Member.PhoneNumber,
                            Email = x.Member.Email,
                            TierName = x.Member.MemberLoyaltyTiers != null ? UnitOfWork.MemberLoyaltyTierRepo.GetAll().FirstOrDefault(z => z.MemberId == x.MemberId).LoyaltyTier.TierName : ""
            }
                    }).FirstOrDefault(x => x.Id.Equals(id));
                    break;

                case GIFT_VOUCHER_COUPON.VOUCHER:
                    data = UnitOfWork.MemberVoucherRepo.GetAll().Select(x => new RedemptionItemViewModel
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Voucher.VoucherName,
                        Amount = x.Voucher.Amount,
                        Discount = x.Voucher.Discount,
                        EffectiveFrom = x.EffectiveFrom,
                        EffectiveTo = x.EffectiveTo,
                        RedeemedDate = x.RedeemedDate,
                        Remark = x.Remark,
                        RedeemedCode = x.RedeemedCode,
                        IsExpired = x.EffectiveTo < DateTime.Now,
                        Type = GIFT_VOUCHER_COUPON.VOUCHER,
                        TypeName = GIFT_VOUCHER_COUPON.VOUCHER.ToName().ToString(),
                        Member = new MemberViewModel
                        {
                            Id = x.MemberId.Value,
                            MemberCode = x.Member.MemberCode,
                            FirstName = x.Member.FirstName,
                            LastName = x.Member.LastName,
                            PhoneNumber = x.Member.PhoneNumber,
                            Email = x.Member.Email,
                            TierName = x.Member.MemberLoyaltyTiers != null ? UnitOfWork.MemberLoyaltyTierRepo.GetAll().FirstOrDefault(z => z.MemberId == x.MemberId).LoyaltyTier.TierName : ""
                        }
                    }).FirstOrDefault(x => x.Id.Equals(id));
                    break;

                case GIFT_VOUCHER_COUPON.COUPON:
                    data = UnitOfWork.MemberCouponRepo.GetAll().Select(x => new RedemptionItemViewModel
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Coupon.CouponName,
                        Amount = x.Coupon.Amount,
                        Discount = x.Coupon.Discount,
                        EffectiveFrom = x.EffectiveFrom,
                        EffectiveTo = x.EffectiveTo,
                        IsExpired = x.EffectiveTo < DateTime.Now,
                        RedeemedDate = x.RedeemedDate,
                        Remark = x.Remark,
                        RedeemedCode = x.RedeemedCode,
                        Type = GIFT_VOUCHER_COUPON.COUPON,
                        TypeName = GIFT_VOUCHER_COUPON.COUPON.ToName().ToString(),
                        Member = new MemberViewModel
                        {
                            Id = x.MemberId.Value,
                            MemberCode = x.Member.MemberCode,
                            FirstName = x.Member.FirstName,
                            LastName = x.Member.LastName,
                            PhoneNumber = x.Member.PhoneNumber,
                            Email = x.Member.Email,
                            TierName = x.Member.MemberLoyaltyTiers != null ? UnitOfWork.MemberLoyaltyTierRepo.GetAll().FirstOrDefault(z => z.MemberId == x.MemberId).LoyaltyTier.TierName : ""
                        }
                    }).FirstOrDefault(x => x.Id.Equals(id));
                    break;
            }

            return data;
        }

        public void RedemptionRedeem(RedemptionItemViewModel model)
        {
            if (model.Type == GIFT_VOUCHER_COUPON.GIFT)
            {
                var memberGift = UnitOfWork.MemberGiftRepo.GetById(model.Id);
                if (memberGift == null)
                {
                    throw new NullReferenceException($"Redemption {model.Id} Not Found.");
                }
                if (memberGift.RedeemedDate.HasValue)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_GiftRedeemed);
                }
                if (memberGift.EffectiveTo < DateTime.Now)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_GiftExpired);
                }

                memberGift.Remark = model.Remark;
                memberGift.RedeemedDate = DateTime.Now;
                memberGift.RedeemedCode = model.VerificationCode;
                memberGift.Remark = model.Remark;
            }

            if (model.Type == GIFT_VOUCHER_COUPON.VOUCHER)
            {
                var memberVoucher = UnitOfWork.MemberVoucherRepo.GetById(model.Id);
                if (memberVoucher == null)
                {
                    throw new NullReferenceException($"Redemption {model.Id} Not Found.");
                }
                if (memberVoucher.RedeemedDate.HasValue)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_VoucherRedeemed);
                }
                if (memberVoucher.EffectiveTo < DateTime.Now)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_VoucherExpired);
                }

                memberVoucher.Remark = model.Remark;
                memberVoucher.RedeemedDate = DateTime.Now;
                memberVoucher.RedeemedCode = model.VerificationCode;
                memberVoucher.Remark = model.Remark;
            }

            if (model.Type == GIFT_VOUCHER_COUPON.COUPON)
            {
                var memberCoupon = UnitOfWork.MemberCouponRepo.GetById(model.Id);
                if (memberCoupon == null)
                {
                    throw new NullReferenceException($"Redemption {model.Id} Not Found.");
                }
                if (memberCoupon.RedeemedDate.HasValue)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_CouponRedeemed);
                }
                if (memberCoupon.EffectiveTo < DateTime.Now)
                {
                    throw new ApplicationException(Resource.Common_errorMessage_CouponExpired);
                }

                memberCoupon.Remark = model.Remark;
                memberCoupon.RedeemedDate = DateTime.Now;
                memberCoupon.RedeemedCode = model.VerificationCode;
                memberCoupon.Remark = model.Remark;
            }

            UnitOfWork.SaveChanges();
        }

        private bool VerificationCode(string code)
        {
            //Todo verify later
            return true;
        }
    }
}
