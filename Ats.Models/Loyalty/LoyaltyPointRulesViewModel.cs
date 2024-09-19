using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Loyalty.Models;
using Ats.Models.Brand;
using Ats.Models.Channel;
using Ats.Models.Product;
using Ats.Models.Store;

namespace Ats.Models.Loyalty
{
    public class LoyaltyPointRulesViewModel : BaseSearchViewModel
    {
        public LoyaltyPointRulesViewModel()
        {
            PointRuleStores = new HashSet<PointRuleStoresViewModel>();
            PointRuleProducts = new HashSet<PointRuleProductViewModel>();
            PointRuleChannels = new HashSet<PointRuleChannelViewModel>();
            PointRuleCategories = new HashSet<PointRuleCategoryViewModel>();
            pointRuleBrands = new HashSet<PointRuleBrandViewModel>();

        }
        public Guid Id { get; set; }
        public string RuleName { get; set; }
        public double RedemptionRate { get; set; }
        public double EarningRate { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool BonusType { get; set; }
        public string? EffectiveFroms { get; set; }
        public string? EffectiveTos { get; set; }
        public string Remark { get; set; }
        public bool Valid { get; set; }

        public ICollection<PointRuleStoresViewModel> PointRuleStores { get; set; }
        public PointRuleStoresViewModel PointRuleStore { get; set; }

        public ICollection<PointRuleProductViewModel> PointRuleProducts { get; set; }
        public PointRuleProductViewModel PointRuleProduct { get; set; }

        public ICollection<PointRuleChannelViewModel> PointRuleChannels { get; set; }
        public PointRuleChannelViewModel PointRuleChannel { get; set; }

        public ICollection<PointRuleCategoryViewModel> PointRuleCategories { get; set; }
        public PointRuleCategoryViewModel PointRuleCategory { get; set; }

        public ICollection<PointRuleBrandViewModel> pointRuleBrands { get; set; }
        public PointRuleBrandViewModel PointRuleBrand { get; set; }

    }
}

