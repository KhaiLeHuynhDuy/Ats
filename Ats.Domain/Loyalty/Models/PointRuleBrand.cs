using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loyalty.Models
{
    [Table("pointrulebrands")]
    public class PointRuleBrand : AuditBase, IEntity<Guid>
    {
        public Guid Id { get; set; }
        //public int? BrandId { get; set; }
        //[ForeignKey("BrandId")]
        //public virtual Brand.Models.Brand Brand { get; set; }

        public Guid? LoyaltyPointRuleId { get; set; }
        [ForeignKey("LoyaltyPointRuleId")]
        public virtual LoyaltyPointRule LoyaltyPointRule { get; set; }

        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool Valid { get; set; }
        public double PointRedemption { get; set; }
        public double EarningRate { get; set; }

        public string Remark { get; set; }
    }
}
