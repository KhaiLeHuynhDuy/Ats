using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Commons
{
    public class SmtpConfig
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string DisplayName { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
