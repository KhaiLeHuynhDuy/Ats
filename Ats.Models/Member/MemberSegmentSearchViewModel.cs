using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberSegmentSearchViewModel : BaseSearchViewModel
    {
        public List<MemberSegmentViewModel> Result { get; set; }
        public MemberSegmentViewModel MemberSegmentViewModel { get; set; }
    }
}
