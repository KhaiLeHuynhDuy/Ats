using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Gift
{
    public class GiftSearchViewModel : BaseSearchViewModel
    {
        public List<GiftViewModel> Gifts { get; set; }
        public GiftViewModel Gift { get; set; }

        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public int? GiftCategoryId { get; set; }
        public string GiftCategoryName { get; set; }
        public Boolean Active { get; set; } = true;
        public string Desc { get; set; }

        public int AvailableStock { get; set; }
        public int NumberRedeemed { get; set; }
        public int StockAdjustment { get; set; }
        public double RetailMarketPrice { get; set; }
        public double PurchasePrice { get; set; }
        public string Specification { get; set; }
        public string Image { get; set; }

        public int? ChannelId { get; set; }
        public string ChannelName { get; set; }


        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date
        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }

        public int? StockFrom { get; set; }
        public int? StockTo { get; set; }
        public int TotalMember { get; set; }

        public bool? GiftType { get; set; } // 0 discount, 1 cash
        public double Amount { get; set; }
        public double Discount { get; set; }

        public string TermAndConditions { get; set; }
    }
}
