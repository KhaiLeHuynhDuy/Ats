using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("loanproducts")]
    public class LoanProduct : AuditBase, IEntity<int>
    {
        public LoanProduct()
        {
            ProductAttributes = new HashSet<LoanProductAttribute>();
        }
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }

        public int? ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public virtual LoanProductCategory ProductCategory { get; set; }

        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }

        public double Duration { get; set; }
        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public virtual ICollection<LoanProductAttribute> ProductAttributes { get; set; }
    }
}
