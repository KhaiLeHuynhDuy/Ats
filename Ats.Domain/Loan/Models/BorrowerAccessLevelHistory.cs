using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("borroweraccesslevelhistories")]
    public class BorrowerAccessLevelHistory : IEntity<Guid>
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
        public BORROWER_ACCESS_LEVEL BorrowerAccessLevel { get; set; }
    }
    public enum BORROWER_ACCESS_LEVEL
    {
        USER = 1,
        TEAM = 2,
        GLOBAL = 3,
    }
}
