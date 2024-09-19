using Ats.Data.EntityFramework;
using Ats.Domain.Member;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Member
{
    class LoyaltyPointRepository : Repository<LoyaltyPoint, Guid>, ILoyaltyPointRepository
    {
        public LoyaltyPointRepository(SCDataContext context) : base(context)
        {

        }
    }
}
