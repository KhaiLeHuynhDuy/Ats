using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ats.Domain;
using Ats.Domain.Lead.Models;

namespace Ats.Models.Member
{
    public class ToTalMemberOfMonthViewModel
    { 
        public int TotalMember { get; set; }
        public int TotalMemberInactive { get; set; } 
        public ToTalMemberOfMonthViewModel()
        {
            TotalMember = 0;
            TotalMemberInactive = 0; 
        } 
    }
}
