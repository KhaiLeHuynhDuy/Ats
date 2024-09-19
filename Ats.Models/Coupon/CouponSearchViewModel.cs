using Ats.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Coupon
{
    public class CouponSearchViewModel : BaseSearchViewModel
    {
        public List<CouponViewModel> Coupons { get; set; }
        public CouponViewModel Coupon { get; set; }

        public int? ChannelID { get; set; }
        public bool? couponType { get; set; }
        public string? fromEffectiveDateBegin { get; set; }
        public string? toEffectiveDateBegin { get; set; }
        public string? fromEffectiveDateEnd { get; set; }
        public string? toEffectiveDateEnd { get; set; }
        public EXPIRY_COUPON? expiryCoupon { get; set; }

    }
}
