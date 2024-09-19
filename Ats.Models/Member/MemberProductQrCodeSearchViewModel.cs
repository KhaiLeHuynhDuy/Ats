using Ats.Domain;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.Member
{
    public class MemberProductQrCodeSearchViewModel : BaseSearchViewModel
    {
        public List<MemberProductQrCodeViewModel> MemberProductQrCodes { get; set; }
        public MemberProductQrCodeViewModel MemberProductQrCode { get; set; } 
    }
}
