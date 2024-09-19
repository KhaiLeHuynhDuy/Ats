using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("assetloans")]
    public class AssetLoan : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public int? AssetId { get; set; }
        [ForeignKey("AssetId")]
        public virtual Asset Asset { get; set; }

        public Guid? LoanId { get; set; }
        [ForeignKey("LoanId")]
        public virtual Loan Loan { get; set; }

        public Guid? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
    }
}
