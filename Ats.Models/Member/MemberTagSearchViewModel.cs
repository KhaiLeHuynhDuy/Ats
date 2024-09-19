using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberTagSearchViewModel : BaseSearchViewModel
    {
        public List<MemberTagViewModel> MemberTags { get; set; }
        public MemberTagViewModel MemberTag { get; set; }
    }
}
