using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reports
{
    public class LoyaltyTransViewModel
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }

        public DateTime AddedTimestamp { get; set; }
    }
}
