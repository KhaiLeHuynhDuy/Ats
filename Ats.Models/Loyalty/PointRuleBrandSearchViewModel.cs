using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Loyalty
{
   public class PointRuleBrandSearchViewModel : BaseSearchViewModel
    {
        public List<PointRuleBrandViewModel> PointRuleBrands { get; set; }
        public PointRuleBrandViewModel PointRuleBrand { get; set; }
    }
}
