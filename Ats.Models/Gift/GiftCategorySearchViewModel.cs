using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Gift
{
    public class GiftCategorySearchViewModel : BaseSearchViewModel
    {
        public List<GiftCategoryViewModel> GiftCategorys { get; set; }
        public GiftCategoryViewModel GiftCategory { get; set; }
    }
}
