using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ats.Domain.Organization.Models
{
    [Table("clients")]
    public class Client : AuditBase, IEntity<Guid>
    {
        public Client()
        {
            Teams = new HashSet<Team>();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CLIENT_TYPE ClientType { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
    public enum CLIENT_TYPE
    {
        CORPORATION = 1,
        COMPANY = 2,
        BRANCH = 3,
        INTERNAL_DEPARTMENT = 4,
        BUSINESS = 5,
    }
}
