using Ats.Domain.Store.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("memberproductqrcodes")]
   public class MemberProductQrCode : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public string ProductCode { get; set; }

    }
}
