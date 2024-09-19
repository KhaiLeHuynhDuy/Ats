using Ats.Domain.Accounts.Models;
using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Organization.Models
{
    [Table("warehouses")]
    public class Warehouse : AuditBase, IEntity<Guid>
    {
        public Warehouse()
        {
            Teams = new HashSet<Team>();
            Assets = new HashSet<Asset>();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public WAREHOUSE_TYPE WarehouseType { get; set; }
        public Guid? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
    }
    public enum WAREHOUSE_TYPE
    {
        [Display(Name = "Physical")]
        PHYSICAL = 1,
        [Display(Name = "Logical")]
        LOGICAL = 2,
    }
}
