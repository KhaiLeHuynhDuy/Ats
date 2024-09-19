using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain.Member.Models
{
    [Table("membersegments")]
    public class MemberSegment : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public string MemberSegmentName { get; set; }
        public Boolean Conditional { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }

        //public virtual ICollection<Member> Members { get; set; }
        public virtual List<MemberSegmentLink> MemberSegmentLinks { get; set; }

    }
}
