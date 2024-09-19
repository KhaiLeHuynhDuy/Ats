using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Email
{
    public class RegisterAccountModel
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string StoreCode { get; set; }
        public string Password { get; set; }
    }
}
