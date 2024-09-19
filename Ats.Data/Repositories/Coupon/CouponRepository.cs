using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Coupon;
using Ats.Domain.Coupon.Models;

namespace Ats.Data.Repositories.Coupon
{
    public class CouponRepository : Repository<Ats.Domain.Coupon.Models.Coupon, Guid>, ICouponRepository
    {
        public CouponRepository(SCDataContext context) : base(context)
        {
        }
        
    }
}

