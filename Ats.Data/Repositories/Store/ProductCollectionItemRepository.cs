using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Store;
using Ats.Domain.Store.Models;

namespace Ats.Data.Repositories.Store
{
    public class ProductCollectionItemRepository : Repository<ProductCollectionItem, Guid>, IProductCollectionItemRepository
    {
        public ProductCollectionItemRepository(SCDataContext context) : base(context)
        {
        }
    }
}
