using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("borrowerstatushistories")]
    public class BorrowerStatusHistory : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public Guid BorrowerId { get; set; }
        [ForeignKey("BorrowerId")]
        public virtual Borrower Borrower { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public BORROWER_STATUS BorrowerStatus { get; set; }
    }
    public enum BORROWER_STATUS
    {
        NEW = 1,
        UPDATED = 2,
        CANCELLED = 3,
        PENDING = 4,
    }
}
