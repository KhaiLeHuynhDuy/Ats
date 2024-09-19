using Ats.Models.Member;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reward
{
    public class SearchRedemptionViewModel
    {
        public List<RedemptionItemViewModel> RedemptionItems { get; set; }
        public SearchInfoRedemptionViewModel SearchInfo { get; set; }
    }

    public class SearchInfoRedemptionViewModel : BaseSearchViewModel
    {      
        public int Page { get; set;  }
        public int Size { get; set; }
        public string Search { get; set; }

        public bool? IsExpireds { get; set; }
        public bool? IsRedeemd { get; set; }
        public string MemberCode { get; set; }
        public string MemberPhoneNumber { get; set; }
        public GIFT_VOUCHER_COUPON? Type { get; set; }
    }

    public class RedemptionItemViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public GIFT_VOUCHER_COUPON Type { get; set; }
        public string TypeName { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? RedeemedDate { get; set; }
        public string RedeemedCode { get; set; }
        public bool TypeAmountDiscount { get; set; }
        public MemberViewModel Member { get; set; }
        public bool IsExpired { get; set; }
        public string VerificationCode { get; set; }
        public string Remark { get; set; }
    }
}
