using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Accounts.Models
{
    [Table("wallets")]
    public class Wallet : AuditBase, IEntity<Guid>
    {
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string WalletName { get; set; }
        public double Balance { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
