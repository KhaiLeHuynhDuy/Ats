using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Store;
namespace Ats.Data.Repositories.Store
{
    public class UnitRepository : Repository<Ats.Domain.Store.Models.Unit, int>, IUnitRepository
    {
        public UnitRepository(SCDataContext context) : base(context)
        {
        }
    }
}
