using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("memberlifecycles")]
    public class MemberLifeCycle : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string MemberLifeCycleName { get; set; }
        public MEMBER_LIFECYCLE Lifecycle { get; set; }
        public int DisplayOrder { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }
        public double LastRegisterMonthDuration { get; set; }
        public int TotalTransaction { get; set; }

        //public virtual ICollection<Member> Members { get; set; }
        public virtual List<MemberLifeCycleLink> MemberLifeCycleLinks { get; set; }
    }
}
