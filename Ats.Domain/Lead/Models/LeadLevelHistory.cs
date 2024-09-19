using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    [Table("leadlevelhistories")]
    public class LeadLevelHistory : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LeadId { get; set; }
        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public LEAD_LEVEL LeadLevel { get; set; }
    }
    public enum LEAD_LEVEL
    {
        NEW = 1,
        LEVEL_1 = 2,
        LEVEL_2 = 3,
        LEVEL_3 = 4,
    }
}
