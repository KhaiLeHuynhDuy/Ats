using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Coupon.Models
{
    [Table("couponcategories")]
    public class CouponCategory : AuditBase, IEntity<int>
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public Boolean Active { get; set; } = true;
   
        public string Desc { get; set; }
    }
}
