using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Coupon.Models
{
    [Table("coupons")]
    public class Coupon : AuditBase, IEntity<Guid>
    {
        public Coupon()
        {
            MemberCoupons = new HashSet<MemberCoupon>();

        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public int? CouponCategoryId { get; set; }
        [ForeignKey("CouponCategoryId")]
        public virtual CouponCategory CouponCategory { get; set; }

        public int? ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Ats.Domain.Channel.Models.MemberChannel Channel { get; set; }
        public int? StoreId { get; set; }
        [ForeignKey("StoreId")]
        public virtual Ats.Domain.Store.Models.Store Store { get; set; }

        public bool CouponType { get; set; } // 0 discount, 1 cash

        public double Amount { get; set; }
        public double Discount { get; set; }

        public bool ValidityPeriodType { get; set; } // 0 relative date - 1 specific date

        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public int AfterReceptionDay { get; set; }
        public int AfterEffectiveDay { get; set; }

        public int MinimumPurchase { get; set; }
        public bool Active { get; set; }

        public string TermAndConditions { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<MemberCoupon> MemberCoupons { get; set; }

    }
}
