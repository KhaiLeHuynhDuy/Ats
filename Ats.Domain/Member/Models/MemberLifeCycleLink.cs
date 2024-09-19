using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("memberlifecyclelinks")]
    public class MemberLifeCycleLink
    {
        public MemberLifeCycleLink()
        {
        }

        public int MemberLifeCycleId { get; set; }
        [ForeignKey("MemberLifeCycleId")]
        public virtual MemberLifeCycle MemberLifeCycle { get; set; }

        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
