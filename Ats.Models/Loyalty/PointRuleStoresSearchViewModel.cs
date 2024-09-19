using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleStoresSearchViewModel : BaseSearchViewModel
    {
        public List<PointRuleStoresViewModel> PointRuleStores { get; set; }
        public PointRuleStoresViewModel PointRuleStore { get; set; }
    }
}
