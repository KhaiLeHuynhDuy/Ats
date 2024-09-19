using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Store.Models;

namespace Ats.Domain.Store
{
    public interface IProductCollectionItemRepository : IRepository<ProductCollectionItem, Guid>
    {
    }
}
