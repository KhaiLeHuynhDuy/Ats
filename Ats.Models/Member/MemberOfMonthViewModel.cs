using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ats.Domain;
using Ats.Domain.Lead.Models;

namespace Ats.Models.Member
{
    public class MemberOfMonthViewModel
    {
        public string YearSelect { get; set; }
        public int Total { get; set; }
        public int TotalMemberInactive { get; set; }
        public int totalmemberinmonths { get; set; }
        public List<string> Data { get; set; }
        public MemberOfMonthViewModel()
        {
            Total = 0;
            Data = new List<string>();
            YearSelect = " ";
        } 
    }
}
