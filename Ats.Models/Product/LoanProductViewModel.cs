using Ats.Domain;
using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class LoanProductViewModel
    {
        public LoanProductViewModel()
        {
            ProductAttributes = new HashSet<ProductAttributeModel>();
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }

        public double Duration { get; set; }
        public string Description { get; set; }
        public DATA_TYPE DataType { get; set; }
        public bool IsActive { get; set; }
        public ProductAttributeModel ProductAttribute { get; set; }
        public ICollection<ProductAttributeModel> ProductAttributes { get; set; }
    }
}
