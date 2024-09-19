using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Voucher
{
   public class VoucherSearchView
    {
        public string VoucherCode { get; set; }
        public string VoucherName { get; set; }
        public int? VoucherCategoryId { get; set; }
        public string VoucherCategoryName { get; set; }
        public Boolean Active { get; set; }
        public string Desc { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public int AvailableStock { get; set; }
        public int? ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string TemplateBanner { get; set; }
        public double FaceValue { get; set; }
        public double Price { get; set; }

        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date

        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }
        public int MinimumPurchase { get; set; }

        public string TermAndConditions { get; set; }

        public bool VoucherType { get; set; } // 0 discount, 1 cash
    }
}
