using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Loan.Models
{
    [Table("loanproductattributes")]
    public class LoanProductAttribute : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual LoanProduct Product { get; set; }
        public int ProductPropertyId { get; set; }
        [ForeignKey("ProductPropertyId")]
        public virtual LoanProductProperty ProductProperty { get; set; }
        public DATA_TYPE DataType { get; set; }
        public string Value { get; set; }
    }
}
