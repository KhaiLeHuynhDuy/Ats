using Ats.Domain;
using Ats.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Models.Account
{
    public class GroupViewModel
    {
        public GroupViewModel()
        {
            Roles = new HashSet<RoleViewModel>();
        }

        [Key]
        public Guid Id { get; set; }

        [StringLength(128, ErrorMessage = "Name cannot be longer than 128 characters.")]
        [Required]
        public string Name { get; set; }

        [StringLength(128, ErrorMessage = "Description cannot be longer than 128 characters.")]
        public string Description { get; set; }

        public ICollection<RoleViewModel> Roles { get; set; }
        public ICollection<DisplayItem> ListRoles { get; set; }
        public ICollection<string> SelectedRoles { get; set; }
    }
}
