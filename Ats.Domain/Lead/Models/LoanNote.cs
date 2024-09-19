using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    [Table("loannotes")]
    public class LoanNote : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public double Amount { get; set; }
        public bool IsConfirmed { get; set; }
        public string Note { get; set; }
        public Guid LeadId { get; set; }
        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; }
        public PRODUCT_TYPE ProductType { get; set; }
    }
}
