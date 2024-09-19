using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Member;

namespace Ats.Data.Repositories.Member
{
    public class MemberGroupRepository : Repository<Ats.Domain.Member.Models.MemberGroup, Guid>, IMemberGroupRepository
    {
        public MemberGroupRepository(SCDataContext context) : base(context)
        {
        }
    }
}
