using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Member.Models;
using Ats.Models.Member;

namespace Ats.Services.Interfaces
{
    public interface IMemberGroupService
    {
        List<MemberGroupViewModel> Search(string searchText, int? MemberGroupId, int? MemberTagId, string orderField, bool IsAscOrder, int page, int size, out int total);
        MemberGroupViewModel GetMemberGroup(Guid id);
        Guid Create(MemberGroupViewModel model);
        void Update(MemberGroupViewModel model);
        void Delete(Guid id);
        List<MemberGroupViewModel> GetMemberGroups();
        List<MemberTag> GetMemberTag();
    }
}
