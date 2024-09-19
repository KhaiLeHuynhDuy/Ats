using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ats.Domain.Identity.Models;
using static Ats.Commons.Constants;

namespace Ats.Domain.Accounts.Models
{
    [Table("profiles")]
    public class Profile : IEntity<System.Guid>
    {
        public Profile()
        {
        }

        [Key]
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string PersonalId { get; set; }
        public DateTime? DOB { get; set; }
        public int YOB { get; set; }  // Year Of Birth

        public Boolean Gender { get; set; }
        public string PhotoUrl { get; set; }
        public string AdditionalPhone { get; set; }
        public string AdditionalEmail { get; set; }

        public Guid? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address.Models.Address Address { get; set; }

        [MaxLength(2)]
        public string Lang { get; set; }
        [MaxLength(2)]
        public string CountryCode { get; set; }
        [MaxLength(8)]
        public string TimezoneCode { get; set; }      
        public PROFILE_TYPE ProfileType { get; set; }
        //public string PersonalIdRegisteredPlace { get; set; }
        public DateTime? PersonalIdRegisteredDate { get; set; }
        public int? PersonalIdRegisterPlace { get; set; }
        public Guid? AdditionalAddressId { get; set; }
        [ForeignKey("AdditionalAddressId")]
        public virtual Address.Models.Address AdditionalAddress { get; set; }
       
        public virtual Lead.Models.Lead Lead { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
