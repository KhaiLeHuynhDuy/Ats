using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ats.Domain;

namespace Ats.Domain.Loyalty.Models
{
    [Table("memberloyaltytiers")]
    public class MemberLoyaltyTier : AuditBase, IEntity<System.Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Ats.Domain.Member.Models.Member Member { get; set; }

        public int? Tier { get; set; }
        [ForeignKey("Tier")]
        public virtual LoyaltyTier LoyaltyTier { get; set; }

        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveEnd { get; set; }
        public double UpgradePoint { get; set; }
        public double DowngradePoint { get; set; }
    }
}
