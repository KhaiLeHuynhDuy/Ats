using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
   public class MemberGroupViewModel
    {
        public MemberGroupViewModel()
        {
            Tags = new List<DisplayItem>();
        }

        public Guid Id { get; set; }
        public string MemberGroupName { get; set; }
        public Boolean Conditional { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }
        public int TotalMembers { get; set; }

        public IList<DisplayItem> Tags { get; set; }
    }
}
