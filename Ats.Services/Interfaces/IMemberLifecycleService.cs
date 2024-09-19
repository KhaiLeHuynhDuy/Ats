using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Gift.Models;
using Ats.Domain.Member.Models;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Member;
using Ats.Models.Product;

namespace Ats.Services
{
    public interface IMemberLifecycleService
    {
        List<MemberLifecycleViewModel> Search(string keyword);
        MemberLifecycleViewModel GetMemberLifecycle(int id);
        List<MemberLifecycleViewModel> GetMemberLifecycles();
        int Create(MemberLifecycleViewModel model);
        void Update(MemberLifecycleViewModel model);
        void Update(List<MemberLifecycleViewModel> model);
    }
}