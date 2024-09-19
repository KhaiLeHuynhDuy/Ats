using Ats.Domain.Organization.Models;
using Ats.Models.Asset;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Warehouse
{
    public class WarehouseViewModel
    {
        public WarehouseViewModel()
        {
            Assets = new HashSet<AssetViewModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public WAREHOUSE_TYPE WarehouseType { get; set; }
        public Guid? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<AssetViewModel> Assets { get; set; }
        public AssetViewModel Asset{ get; set; }
    }
}
