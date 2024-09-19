using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ats.Domain;
using Ats.Domain.Lead.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ats.Models.Member
{
    public class MemberRecruitmentViewModel
    {
        public Guid Id { get; set; }
        public string MemberCode { get; set; }
        public bool MemberCodeAutomatically { get; set; }


        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime? DOB { get; set; } // day of birth
        public int YOB { get; set; } // year of birth

        public int? JobTitleId { get; set; }
        public int? OccupationId { get; set; }

        public MARITAL_STATUS? MaritalStatus { get; set; }
        public DateTime? WeddingDate { get; set; }

        public string Address1 { get; set; }
        public int? CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }

        public bool AcceptToReceiveMarketing { get; set; }
        public bool AcceptToReceiveLoyaltyInformation { get; set; }
        public bool AcceptToBeContactedViaMobilePhone { get; set; }
        public bool AcceptToBeContactedViaMobileEmail { get; set; }
        public bool NotAcceptAnyContact { get; set; }

        public bool AcceptTermOfService { get; set; }

        public string RegisterDate { get; set; }
        public string RegisterEmployee { get; set; }

        public bool noti { get; set; }
        public string Remark { get; set; }
        public IEnumerable<SelectListItem> Year { get; set; }

    }
}
