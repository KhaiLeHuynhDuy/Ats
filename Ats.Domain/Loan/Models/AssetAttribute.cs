using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("assetattributes")]
    public class AssetAttribute : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public int AssetId { get; set; }
        [ForeignKey("AssetId")]
        public virtual Asset Asset { get; set; }

        public int AssetPropertyId { get; set; }
        [ForeignKey("AssetPropertyId")]
        public virtual AssetProperty AssetProperty { get; set; }

        public DATA_TYPE DataType { get; set; }
        public string Value { get; set; }
    }
}
