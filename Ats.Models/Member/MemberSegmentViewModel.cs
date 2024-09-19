using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
   public class MemberSegmentViewModel
    {
        public MemberSegmentViewModel()
        {            
        }

        public Guid Id { get; set; }
        public string SegmentName { get; set; }
        public Boolean Conditional { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }
        public int TotalMembers { get; set; }

    }
}
