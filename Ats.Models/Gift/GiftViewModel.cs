using Ats.Domain.Gift.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Gift
{
    public class GiftViewModel
    {
        public Guid Id { get; set; }
        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public int? GiftCategoryId { get; set; }
        public string GiftCategoryName { get; set; }
   
        public Boolean Active { get; set; }
        public string Desc { get; set; }


        //// Add new column
        public int AvailableStock { get; set; }
        public int NumberRedeemed { get; set; }
        public int StockAdjustment { get; set; }
        public double RetailMarketPrice { get; set; }
        public double PurchasePrice { get; set; }
        public string Specification { get; set; }
        public string uniqueFileName { get; set; }

        // new CodeAutomatically
        public bool GiftCodeAutomatically { get; set; }

        //// Add new column
        public int? ChannelId { get; set; }
        public string ChannelName { get; set; }

        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date
        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public string? EffectiveDateBegins { get; set; }
        public string? EffectiveDateEnds { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }

        public int TotalMember { get; set; }

        public bool GiftType { get; set; } // 0 discount, 1 cash
        public double Amount { get; set; }
        public double Discount { get; set; }

        public string TermAndConditions { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
