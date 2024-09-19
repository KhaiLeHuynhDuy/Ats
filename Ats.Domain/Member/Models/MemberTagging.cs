using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("membertaggings")]
    public class MemberTagging 
    {
        public MemberTagging()
        {
        }

        public Guid MemberTagId { get; set; }
        [ForeignKey("MemberTagId")]
        public virtual MemberTag MemberTag { get; set; }

        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
