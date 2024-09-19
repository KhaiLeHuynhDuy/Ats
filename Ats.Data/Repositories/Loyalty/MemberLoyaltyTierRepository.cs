using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Loyalty;

namespace Ats.Data.Repositories.Loyalty
{
    public class MemberLoyaltyTierRepository : Repository<Ats.Domain.Loyalty.Models.MemberLoyaltyTier, Guid>, IMemberLoyaltyTierRepository
    {
        public MemberLoyaltyTierRepository(SCDataContext context) : base(context)
        {
        }
    }
}
