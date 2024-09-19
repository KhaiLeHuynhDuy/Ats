using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ats.Domain;



namespace Ats.Domain.Loyalty.Models
{
    [Table("loyaltypointrules")]
    public class LoyaltyPointRule : IEntity<Guid>
    {

        public LoyaltyPointRule()
        {
            PointRuleStores = new HashSet<PointRuleStore>();
            PointRuleBrands = new HashSet<PointRuleBrand>();
            PointRuleProducts = new HashSet<PointRuleProduct>();
            PointRuleChannels = new HashSet<PointRuleChannel>();
            PointRuleCategories = new HashSet<PointRuleCategory>();
        }
        [Key]
        public Guid Id { get; set; }
        public string RuleName { get; set; }

        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool Valid { get; set; }

        public bool BonusType { get; set; } //0-Amount / 1-rate

        public double RedemptionRate { get; set; }
        public double EarningRate { get; set; }

        public string Remark { get; set; }


        public virtual ICollection<PointRuleStore> PointRuleStores { get; set; }
        public virtual ICollection<PointRuleBrand> PointRuleBrands { get; set; }
        public virtual ICollection<PointRuleProduct> PointRuleProducts { get; set; }
        public virtual ICollection<PointRuleChannel> PointRuleChannels { get; set; }
        public virtual ICollection<PointRuleCategory> PointRuleCategories { get; set; }

    }
}
