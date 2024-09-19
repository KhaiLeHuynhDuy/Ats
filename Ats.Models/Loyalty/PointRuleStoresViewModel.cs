using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleStoresViewModel
    {

        public Guid Id { get; set; }

        public int StoreId { get; set; }      
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public Guid? LoyaltyPointRuleId { get; set; }   
        public string LoyaltyPointRuleName { get; set; }   

        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public string? EffectiveDateBegin { get; set; }
        public string? EffectiveDateEnd { get; set; }

        public bool Valid { get; set; }
        public double PointRedemption { get; set; }
        public double EarningRate { get; set; }

        public string Remark { get; set; }
    }
}
