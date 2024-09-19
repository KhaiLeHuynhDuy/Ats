using Ats.Domain;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Lead.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ats.Domain.Identity.Models
{
    [Table("users")]
    public class User : IdentityUser<Guid>, IEntity<Guid>
    {
        public User()
        {
            UserGroups = new HashSet<UserGroup>();
            UserRoles = new HashSet<UserRole>();
            TeamUsers = new HashSet<TeamUser>();
            Wallets = new HashSet<Wallet>();
        }

        [Required]
        [MinLength(8, ErrorMessage = "Username must has least minimum is 8 charated")]
        public override string UserName { get; set; }

        public USER_TYPE UserType { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }
        public bool IsActive { get; set; }

        public string PhotoUrl { set; get; }
        public string Title { get; set; }

        [MaxLength(2)]
        public string Lang { get; set; }
        [MaxLength(2)]
        public string CountryCode { get; set; }
        [MaxLength(8)]
        public string TimezoneCode { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Wallet> Wallets { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<TeamUser> TeamUsers { get; set; }
    }
}
