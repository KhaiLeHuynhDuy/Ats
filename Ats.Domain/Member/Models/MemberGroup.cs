using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain.Member.Models
{
    [Table("membergroups")]
    public class MemberGroup : AuditBase, IEntity<Guid>
    {
        public MemberGroup()
        {
            MemberGroupTags = new HashSet<MemberGroupTag>();
        }

        [Key]
        public Guid Id { get; set; }
        public string MemberGroupName { get; set; }
        public Boolean Conditional { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }

        public virtual ICollection<MemberGroupTag> MemberGroupTags { get; set; }

        //public virtual ICollection<Member> Members { get; set; }
        public virtual List<MemberGroupLink> MemberGroupLinks { get; set; }
    }
}
