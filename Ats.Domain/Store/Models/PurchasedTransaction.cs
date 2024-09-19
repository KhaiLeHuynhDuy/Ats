using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Store.Models
{
    [Table("purchasedtransactions")]
    public class PurchasedTransaction : AuditBase, IEntity<Guid>
    {
        public PurchasedTransaction()
        {
        }
            
        [Key]
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public string RefId { get; set; }

        public double Amount { get; set; }
        public DateTime PurchasedDate { get; set; }
        
        public string Currency { get; set; }
        public string Remark { get; set; }

        public string MemberReference { get; set; }
        public Guid? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member.Models.Member Member { get; set; }

        public int? ChannelId { get; set; }
        [ForeignKey("ChannelId")]
        public virtual Channel.Models.MemberChannel Channel { get; set; }

        public int? StoreId { get; set; }
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }

        public DateTime? ProcessDate { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
    }
}
