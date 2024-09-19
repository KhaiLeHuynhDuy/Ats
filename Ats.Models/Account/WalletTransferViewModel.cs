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
    public class WalletTransferViewModel
    {
        public WalletTransferViewModel()
        {
        }

        [Required]
        public string EmailFrom { get; set; }
        
        [Required]
        public string EmailTo { get; set; }
        
        public double Amount { get; set; }
    }
}
