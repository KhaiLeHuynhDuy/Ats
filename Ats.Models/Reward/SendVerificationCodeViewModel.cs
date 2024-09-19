using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Reward
{
    public enum ReceiveCodeViaEnum
    {
        Email,
        Mobile,
    }
    public class SendVerificationCodeViewModel
    {
        public Guid MemberId { get; set; }
        public ReceiveCodeViaEnum ReceiveCodeVia { get; set; } //Email/Mobile 
    }
}
