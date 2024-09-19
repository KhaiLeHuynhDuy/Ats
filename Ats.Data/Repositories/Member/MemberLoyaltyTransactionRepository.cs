using Ats.Data.EntityFramework;
using Ats.Domain.Member;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Member
{

    class MemberLoyaltyTransactionRepository : Repository<MemberLoyaltyTransaction, Guid>, IMemberLoyaltyTransactionRepository
    {
        public MemberLoyaltyTransactionRepository(SCDataContext context) : base(context)
        {

        }
    }
}
