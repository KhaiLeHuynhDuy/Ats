using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class ProductCategoryViewModel
    {
        public ProductCategoryViewModel()
        {
            ProductProperties = new HashSet<ProductPropertyViewModel>();
        }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int? CateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProductPropertyViewModel> ProductProperties { get; set; }
        public ProductPropertyViewModel ProductProperty { get; set; }
    }
}
