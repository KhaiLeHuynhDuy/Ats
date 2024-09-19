using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Email
{
    public class ResetPasswordModel
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string UrlResetPassword { get; set; }
    }
}
