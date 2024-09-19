using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("membertagcategories")]
    public class MemberTagCategory : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string TagCategoryName { get; set; }
        public Boolean Active { get; set; }
        public string Remark { get; set; }
    }
}

