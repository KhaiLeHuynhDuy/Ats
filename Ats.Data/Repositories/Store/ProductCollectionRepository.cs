using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Store;
using Ats.Domain.Store.Models;

namespace Ats.Data.Repositories.Store
{
  
    public class ProductCollectionRepository : Repository<ProductCollection, Guid>, IProductCollectionRepository
    {
        public ProductCollectionRepository(SCDataContext context) : base(context)
        {
        }
    }
}


