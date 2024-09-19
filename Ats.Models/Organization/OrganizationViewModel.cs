using Ats.Domain.Organization.Models;
using Ats.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Organization
{
    public class OrganizationViewModel
    {
        public OrganizationViewModel()
        {
            Warehouses = new HashSet<WarehouseViewModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ORGANIZATION_TYPE OrganizationType { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<WarehouseViewModel> Warehouses { get; set; }
        public WarehouseViewModel Warehouse { get; set; }
    }
}
