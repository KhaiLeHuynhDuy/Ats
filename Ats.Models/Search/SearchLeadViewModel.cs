using Ats.Models.Lead;
using System;
using System.Collections.Generic;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Models.Search
{
    public class SearchLeadViewModel : BaseSearchViewModel
    {
        public int? ProvinceId { get; set; }
        public int? OccupationId { get; set; }
        public int? LoanProductId { get; set; }
        public int? LoanPeriod { get; set; }
        public int? LoanAmount { get; set; }
        public List<LeadViewModel> Leads { get; set; }
    }
}
