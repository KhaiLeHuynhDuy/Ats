using Ats.Data.EntityFramework;
using Ats.Domain.Member;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Member
{
    public class MemberTagRepository : Repository<MemberTag, Guid>, IMemberTagRepository
    {
        public MemberTagRepository(SCDataContext context) : base(context)
        {

        }

        public void UpdateMemberTagging(Guid memberTagId, List<Guid> memberIds)
        {
            var entity = this.GetById(memberTagId);

            entity.TotalMember = memberIds.Count;
            entity.LastUpdate = DateTime.UtcNow;
            this.Update(entity, x => x.TotalMember, x => x.LastUpdate);


            // existing members
            List<Guid> existingMembers = this.DataContext.Get<MemberTagging>().Where(x => x.MemberTagId == memberTagId).Select(x => x.MemberId).ToList();
            List<Guid> overlap = memberIds.Intersect(existingMembers).ToList();

            // Adding
            List<Guid> newMembers = memberIds.Where(x => !overlap.Contains(x)).ToList();
            foreach (Guid memberId in newMembers)
            {
                MemberTagging tag = new MemberTagging()
                {
                    MemberId = memberId,
                    MemberTagId = memberTagId,
                    CreatedDate = DateTime.UtcNow
                };

                this.DataContext.Set<MemberTagging>().Add(tag);
            }

            // Deleting
            List<Guid> deletingMembers = existingMembers.Where(x => !overlap.Contains(x)).ToList();
            List< MemberTagging> deletetingTags = this.DataContext.Get<MemberTagging>().Where(x => x.MemberTagId == memberTagId && deletingMembers.Contains(x.MemberId)).ToList();
            
            this.DataContext.Set<MemberTagging>().RemoveRange(deletetingTags);

        }

        public IEnumerable<MemberTag> GetMemberTagLabels(Guid memberId)
        {
            var query = (from tag in DataContext.MemberTag
                         join tagging in DataContext.MemberTagging on tag.Id equals tagging.MemberTagId
                         where tagging.MemberId == memberId
                         select tag).AsEnumerable();
            return query;
        }
    }
}
