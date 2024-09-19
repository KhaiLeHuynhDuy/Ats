using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Voucher.Models 
{
    [Table("vouchercategories")]
    public class VoucherCategory : AuditBase, IEntity<int>
    {
        public int Id { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherName { get; set; }
        public Boolean Active { get; set; } = true;
     
        public string Desc { get; set; }
    }
}
