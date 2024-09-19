using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loyalty
{
   public interface IPointRuleCategoryRepository : IRepository<Models.PointRuleCategory,Guid>
    {
        bool CheckExistAddCategories(Guid? ruleId, int? cateId);
    }
}
