using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("loanproductcats")]
    public class LoanProductCategory : AuditBase, IEntity<int>
    {
        public LoanProductCategory()
        {
            ProductProperties = new HashSet<LoanProductProperty>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public virtual ICollection<LoanProductProperty> ProductProperties { get; set; }
    }
}
