using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Brand
{
   public class BrandSearchViewModel : BaseSearchViewModel
    {
        public List<BrandViewModel> Brands { get; set; }
        public BrandViewModel Brand { get; set; }
    }
}
