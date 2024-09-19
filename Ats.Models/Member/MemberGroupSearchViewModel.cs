using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberGroupSearchViewModel : BaseSearchViewModel
    {
        public List<MemberGroupViewModel> MemberGroupViewModels { get; set; }
        public MemberGroupViewModel MemberGroupViewModel { get; set; }
    }
}
