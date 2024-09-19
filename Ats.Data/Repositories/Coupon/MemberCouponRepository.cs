using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Coupon;

namespace Ats.Data.Repositories.Coupon
{
    public class MemberCouponRepository : Repository<Ats.Domain.Coupon.Models.MemberCoupon, Guid>, IMemberCouponRepository
    {
        public MemberCouponRepository(SCDataContext context) : base(context)
        {
        }
    }
}
