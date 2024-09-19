using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Voucher
{
    public class VoucherCategoryViewModel
    {
        public int Id { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherName { get; set; }
        public Boolean Active { get; set; }
        public string Desc { get; set; }
    }
}
