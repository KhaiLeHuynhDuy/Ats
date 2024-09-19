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
    public class WalletViewModel
    {
        public WalletViewModel()
        {
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }

        public string WalletName { get; set; }
        public double Balance { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
