using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ats.Domain.Gift.Models;
using Ats.Domain.Member.Models;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Member;
using Ats.Models.Product;

namespace Ats.Services
{
    public interface IMemberTagService
    {
        List<MemberTagViewModel> Search(string searchText,int? TagCagetoryId, string orderField, bool IsAscOrder, int page, int size, out int total);
        MemberTagViewModel GetMemberTag(Guid id);
        Guid Create(MemberTagViewModel model);
        void Update(MemberTagViewModel model);
        void Delete(Guid id);
        List<MemberTagViewModel> GetMemberTags();
        List<DisplayItem> GetMemberTagsForDisplay(Guid memberId);

        List<MemberTagCategory> GetMembertagCategory();
        List<MemberTag> GetMemberTag();
        int Analyst(Guid id);
    }
}