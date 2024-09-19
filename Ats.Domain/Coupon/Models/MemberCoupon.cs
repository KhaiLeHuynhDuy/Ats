using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Coupon.Models
{
    [Table("membercoupons")]
    public class MemberCoupon : AuditBase, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member.Models.Member Member { get; set; }

        public Guid? CouponId { get; set; }
        [ForeignKey("CouponId")]
        public virtual Coupon Coupon { get; set; }

        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool Valid { get; set; }
        public double PointRedemption { get; set; }
        public double EarningRate { get; set; }

        public string Remark { get; set; }

        public DateTime? RedeemedDate { get; set; }
        public string RedeemedCode { get; set; }
    }
}
