using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Lead.Models;
using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("loans")]
    public class Loan : AuditBase, IEntity<Guid>
    {
        public Loan()
        {
            Documents = new HashSet<Document>();
        }
        [Key]
        public Guid Id { get; set; }

        public Guid? BorrowerId { get; set; }
        [ForeignKey("BorrowerId")]
        public virtual Borrower Borrower { get; set; }

        public int? LenderId { get; set; }
        [ForeignKey("LenderId")]
        public virtual Lender Lender { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual LoanProduct Product { get; set; }

        public double Amount { get; set; }
        public double LoanPeriod { get; set; }
        
        public virtual ICollection<Document> Documents { get; set; }
    }
}
