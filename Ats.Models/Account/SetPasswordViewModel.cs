using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ats.Models.Account
{
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPW", ResourceType = typeof(Ats.Commons.Resource.Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPW", ResourceType = typeof(Ats.Commons.Resource.Resource))]
        [Compare("NewPassword", ErrorMessageResourceName = "ErrorConfirmPW", ErrorMessageResourceType = typeof(Ats.Commons.Resource.Resource))]
        public string ConfirmPassword { get; set; }
    }
}
