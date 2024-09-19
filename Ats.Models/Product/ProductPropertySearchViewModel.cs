using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class ProductPropertySearchViewModel : BaseSearchViewModel
    {
        public List<ProductPropertyViewModel> ProductProperties { get; set; }
        public ProductPropertyViewModel ProductProperty { get; set; }
    }
}
