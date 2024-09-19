using Ats.Domain;
using Ats.Domain.Coupon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Models.Coupon
{
    public class CouponViewModel
    {
        public Guid Id { get; set; }
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public bool CouponCodeAutomatically { get; set; }
        public int? CouponCategoryId { get; set; }
        public string CouponCategoryName { get; set; }
        public Boolean Active { get; set; }
        public virtual CouponCategory CouponCategory { get; set; }
        public string Remark { get; set; }

        public int? ChannelId { get; set; }
        public string ChannelName { get; set; }
        public virtual Ats.Domain.Channel.Models.MemberChannel Channel { get; set; }

        public int? StoreId { get; set; }
        public string StoreName { get; set; }
        public virtual Ats.Domain.Store.Models.Store Store { get; set; }


        public double CashAmount { get; set; }
        public double Discount { get; set; }
        public string TermAndConditions { get; set; }
        public bool ValidityPeriodType { get; set; }


        public bool CouponType { get; set; } // 0 discount, 1 cash
        public int MinimumPurchase { get; set; }
        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; } 

        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }
        public string? EffectualDateBegin { get; set; }
        public string? EffectualDateEnd { get; set; }
        public EXPIRY_COUPON? Expiry_Coupon { get; set; }

    }
}
