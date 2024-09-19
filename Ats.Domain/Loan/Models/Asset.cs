using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("assets")]
    public class Asset : AuditBase, IEntity<int>
    {
        public Asset()
        {
            AssetAttributes = new HashSet<AssetAttribute>();
        }
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }

        public int? AssetTypeId { get; set; }
        [ForeignKey("AssetTypeId")]
        public virtual AssetType AssetType { get; set; }

        public Guid? LoanId { get; set; }
        [ForeignKey("LoanId")]
        public virtual Loan Loan { get; set; }

        public Guid? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        public string Description { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public virtual ICollection<AssetAttribute> AssetAttributes { get; set; }
    }
}
