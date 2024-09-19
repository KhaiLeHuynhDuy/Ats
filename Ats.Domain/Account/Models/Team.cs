using Ats.Domain.Lead.Models;
using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Accounts.Models
{
    [Table("teams")]
    public class Team : AuditBase, IEntity<Guid>
    {
        public Team()
        {
            TeamUsers = new HashSet<TeamUser>();
            LeadTeams = new HashSet<LeadTeam>();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public virtual ICollection<TeamUser> TeamUsers { get; set; }
        public virtual ICollection<LeadTeam> LeadTeams { get; set; }
    }
}
