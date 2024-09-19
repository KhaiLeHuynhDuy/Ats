using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("loanproductproperties")]
    public class LoanProductProperty : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public DATA_TYPE DataType { get; set; }

        public int? ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public virtual LoanProductCategory ProductCategory { get; set; }

        public string Description { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
    }
}
