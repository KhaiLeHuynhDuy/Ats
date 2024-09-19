using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reports
{
  public  class RedemptionReportsViewModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int TotalNewOrder { get; set; }
        public int TotalInTransit { get; set; }
        public int TotalCompleted { get; set; }
        public int TotalCanceled { get; set; }
        public int Stock { get; set; }
    }
}
