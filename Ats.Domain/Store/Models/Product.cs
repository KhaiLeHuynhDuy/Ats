using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Store.Models
{
    [Table("products")]
    public class Product : AuditBase, IEntity<int>
    {
        public Product()
        {
            ProductAttributes = new HashSet<ProductAttribute>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }

        public int? ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }
                
        //public int? BrandId { get; set; }
        //[ForeignKey("BrandId")]
        //public virtual Brand.Models.Brand Brand { get; set; }

        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        public double RetailPrice { get; set; }
        public double PurchasedPrice { get; set; }
        public string ImageUrl { get; set; }

        
        public string Currency { get; set; }
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }

        public bool AllowEarnPoint { get; set; }
        public bool AllowRedeem  { get; set; }

        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
    }
}
