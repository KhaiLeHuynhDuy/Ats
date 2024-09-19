using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Gift.Models
{  
    [Table("gifts")]
    public class Gift : AuditBase, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public int? GiftCategoryId { get; set; }
        [ForeignKey("GiftCategoryId")]
        public virtual GiftCategory GiftCategory { get; set; }
        public Boolean Active { get; set; } = true;
        public string Desc { get; set; }//TermAndConditions

        //// Add new column
        public int AvailableStock { get; set; }
        public int NumberRedeemed { get; set; }
        public int StockAdjustment { get; set; }
        public double RetailMarketPrice { get; set; }
        public double PurchasePrice { get; set; }
        public string Specification { get; set; }
        public string Image { get; set; }

        //// Add new column
        public int? ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Ats.Domain.Channel.Models.MemberChannel Channel { get; set; }

        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date
        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }
        public string TermAndConditions { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }
        public bool GiftType { get; set; } // 0 discount, 1 cash
        public double Amount { get; set; }
        public double Discount { get; set; }
        public int TotalMember { get; set; }



    }
}
