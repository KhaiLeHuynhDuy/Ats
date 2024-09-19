using Ats.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class ProductPropertyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string Description { get; set; }
        public DATA_TYPE DataType { get; set; }
        public bool IsActive { get; set; }
    }
}
