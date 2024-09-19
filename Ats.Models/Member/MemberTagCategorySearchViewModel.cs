using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberTagCategorySearchViewModel : BaseSearchViewModel
    {
        public List<MemberTagCategoryViewModel> MemberTagCats { get; set; }
        public MemberTagCategoryViewModel MemberTagCat { get; set; }
    }
}
