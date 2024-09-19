using Ats.Domain;
using Ats.Domain.Lead.Models;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberLifecycleViewModel
    { 
        public int Id { get; set; }
        public string MemberLifeCycleName { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }
        public int TotalTransaction { get; set; }
        public double LastRegisterMonthDuration { get; set; }

        public MEMBER_LIFECYCLE Lifecycle { get; set; }
        public int DisplayOrder { get; set; } 
        
 
    }
}
