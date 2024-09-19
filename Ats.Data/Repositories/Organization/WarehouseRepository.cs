using Ats.Data.EntityFramework;
using Ats.Domain.Organization;
using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Organization
{
    public class WarehouseRepository : Repository<Warehouse, Guid>, IWarehouseRepository
    {
        public WarehouseRepository(SCDataContext context) : base(context)
        {
        }
    }
}
