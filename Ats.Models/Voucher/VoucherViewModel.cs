using Ats.Domain.Voucher.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Voucher
{
    public class VoucherViewModel
    {
        public Guid Id { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherCodeAutomatically { get; set; }
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
        public string uniqueFileName { get; set; }
        public double FaceValue { get; set; }
        public double Price { get; set; }

        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date

        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }
        public string EffectualDateBegin { get; set; }
        public string EffectualDateEnd { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }  
        public int MinimumPurchase { get; set; }

        public VOUCHER_TEMPLATE Templates { get; set; }

        public string TermAndConditions { get; set; }

        public bool VoucherType { get; set; } // 0 discount, 1 cash

        public IFormFile ProfileImage { get; set; }
    }
}
