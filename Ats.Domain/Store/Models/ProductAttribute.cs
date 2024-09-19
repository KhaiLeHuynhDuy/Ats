using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Store.Models
{
    [Table("productattributes")]
    public class ProductAttribute : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int ProductPropertyId { get; set; }
        [ForeignKey("ProductPropertyId")]
        public virtual ProductProperty ProductProperty { get; set; }
        public DATA_TYPE DataType { get; set; }
        public string Value { get; set; }
    }
}
