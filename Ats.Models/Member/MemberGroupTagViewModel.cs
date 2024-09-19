using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberGroupTagViewModel
    {
        public Guid Id { get; set; }
        public Guid MemberGroupId { get; set; }
        public string MemberGroupName { get; set; }
        public Guid MemberTagId { get; set; }
        public string MemberTagName { get; set; }
        public string Remark { get; set; }
    }
}
