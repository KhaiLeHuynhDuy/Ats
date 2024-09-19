using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ats.Commons.Resource;


namespace Ats.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        //[Display(Name = "CurPW", ResourceType = typeof(Resource))]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must contain at least 6 characters, including at least 1 number and includes both lower and uppercase letters and special characters")]
        [DataType(DataType.Password)]
        //[Display(Name = "NewPW", ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        //[Display(Name = "ConfirmNewPW", ResourceType = typeof(Resource))]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password must contain at least 6 characters, including at least 1 number and includes both lower and uppercase letters and special characters")]
        [Compare("NewPassword", ErrorMessageResourceName = "ErrorConfirmPW", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }
}
