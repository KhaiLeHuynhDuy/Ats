using Ats.Data.EntityFramework;
using Ats.Domain.Store;
using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Store
{
    public class ProductAttributeRepository : Repository<ProductAttribute, int>, IProductAttributeRepository
    {
        public ProductAttributeRepository(SCDataContext context) : base(context)
        {
        }
        public bool CheckExistAddProductAttribute(int productId, int productPropertyId)
        {
            return GetAll().Any(x => x.ProductId.Equals(productId) && x.ProductPropertyId.Equals(productPropertyId));
        }
    }
}
