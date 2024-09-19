using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models.Brand;
using Ats.Models.Channel;
using Ats.Models.Loyalty;
using Ats.Models.Product;
using Ats.Models.Store;

namespace Ats.Models.Loyalty
{
    public class LoyaltySearchViewModel :BaseSearchViewModel
    {
      
        public List<LoyaltyViewModel> Loyaltys { get; set; }
        public LoyaltyViewModel Loyalty { get; set; }

        public List<LoyaltyPointRulesViewModel> LoyaltyPointRules { get; set; }
        public LoyaltyPointRulesViewModel LoyaltyPointRule { get; set; }

        public List<LoyaltyPointSettingViewModel> LoyaltyPointSettings { get; set; }
        public LoyaltyPointSettingViewModel LoyaltyPointSetting { get; set; }

        public List<LoyaltyPointTypeViewModel> LoyaltyPointTypes { get; set; }
        public LoyaltyPointTypeViewModel LoyaltyPointType { get; set; }

        #region Item Point Rule Custom
        public List<PointRuleStoresViewModel> PointRuleStores { get; set; }
        public PointRuleStoresViewModel PointRuleStore { get; set; }

        public List<PointRuleProductViewModel> PointRuleProducts { get; set; }
        public PointRuleProductViewModel PointRuleProduct { get; set; }

        public List<PointRuleChannelViewModel> PointRuleChannels { get; set; }
        public PointRuleChannelViewModel PointRuleChannel { get; set; }

        public List<PointRuleCategoryViewModel> PointRuleCategories { get; set; }
        public PointRuleCategoryViewModel PointRuleCategory { get; set; }

        public List<PointRuleBrandViewModel> PointRuleBrands { get; set; }
        public PointRuleBrandViewModel PointRuleBrand { get; set; }
        #endregion

        public double PointEarn { get; set; }
        public double PointRedemption { get; set; }
        public double EarningRate { get; set; }
        public double RedeemedRate { get; set; }
    }
}
