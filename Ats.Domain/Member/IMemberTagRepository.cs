using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Member.Models;

namespace Ats.Domain.Member
{
    public interface IMemberTagRepository : IRepository<MemberTag, Guid>
    {
        void UpdateMemberTagging(Guid memberTagId, List<Guid> memberIds);
        IEnumerable<MemberTag> GetMemberTagLabels(Guid memberId);
    }
}
