using Ats.Domain.Identity.Models;
using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    [Table("loanbooks")]
    public class LoanBook : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? LeadId { get; set; }
        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; }
        public Guid? BorrowerId { get; set; }
        [ForeignKey("BorrowerId")]
        public virtual Borrower Borrower { get; set; }
        public int? LoanProductId { get; set; }
        [ForeignKey("LoanProductId")]
        public virtual LoanPackage LoanProduct { get; set; }
        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
       
        public BOOK_TYPE BookType { get; set; }
        public DateTime? BookDate { get; set; }
        public double Price { get; set; }
    }
    public enum BOOK_TYPE
    {
        [Display(Name = "Text")]
        TEXT = 1,
        [Display(Name = "Voice")]
        VOICE = 2,
    }
}
