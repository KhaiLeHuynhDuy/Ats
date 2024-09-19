using Ats.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("preferredproduct")]
    public class PreferredProduct : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public int? ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual Store.Models.Product Product { get; set; }

        public DateTime? ActiveFrom { get; set; } 
        public DateTime? ActiveEnd { get; set; } 
    }
}
