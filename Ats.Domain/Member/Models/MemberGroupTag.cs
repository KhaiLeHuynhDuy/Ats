using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("membergrouptags")]
    public class MemberGroupTag : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? MemberGroupId { get; set; }
        [ForeignKey("MemberGroupId")]
        public virtual MemberGroup MemberGroup { get; set; }
        public Guid? MemberTagId { get; set; }
        [ForeignKey("MemberTagId")]
        public virtual MemberTag MemberTag { get; set; }
        public string Remark { get; set; }
    }
}
