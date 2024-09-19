using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Domain.Lead.Models
{
    public enum LEAD_IMPORT
    {
        [Display(Name = "--- Select ---")]
        SELECT = 0,
        [Display(Name = "First name")]
        FIRSTNAME = 1,
        [Display(Name = "Last name")]
        LASTNAME = 2,
        [Display(Name = "CMND")]
        CMND = 3,
        [Display(Name = "Phone")]
        PHONE = 4,
        [Display(Name = "Province")]
        PROVINCE = 5,
        [Display(Name = "Công ty")]
        COMPANY = 6
    }
}
