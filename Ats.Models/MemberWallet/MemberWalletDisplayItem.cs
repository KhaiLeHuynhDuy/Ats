using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.MemberWallet
{
   public class MemberWalletDisplayItem
    {
        public string MemberCode { get; set; }
        public string FullName { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Selected { get; set; }
        public string Value { get; set; }
    }
}
