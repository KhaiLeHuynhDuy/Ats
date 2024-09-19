using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    [Table("documents")]
    public class Document : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string DocumentName { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }
        public string Remark { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ValidTo { get; set; }
        public Guid? LeadId { get; set; }
        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; }
        public Guid? BorrowerId { get; set; }
        [ForeignKey("BorrowerId")]
        public virtual Borrower Borrower { get; set; }
        public DOCUMENT_TYPE DocumentType { get; set; }
    }
    public enum DOCUMENT_TYPE
    {
        [Display(Name = "Id")]
        ID = 1,
        [Display(Name = "Labor")]
        LABOR = 2,
        [Display(Name = "Finance statement sheet")]
        FINANCE_STATEMENT_SHEET = 3,
    }
}
