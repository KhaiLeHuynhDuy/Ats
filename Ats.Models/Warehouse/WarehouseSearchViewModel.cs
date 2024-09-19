using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Warehouse
{
    public class WarehouseSearchViewModel : BaseSearchViewModel
    {
        public List<WarehouseViewModel> Warehouses { get; set; }
        public WarehouseViewModel Warehouse { get; set; }
        public WAREHOUSE_TYPE? Type { get; set; }
    }
}
