using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Member.Models;
using Ats.Models.Member;

namespace Ats.Services.Interfaces
{
   public interface IMemberGroupTagService
    {
        List<MemberGroupTagViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        MemberGroupTagViewModel Get(Guid id);
        List<MemberGroupTagViewModel> Gets();
        Guid Create(MemberGroupTagSearchViewModel model);
        void Update(MemberGroupTagViewModel model);
        void Delete(Guid id);
        List<MemberGroup> GetMemberGroup();
        List<MemberTag> GetMemberTag();
    }
}
