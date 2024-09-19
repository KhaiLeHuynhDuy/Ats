using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleBrandViewModel
    {
        public Guid Id { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandCode { get; set; }
        public string Desc { get; set; }
        public Guid? LoyaltyPointRuleId { get; set; }
        public string LoyaltyPointRuleName { get; set; }

        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool Valid { get; set; }
        public double PointRedemption { get; set; }
        public double EarningRate { get; set; }

        public string Remark { get; set; }
    }
}
