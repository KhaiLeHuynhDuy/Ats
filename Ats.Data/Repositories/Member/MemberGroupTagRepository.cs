using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Member;

namespace Ats.Data.Repositories.Member
{
    public class MemberGroupTagRepository : Repository<Ats.Domain.Member.Models.MemberGroupTag, Guid>, IMemberGroupTagRepository
    {
        public MemberGroupTagRepository(SCDataContext context) : base(context)
        {
        }
    }
}
