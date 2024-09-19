using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    [Table("leadstatushistories")]
    public class LeadStatusHistory : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public Guid LeadId { get; set; }
        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public LEAD_STATUS LeadStatus { get; set; }
    }
    public enum LEAD_STATUS
    {
        NEW = 1,
        UPDATED = 2,
        CANCELLED = 3,
        PENDING = 4,
    }
}
