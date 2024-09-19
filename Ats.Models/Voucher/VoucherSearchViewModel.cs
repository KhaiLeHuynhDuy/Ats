using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Voucher
{
    public class VoucherSearchViewModel : BaseSearchViewModel
    {
        public List<VoucherViewModel> Vouchers { get; set; }
        public VoucherViewModel Voucher { get; set; }
        public int? voucherCateid { get; set; }
        public int? channelid { get; set; }
        public bool? voucherType { get; set; }
        public int? stockFrom { get; set; }
        public int? stockTo { get; set; }
    }
}
