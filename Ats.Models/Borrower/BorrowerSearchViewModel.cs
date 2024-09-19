using System;
using System.Collections.Generic;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Models.Borrower
{
    public class BorrowerSearchViewModel : BaseSearchViewModel
    {
        public List<BorrowerViewModel> Borrowers { get; set; }
        public BorrowerViewModel Borrower { get; set; }
        public int? LoanPeriodId { get; set; }
        public int? LoanAmountId { get; set; }
        public int? ProvinceId { get; set; }
        public int? OccupationId { get; set; }
        public int? LoanProductId { get; set; }
    }
}
