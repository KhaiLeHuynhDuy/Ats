using Ats.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Member.Models
{
    [Table("preferredbrand")]
    public class PreferredBrand : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        //public int? BrandId { get; set; }
        //[ForeignKey("BrandId")]
        //public virtual Ats.Domain.Brand.Models.Brand Brand { get; set; }

        public DateTime? ActiveFrom { get; set; }   
        public DateTime? ActiveEnd { get; set; }


        
    }
}
