using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ats.Api.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Passwod is required.")]
        public string Password { get; set; }
    }
}
