using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loyalty
{
   public interface ILoyaltyTierRepository : IRepository<Models.LoyaltyTier,int>
    {
        int GetLowestLoyaltyTierLevel();
    }
}
