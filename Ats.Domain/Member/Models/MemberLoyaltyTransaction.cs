
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("memberloyaltytransactions")]
    public class MemberLoyaltyTransaction : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        [ForeignKey("WalletId")]
        public virtual MemberWallet MemberWallet { get; set; }

        public bool TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string InvoiceNo { get; set; }
        public double Amount { get; set; }
        public double Point { get; set; }
        public double Rate { get; set; }

        public string RefId { get; set; }
        public int? PointTypeId { get; set; }
        [ForeignKey("PointTypeId")]
        public virtual LoyaltyPointType PointType { get; set; }

        public int? ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Channel.Models.MemberChannel Channel { get; set; }

        public int? StoreId { get; set; }
        [ForeignKey("StoreId")]
        public virtual Store.Models.Store Store { get; set; }

       

        public Guid? CouponId { get; set; }
        [ForeignKey("CouponId")]
        public virtual Ats.Domain.Coupon.Models.Coupon Coupon { get; set; }

        public string Remark { get; set; }
    }
}
