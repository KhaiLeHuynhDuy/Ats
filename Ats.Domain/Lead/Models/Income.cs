using Ats.Domain.Account.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models 
{ 
    [Table("incomes")]
    public class Income : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string Company { get; set; }
        public int Position { get; set; }
        public Guid LeadId { get; set; }
        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; }
        public Guid OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization.Models.Organization Organization { get; set; }
        public int JobTitleId { get; set; }
        [ForeignKey("JobTitleId")]
        public virtual JobTitle JobTitle { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public double? SalaryFrom { get; set; }
        public double? SalaryTo { get; set; }
    }
}
