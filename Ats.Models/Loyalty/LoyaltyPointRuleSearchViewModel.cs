using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
    public class LoyaltyPointRuleSearchViewModel : BaseSearchViewModel
    {

        public List<LoyaltyPointRulesViewModel> LoyaltyPointRules { get; set; }
        public LoyaltyPointRulesViewModel LoyaltyPointRule { get; set; }
    }
}
