using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
    public class LoyaltyPointTypeSearchViewModel : BaseSearchViewModel
    {
        public List<LoyaltyPointTypeViewModel> LoyaltyPointTypes { get; set; }
        public LoyaltyPointTypeViewModel LoyaltyPointType { get; set; }
    }
}
