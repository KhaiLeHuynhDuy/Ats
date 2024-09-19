using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Store;
namespace Ats.Data.Repositories.Store
{
    public class StoreRepository : Repository<Ats.Domain.Store.Models.Store, int>, IStoreRepository
    {
        public StoreRepository(SCDataContext context) : base(context)
        {
        }
    }
}
