using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Store.Models
{
    [Table("productcategories")]
    public class ProductCategory : AuditBase, IEntity<int>
    {
        public ProductCategory()
        {
            ProductProperties = new HashSet<ProductProperty>();
        }
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public virtual ICollection<ProductProperty> ProductProperties { get; set; }
    }
}
