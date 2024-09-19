using Ats.Domain.Lead.Models;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberTagCategoryViewModel 
    {
        public int Id { get; set; }
        public string TagCategoryName { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }



    }
}
