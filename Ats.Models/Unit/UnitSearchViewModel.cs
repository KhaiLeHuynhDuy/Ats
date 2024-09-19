using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Unit
{
   public class UnitSearchViewModel : BaseSearchViewModel
    {
        public List<UnitViewModel> Units { get; set; }
        public UnitViewModel Unit { get; set; }
    }
}
