using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;

namespace Ats.Data.Repositories.Loyalty
{
    public class PointRuleBrandRepository : Repository<Ats.Domain.Loyalty.Models.PointRuleBrand, Guid>
    {
        public PointRuleBrandRepository(SCDataContext context) : base(context)
        {
        }

        //public bool CheckExistAddBrands(Guid? ruleId, int? brandId)
        //{
        //    return GetAll().Any(x => x.LoyaltyPointRuleId.Equals(ruleId) && x.BrandId.Equals(brandId));
        //}
    }
}
