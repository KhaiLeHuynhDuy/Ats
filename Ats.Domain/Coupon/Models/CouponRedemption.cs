using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Coupon.Models
{
    [Table("couponredemptions")]
    public class CouponRedemption : AuditBase, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid MemberCouponId { get; set; }
        [ForeignKey("MemberCouponId")]
        public virtual MemberCoupon MemberCoupon { get; set; }

        public int? ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Ats.Domain.Channel.Models.MemberChannel Channel { get; set; }
        public int? StoreId { get; set; }
        [ForeignKey("StoreId")]
        public virtual Ats.Domain.Store.Models.Store Store { get; set; }

        public bool CouponType { get; set; } // 0 discount, 1 cash
        public double CouponAmount { get; set; }
        public double CouponDiscount { get; set; }

        public DateTime? TransDate { get; set; }
        public string TransInvoiceNo { get; set; }
        public double TransAmount { get; set; }
        public double TransDiscount { get; set; }

        public DateTime? OpenDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public string Remark { get; set; }
    }
}
