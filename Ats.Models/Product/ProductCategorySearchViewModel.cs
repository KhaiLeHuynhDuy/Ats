using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class ProductCategorySearchViewModel : BaseSearchViewModel
    {
        public List<ProductCategoryViewModel> ProductCategories { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
    }
}
