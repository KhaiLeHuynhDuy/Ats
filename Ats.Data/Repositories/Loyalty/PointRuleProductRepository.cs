using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;

namespace Ats.Data.Repositories.Loyalty
{

    public class PointRuleProductRepository : Repository<Ats.Domain.Loyalty.Models.PointRuleProduct, Guid>, IPointRuleProductRepository
    {
        public PointRuleProductRepository(SCDataContext context) : base(context)
        {

        }

        public bool CheckExistAddProducts(Guid? ruleId, int? productId)
        {
            return GetAll().Any(x => x.LoyaltyPointRuleId.Equals(ruleId) && x.ProductId.Equals(productId));
        }
    }
}
