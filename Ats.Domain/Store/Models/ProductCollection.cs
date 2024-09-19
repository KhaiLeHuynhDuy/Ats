using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Store.Models
{
    [Table("productcollections")]
    public class ProductCollection : AuditBase, IEntity<Guid>
    {
        public ProductCollection()
        {
            ProductItems = new HashSet<ProductCollectionItem>();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? EffectiveDateBegin { get; set; }
        public DateTime? EffectiveDateEnd { get; set; }

        public virtual ICollection<ProductCollectionItem> ProductItems { get; set; }
    }
}
