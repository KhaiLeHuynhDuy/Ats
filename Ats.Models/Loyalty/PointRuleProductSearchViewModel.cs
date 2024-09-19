using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleProductSearchViewModel : BaseSearchViewModel
    {
        public List<PointRuleProductViewModel> PointRuleProducts { get; set; }
        public PointRuleProductViewModel PointRuleProduct { get; set; }
    }
}
