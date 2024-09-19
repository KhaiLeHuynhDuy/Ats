using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reports
{
   public class MemberReportsSearchViewModel : BaseSearchViewModel
    {
        public List<MemberReportsViewModel> memberReports { get; set; }
        public MemberReportsViewModel memberReport { get; set; }
        public string? DateFrom { get; set; }
        public string? DateTo { get; set; }
        public string StringDateFrom { get; set; }
        public string StringDateTo { get; set; }


    }
}
