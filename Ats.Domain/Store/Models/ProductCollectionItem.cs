using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Store.Models
{
    [Table("productcollectionitems")]
    public class ProductCollectionItem : AuditBase, IEntity<Guid>
    {
        public ProductCollectionItem()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }
        [Key]
        public Guid Id { get; set; }
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public Guid? ProductCollectionId { get; set; }
        [ForeignKey("ProductCollectionId")]
        public virtual ProductCollection ProductCollection { get; set; }

        public int Stock { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }

        public string Remark { get; set; }

        public virtual ICollection<ProductOrder> ProductOrders { get; set; }

    }
}
