using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class ProductSearchViewModel : BaseSearchViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
