using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loyalty.Models
{
    [Table("pointrulecategories")]
    public class PointRuleCategory : AuditBase, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public int? ProductCateId { get; set; }
        [ForeignKey("ProductCateId")]
        public virtual ProductCategory ProductCategory { get; set; }

        public Guid? LoyaltyPointRuleId { get; set; }
        [ForeignKey("LoyaltyPointRuleId")]
        public virtual LoyaltyPointRule LoyaltyPointRule { get; set; }

        public bool Valid { get; set; }
    }
}
