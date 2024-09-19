using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("borrowerlevelhistories")]
    public class BorrowerLevelHistory : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BorrowerId { get; set; }
        [ForeignKey("BorrowerId")]
        public virtual Borrower Borrower { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public BORROWER_LEVEL BorrowerLevel { get; set; }
    }
    public enum BORROWER_LEVEL
    {
        NEW = 1,
        LEVEL_1 = 2,
        LEVEL_2 = 3,
        LEVEL_3 = 4,
    }
}
