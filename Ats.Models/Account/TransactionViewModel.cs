using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Models.Account
{
    public class TransactionViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid WalletId { get; set; }
        public DateTime Date { get; set; }
        public TRANSACTION_TYPE TransType { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool Action { get; set; } = false; // True - Income, False - Out
        public double Amount { get; set; }
        public Guid? RefId { get; set; }

        public string Comment { get; set; }
    }
}
