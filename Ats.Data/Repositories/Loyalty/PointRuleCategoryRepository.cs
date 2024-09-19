using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;

namespace Ats.Data.Repositories.Loyalty
{
    public class PointRuleCategoryRepository : Repository<Ats.Domain.Loyalty.Models.PointRuleCategory, Guid>, IPointRuleCategoryRepository
    { 
        public PointRuleCategoryRepository(SCDataContext context) : base(context)
        {
        }

        public bool CheckExistAddCategories(Guid? ruleId, int? cateId)
        {
            return GetAll().Any(x => x.LoyaltyPointRuleId.Equals(ruleId) && x.ProductCateId.Equals(cateId));
        }
    }
}
