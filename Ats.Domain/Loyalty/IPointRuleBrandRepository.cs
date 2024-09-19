using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loyalty
{
   public interface IPointRuleBrandRepository : IRepository<Models.PointRuleBrand,Guid>
    {
        bool CheckExistAddBrands(Guid? ruleId, int? brandId);
    }
}
