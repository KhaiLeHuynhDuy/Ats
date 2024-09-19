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
    [Table("transactions")]
    public class Transaction : AuditBase, IEntity<int>
    {
        public Transaction()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }

        [Required]
        public Guid WalletId { get; set; }
        [ForeignKey("WalletId")]
        public virtual Wallet Wallet { get; set; }
        public DateTime Date { get; set; }
        public TRANSACTION_TYPE TransType { get; set; }
        //public bool TransactionType { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool Action { get; set; } = false; // True - Income, False - Out
        public double Amount { get; set; }
        public Guid? RefId { get; set; }
        public string Comment { get; set; }        
    }
}
