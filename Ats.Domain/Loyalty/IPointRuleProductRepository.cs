using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loyalty
{
   public interface IPointRuleProductRepository :IRepository<Models.PointRuleProduct, Guid>
    {
        bool CheckExistAddProducts(Guid? ruleId, int? productId);
    }
}
