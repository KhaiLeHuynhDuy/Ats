using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Campaign
{
    public class ScheduledViewModel
    {
        public Guid Id { get; set; }
        public string CampaignName { get; set; }
        public DateTime? StartDate { get; set; }//MemberCoupons
        public string StartDateString { get; set; }//MemberCoupons
        public string ChannelName { get; set; }
        public bool Active { get; set; }
        public int? TotalMember { get; set; }
       
    }
}
