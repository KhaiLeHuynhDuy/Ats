using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Models.MemberWallets
{
    public class MemberLoyaltyTransactionsViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public int? PointTypeId { get; set; }
        public bool? TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public double Point { get; set; }
        public double Amount { get; set; } 
        public double Rate { get; set; }
        public string RefId { get; set; }

        public int? ChannelId { get; set; }
        public int? StoreId { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? CouponId { get; set; }
        public int? TransactionId { get; set; }
        public string Remark { get; set; }
    }
}
