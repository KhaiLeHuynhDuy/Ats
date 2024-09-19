using Ats.Domain;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberSearchViewModel : BaseSearchViewModel
    {
        public List<MemberViewModel> Members { get; set; }
        public MemberViewModel Member { get; set; }
     
        public int? OccupationId { get; set; }
        public int? ProvinceId { get; set; }
        public int? JobTitleId { get; set; }
        public int? DistrictId { get; set; }
        public int? StoreId { get; set; }
        public int? ChannelId { get; set; }
        public bool? Valid { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DOB { get; set; }
        public MARITAL_STATUS? MaritalStatus { get; set; }
        public DateTime? WeddingDate { get; set; }

        public bool noti { get; set; }
        public string DeleteAllInactive { get; set; }

    }
}
