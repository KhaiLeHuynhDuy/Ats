using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;

namespace Ats.Data.Repositories.Loyalty
{
     public class LoyaltyPointRuleRepository : Repository<Ats.Domain.Loyalty.Models.LoyaltyPointRule, Guid>, ILoyaltyPointRuleRepository
    {
        public LoyaltyPointRuleRepository(SCDataContext context) : base(context)
        {
        }
    }
}

