using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Coupon;

namespace Ats.Data.Repositories.Coupon
{
    public class CouponCategoryRepository : Repository<Ats.Domain.Coupon.Models.CouponCategory, int>, ICouponCategoryRepository
    {
        public CouponCategoryRepository(SCDataContext context) : base(context)
        {
        }
    }
}
