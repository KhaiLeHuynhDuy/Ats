using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("membertagvalues")]
    public class MemberTagValue : AuditBase, IEntity<System.Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MemberTagId { get; set; }
        [ForeignKey("MemberTagId")]
        public virtual MemberTag MemberTag { get; set; }

        public int TagKeyId { get; set; }
        [ForeignKey("TagKeyId")]
        public virtual TagKey TagKey { get; set; }

        public string Values { get; set; }
        public DataType? DataType { get; set; }

    }
}
