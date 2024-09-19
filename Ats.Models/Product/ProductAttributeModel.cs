using Ats.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Product
{
    public class ProductAttributeModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductPropertyId { get; set; }
        public string ProductPropertyName { get; set; }
        public string ProductPropertyShortName { get; set; }
        public DATA_TYPE DataType { get; set; }
        public string Value { get; set; }
    }
}
