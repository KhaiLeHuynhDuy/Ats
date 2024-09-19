using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.LoyaltyTier
{
    public class LoyaltyTierSearchViewModel : BaseSearchViewModel
    {
        public List<LoyaltyTierViewModel> LoyaltyTiers { get; set; }
        public LoyaltyTierViewModel LoyaltyTier { get; set; }
    }
}
