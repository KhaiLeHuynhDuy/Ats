using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    [Table("loanpackages")]
    public class LoanPackage : AuditBase, IEntity<Int32>
    {
        public LoanPackage()
        {
            LoanBooks = new HashSet<LoanBook>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public virtual ICollection<LoanBook> LoanBooks { get; set; }
    }
}
