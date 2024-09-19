using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models
{
    public class BaseSearchProductPointsScanViewModel
    {
        public int? OrderBy { get; set; }
        public string Keyword { get; set; }
        public string StringDateFrom { get; set; }

        public string StringDateTo { get; set; }

        public PagerProductPointsScanReportViewModel PagerProductPointsScan { get; set; }
    }
}
