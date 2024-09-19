using Ats.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Ats.Domain.Member.Models
{
    [Table("loyaltypoints")]
    public class LoyaltyPoint : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string WalletId { get; set; }
        [ForeignKey("WalletId")]
        public Guid? PointType { get; set; }
        [ForeignKey("PointType")]
        public DateTime? LastUpdate { get; set; }
        public double Point { get; set; }

    }
}
