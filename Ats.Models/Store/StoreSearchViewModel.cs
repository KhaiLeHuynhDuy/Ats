using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Store
{
    public class StoreSearchViewModel : BaseSearchViewModel
    {
        public List<StoreViewModel> Stores { get; set; }
        public StoreViewModel Store { get; set; }
    }
}
