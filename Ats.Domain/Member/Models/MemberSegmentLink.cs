using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("membersegmentlinks")]
    public class MemberSegmentLink
    {
        public MemberSegmentLink()
        {
        }

        public Guid MemberSegmentId { get; set; }
        [ForeignKey("MemberSegmentId")]
        public virtual MemberSegment MemberSegment { get; set; }

        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
