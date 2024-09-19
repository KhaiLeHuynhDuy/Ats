using Ats.Domain.Organization.Models;
using Ats.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IWarehouseService
    {
        List<WarehouseViewModel> Search(string searchText, WAREHOUSE_TYPE? type, string orderField, bool IsAscOrder, int page, int size, out int total);
        WarehouseViewModel GetWarehouse(Guid id);
        void CreateWarehouse(WarehouseViewModel model);
        void UpdateWarehouse(WarehouseViewModel model);
        void DeleteWarehouse(Guid id);
    }
}
