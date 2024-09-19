using Ats.Data.EntityFramework;
using Ats.Domain.Store;
using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Store
{
    public class ProductOrderRepository : Repository<ProductOrder, Guid>, IProductOrderRepository
    {
        public ProductOrderRepository(SCDataContext context) : base(context)
        {
        }
    }
}
