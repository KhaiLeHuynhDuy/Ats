using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
   public class ProductCollectionItemViewModel
    {
        public Guid Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public Guid? ProductCollectionId { get; set; }
        public string ProductCollectionName { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
  
        public string Remark { get; set; }
    }
}
