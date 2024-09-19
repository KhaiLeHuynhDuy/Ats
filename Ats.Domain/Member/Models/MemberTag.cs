using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("membertags")]
    public class MemberTag : AuditBase, IEntity<System.Guid>
    {
        public MemberTag()
        {
            MemberTagValues  = new HashSet<MemberTagValue>();
            MemberTaggings = new HashSet<MemberTagging>();

        }

        [Key]
        public Guid Id { get; set; }
        public int TagCagetoryId { get; set; }
        [ForeignKey("TagCagetoryId")]
        public virtual MemberTagCategory MemberTagCategory { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }
        public string TagName { get; set; }
        public bool TagType { get; set; } // Non-conditional vs Conditional

        public int TotalMember { get; set; }
        public DateTime? LastUpdate { get; set; }

        public virtual ICollection<MemberTagValue> MemberTagValues { get; set; }
        public virtual ICollection<MemberTagging> MemberTaggings { get; set; }


    }
}
