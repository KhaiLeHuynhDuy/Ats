using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Coupon
{
    public class CouponCategorySearchViewModel : BaseSearchViewModel
    {
        public List<CouponCategoryViewModel> CouponCategorys { get; set; }
        public CouponCategoryViewModel CouponCategory { get; set; }
    }
}
