using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.MemberWallet
{
    public class LoyaltyPointsViewModel
    {

        public Guid Id { get; set; }
        public string WalletId { get; set; }

        public Guid? LoyaltyType { get; set; }

        public DateTime? LastUpdate { get; set; }
        public double Point { get; set; }

    }
}
