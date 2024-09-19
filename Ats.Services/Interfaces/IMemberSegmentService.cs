using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models.Member;

namespace Ats.Services.Interfaces
{
    public interface IMemberSegmentService
    {
        List<MemberSegmentViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        MemberSegmentViewModel GetMemberSegment(Guid id);
        Guid Create(MemberSegmentViewModel model); 
        void Update(MemberSegmentViewModel model);
        void Delete(Guid id);
        List<MemberSegmentViewModel> GetMemberSegments();
    }
}
