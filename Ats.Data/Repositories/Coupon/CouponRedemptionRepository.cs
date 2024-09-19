using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Coupon;

namespace Ats.Data.Repositories.Coupon
{
    public class CouponRedemptionRepository : Repository<Ats.Domain.Coupon.Models.CouponRedemption, Guid>, ICouponRedemptionRepository
    {
        public CouponRedemptionRepository(SCDataContext context) : base(context)
        {
        }
    }
}
