﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Gift
{
   public class GiftSearchView
    {
        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public int? GiftCategoryId { get; set; }
        public int? ChannelId { get; set; }
        public Boolean Active { get; set; } = true;
        public string Desc { get; set; }

        public int AvailableStock { get; set; }
        public int NumberRedeemed { get; set; }
        public int StockAdjustment { get; set; }
        public double RetailMarketPrice { get; set; }
        public double PurchasePrice { get; set; }
        public string Specification { get; set; }
        public string Image { get; set; }
   
        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date
        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }
        public int TotalMember { get; set; }

        public string? FromEffectiveDateBegin { get; set; }
        public string? ToEffectiveDateBegin { get; set; }
        public string? FromEffectiveDateEnd { get; set; }
        public string? ToEffectiveDateEnd { get; set; }

        public bool GiftType { get; set; } // 0 discount, 1 cash
        public double Amount { get; set; }
        public double Discount { get; set; }
    }
}
