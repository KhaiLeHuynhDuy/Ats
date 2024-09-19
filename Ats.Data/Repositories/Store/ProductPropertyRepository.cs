using Ats.Data.EntityFramework;
using Ats.Domain.Store;
using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Store
{
    public class ProductPropertyRepository : Repository<ProductProperty, int>, IProductPropertyRepository
    {
        public ProductPropertyRepository(SCDataContext context) : base(context)
        {
        }
    }
}
