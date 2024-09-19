using Ats.Domain;
using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Ats.Domain.Member.Models
{
    [Table("memberwallets")]
    public class MemberWallet : AuditBase, IEntity<System.Guid>
    {
        public MemberWallet()
        {
            MemberLoyaltyTransactions = new HashSet<MemberLoyaltyTransaction>();
        }
        [Key]
        public Guid Id { get; set; }      
        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public double Point { get; set; }
        public bool Active { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveEnd { get; set; }
        public string Remark { get; set; }
      

        public virtual ICollection<MemberLoyaltyTransaction> MemberLoyaltyTransactions { get; set; }
    }
}
