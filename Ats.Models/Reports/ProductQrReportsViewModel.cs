using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reports
{
    public class ProductQrReportsViewModel
    {
        public List<int> Stt { get; set; }
        public int STT { get; set; }
        public string CodeScanDate { get; set; }
        public string MemberCode { get; set; }
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string FullName{get;set;}
        public string BillNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public int? PointTypeId { get; set; }

        public string FullProduct { get; set; }
        public int total { get; set; }
    }
}
