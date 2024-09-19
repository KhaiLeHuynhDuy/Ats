using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;

namespace Ats.Data.Repositories.Loyalty
{
   public class PointRuleStoresRepository : Repository<Ats.Domain.Loyalty.Models.PointRuleStore,Guid>, IPointRuleStoresRepository
    {
        public PointRuleStoresRepository(SCDataContext context) : base(context)
            {
            }
        public bool CheckExistAddStores(Guid? ruleId, int storeId)
        {
            return GetAll().Any(x => x.LoyaltyPointRuleId.Equals(ruleId) && x.StoreId.Equals(storeId));
        }
    }
}

