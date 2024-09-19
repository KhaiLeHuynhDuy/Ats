using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.LoyaltyTier
{
    public class LoyaltyTierViewModel
    {
        public int Id { get; set; }
        public string TierName { get; set; }
        public int TierLevel { get; set; }
        public Boolean Active { get; set; }
        public double UpgradePoint { get; set; }
        public double DowngradePoint { get; set; }
        public double PointMin { get; set; }
        public double PointMax { get; set; }
        public string Desc { get; set; }
    }
}
