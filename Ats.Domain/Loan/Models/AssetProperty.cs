using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("assetproperties")]
    public class AssetProperty : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public DATA_TYPE DataType { get; set; }

        public int? AssetTypeId { get; set; }
        [ForeignKey("AssetTypeId")]
        public virtual AssetType AssetType { get; set; }

        public string Description { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
    }
}
