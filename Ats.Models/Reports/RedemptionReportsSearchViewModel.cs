using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reports
{
   public class RedemptionReportsSearchViewModel : BaseSearchViewModel
    {
        public List<RedemptionReportsViewModel> redemptionReports { get; set; }
        public RedemptionReportsViewModel redemption { get; set; }

        public Guid? collectionId { get; set; }
        public string collectionName { get; set; }

    }
}
