using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Campaign
{
    public class CampaignViewModel
    {
        public Guid Id { get; set; }
        public Guid? CouponId { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? GiftId { get; set; }

        public string CouponName { get; set; }
        public string CampaignName { get; set; }
        public DateTime? StartDate { get; set; }//send date
        public DateTime? CreateDate { get; set; } // order for dashboard
        public string StartDateString { get; set; }
        public DateTime SendtDate { get; set; }//MemberCoupons
       

   

        public Guid? MemberRefTag { get; set; }
        public Guid? MemberRefGroup { get; set; }
        public Guid? MemberRefSegment { get; set; }
  
     
        public int? Tier { get; set; }
        public int? ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string MemberName { get; set; }
        public string Channel { get; set; } 
        public double BonusPoint { get; set; }
        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }

        public bool Active { get; set; }
        public int? TotalMember { get; set; }
       
        public bool IsPublished { get; set; } 
        public string TermAndConditions { get; set; }
        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date
        public string Remark { get; set; }
        public int? total { get; set; }
    }
}
