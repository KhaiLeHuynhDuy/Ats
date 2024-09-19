using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleCategorySearchViewModel : BaseSearchViewModel
    {
        public List<PointRuleCategoryViewModel> PointRuleCategories { get; set; }
        public PointRuleCategoryViewModel PointRuleCategory { get; set; }
    }
}
