using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleChannelViewModel
    {
        public Guid Id { get; set; }
        public int? ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string Desc { get; set; }

        public Guid? LoyaltyPointRuleId { get; set; }  
        public string LoyaltyPointRuleName { get; set; }


        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public string EffectiveFroms { get; set; }
        public string  EffectiveTos { get; set; }

        public bool Valid { get; set; }
        public double PointRedemption { get; set; }
        public double EarningRate { get; set; }

        public string Remark { get; set; }
    }
}
