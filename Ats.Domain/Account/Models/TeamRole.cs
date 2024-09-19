using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Domain.Account.Models
{
    public enum TEAM_ROLE
    {
        [Display(Name = "Undefined")]
        UNDEFINED = 0,
        [Display(Name = "Manager")]
        MANAGER = 1,
        [Display(Name = "Supervisor")]
        SUPERVISOR = 2,
        [Display(Name = "Leader")]
        LEADER = 3,
        [Display(Name = "Sale")]
        SALE = 4,
        [Display(Name = "Partner")]
        PARTNER = 5,
        [Display(Name = "Agency")]
        AGENCY = 6,
        [Display(Name = "Employee")]
        EMPLOYEE = 7,
    }
}
