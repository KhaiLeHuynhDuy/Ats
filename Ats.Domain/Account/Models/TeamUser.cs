using Ats.Domain.Account.Models;
using Ats.Domain.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Accounts.Models
{
    [Table("teamusers")]
    public class TeamUser : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public Guid TeamId { get; set; }
        [ForeignKey("TeamId")]
        public virtual Team Team { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public TEAM_ROLE TeamRole { get; set; }

    }
}
