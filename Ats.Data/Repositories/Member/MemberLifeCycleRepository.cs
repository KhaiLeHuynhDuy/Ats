using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Member;

namespace Ats.Data.Repositories.Member
{
    public class MemberLifeCycleRepository : Repository<Ats.Domain.Member.Models.MemberLifeCycle, int>, IMemberLifeCycleRepository
    {
        public MemberLifeCycleRepository(SCDataContext context) : base(context)
        {
        }
    }
}
