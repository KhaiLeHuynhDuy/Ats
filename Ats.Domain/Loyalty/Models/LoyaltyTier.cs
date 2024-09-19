using Ats.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Ats.Commons.Constants;
namespace Ats.Domain.Loyalty.Models
{
    [Table("loyaltytiers")]
    public class LoyaltyTier : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string TierName { get; set; }
        public int TierLevel { get; set; }
        public Boolean Active { get; set; }
        public double UpgradePoint { get; set; }
        public double DowngradePoint { get; set; }
        public double PointMin { get; set; }
        public double PointMax { get; set; }
        public string Desc { get; set; }

        public virtual ICollection<MemberLoyaltyTier> MemberLoyalties { get; set; }
    }
}
