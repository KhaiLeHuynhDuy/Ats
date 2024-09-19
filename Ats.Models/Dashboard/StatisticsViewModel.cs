using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ats.Domain;
using Ats.Domain.Lead.Models;

namespace Ats.Models.Dashboard
{
    public class StatisticsViewModel
    {
        
        public int NumberOfCodeScans { get; set; } 
        public int NumberOfRedemptions { get; set; } 
        public int NumberOfNewMembers { get; set; } 
        public string DateToDateThisMonth { get; set; } 

        public StatisticsViewModel()
        {
            NumberOfCodeScans = 0;
            NumberOfRedemptions = 0;
            NumberOfNewMembers = 0;
            DateToDateThisMonth = "";  
        } 
    }
}
