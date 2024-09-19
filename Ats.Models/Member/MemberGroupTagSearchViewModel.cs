using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
   public class MemberGroupTagSearchViewModel : BaseSearchViewModel
    {
        public List<MemberGroupTagViewModel> MemberGroupTagViewModels { get; set; }
        public MemberGroupTagViewModel MemberGroupTagViewModel { get; set; }
    }
}
