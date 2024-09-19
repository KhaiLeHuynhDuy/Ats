using Ats.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ats.Models.Account
{    
    public class RoleViewModel
    {
        public RoleViewModel()
        {
        }

        [Key]
        public Guid Id { get; set; }

        [StringLength(256, ErrorMessage = "Name cannot be longer than 256 characters.")]
        public string Name { get; set; }
    }
}
