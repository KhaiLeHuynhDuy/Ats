using Ats.Domain.Account.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{ 
    [Table("debts")]
    public class Debt : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LeadId { get; set; }
        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; }
        public Guid OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization.Models.Organization Organization { get; set; }
        public PRODUCT_TYPE ProductType { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double? Amount { get; set; }
    }
}
