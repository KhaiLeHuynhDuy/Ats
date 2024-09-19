using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loyalty.Models
{
    [Table("pointrulechannels")]
    public class PointRuleChannel : AuditBase, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public int? ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Channel.Models.MemberChannel Channel { get; set; }

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
