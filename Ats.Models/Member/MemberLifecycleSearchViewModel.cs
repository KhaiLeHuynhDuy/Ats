using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberLifecycleSearchViewModel : BaseSearchViewModel
    {
        public List<MemberLifecycleViewModel> MemberLifecycles { get; set; }
        public MemberLifecycleViewModel MemberLifecycle { get; set; }
    }
}
