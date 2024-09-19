using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Coupon
{
    public class CouponCategoryViewModel
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public Boolean Active { get; set; }
        public string Desc { get; set; }
    }
}
