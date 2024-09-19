using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Store
{
    public interface IProductCategoryRepository : IRepository<ProductCategory, int>
    {
    }
}
