using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("membergrouplinks")]
    public class MemberGroupLink
    {
        public MemberGroupLink()
        {
        }

        public Guid MemberGroupId { get; set; }
        [ForeignKey("MemberGroupId")]
        public virtual MemberGroup MemberGroup { get; set; }

        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
