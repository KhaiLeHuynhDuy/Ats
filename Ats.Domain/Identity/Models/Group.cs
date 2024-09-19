using Ats.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Domain.Identity.Models
{
    [Table("rolegroups")]
    public class Group : IEntity<Guid>
    {
        public Group()
        {
            GroupRoles = new HashSet<GroupRole>();
            UserGroups = new HashSet<UserGroup>();
        }

        [Key]
        public Guid Id { get; set; }

        [StringLength(500, ErrorMessage = "Name cannot be longer than 500 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
        public virtual ICollection<GroupRole> GroupRoles { set; get;}
        public virtual ICollection<UserGroup> UserGroups { set; get;}
    }
}
