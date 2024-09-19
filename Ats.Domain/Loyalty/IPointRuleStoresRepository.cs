using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loyalty
{
   public interface IPointRuleStoresRepository : IRepository<Models.PointRuleStore,Guid>
    {
        bool CheckExistAddStores(Guid? ruleId, int storeId);

    }
}
