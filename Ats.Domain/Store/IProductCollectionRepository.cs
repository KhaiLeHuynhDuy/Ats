using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Store.Models;

namespace Ats.Domain.Store
{
    public interface IProductCollectionRepository : IRepository<ProductCollection, Guid>
    {
    }
}
