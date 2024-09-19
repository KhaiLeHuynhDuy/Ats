using System.ComponentModel.DataAnnotations;

namespace Ats.Security.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}