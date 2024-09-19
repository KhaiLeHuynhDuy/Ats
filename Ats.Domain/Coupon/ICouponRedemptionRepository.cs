using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Coupon
{
    public interface ICouponRedemptionRepository : IRepository<Models.CouponRedemption, Guid>
    {
    }
}
