using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Voucher
{
    public class VoucherCategorySearchViewModel : BaseSearchViewModel
    {
        public List<VoucherCategoryViewModel> VoucherCategorys { get; set; }
        public VoucherCategoryViewModel VoucherCategory { get; set; }
    }
}
