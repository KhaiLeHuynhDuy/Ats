using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Store
{
    public class PurchasedTransExcelViewModel
    {
        public string InvoiceNo { get; set; }
        public string RefId { get; set; }

        public string Amount { get; set; }

        public DateTime PurchasedDate { get; set; }
        public string PurchasedDates { get; set; }


        public string Currency { get; set; }
        public string Remark { get; set; }

        public string MemberReference { get; set; }

        public DateTime? RegisterDate { get; set; }
        public Guid? RegisterEmployee { get; set; }
        public string REGISTERCHANNEL { get; set; }
        public string REGISTERSTORE { get; set; }
    }
}
