using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Models.Member
{
    public  class GiftVoucherCouponViewModel :BaseSearchViewModel
    {
        public Guid Id { get; set; }
        public Guid? MemberId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public string Type { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool TypeAmountDiscount { get; set; }

        public string EffectiveFromString { get; set; }
        public string EffectiveToString { get; set; }
    }
    public enum GIFT_VOUCHER_COUPON
    {
       
        [Display(Name = "Gift")]
        GIFT = 1,
        [Display(Name = "Voucher")]
        VOUCHER = 2,
        [Display(Name = "Coupon")]
        COUPON = 3,
    }
}
