using System;
using System.Collections.Generic;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Models.Lead
{
    public class SearchLeadViewModel : BaseSearchViewModel
    {
        public List<LeadViewModel> Leads { get; set; }
        public LeadViewModel Lead { get; set; }
        public int? LoanPeriodId { get; set; }
        public int? LoanAmountId { get; set; }
        public int? ProvinceId { get; set; }
        public int? OccupationId { get; set; }
        public int? LoanProductId { get; set; }
    }
}
