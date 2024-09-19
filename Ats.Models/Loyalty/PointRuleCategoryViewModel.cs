using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleCategoryViewModel
    {
        public Guid Id { get; set; }
        public int? ProductCateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public string ProductCategoriesName { get; set; }
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
