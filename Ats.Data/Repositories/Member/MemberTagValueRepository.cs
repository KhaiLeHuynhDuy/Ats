using Ats.Data.EntityFramework;
using Ats.Domain.Member;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.TagMember
{
    public class MemberTagValueRepository : Repository<MemberTagValue, Guid>, IMemberTagValueRepository
    {
        public MemberTagValueRepository(SCDataContext context) : base(context)
        {

        }
    }
}
