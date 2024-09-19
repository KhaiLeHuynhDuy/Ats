using Ats.Domain;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
  public class MemberSearchView
    {
      public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public bool Gender { get; set; }
        public string RegisterEmployee { get; set; }

        public int? StoreId { get; set; }
        public int? ChannelId { get; set; }

        public DateTime? DOB { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? WeddingDate { get; set; }

        public MARITAL_STATUS? MaritalStatus { get; set; }

    }
}
