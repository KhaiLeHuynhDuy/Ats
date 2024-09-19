using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Ats.Domain;

namespace Ats.Domain.Store.Models
{
    [Table("stores")]
   public class Store : AuditBase, IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        
        public Guid? TeamId{ get; set; }
        [ForeignKey("TeamId")]
        public virtual Accounts.Models.Team Team { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Desc { get; set; }
        public Boolean Active { get; set; } = true;
        [Required]
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveEnd { get; set; }
    }
}
