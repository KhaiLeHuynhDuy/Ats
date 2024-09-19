using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Models.MemberWallet
{
   public class SearchSendMemberWalletViewModel : BaseSearchViewModel
    {
        public SearchSendMemberWalletViewModel()
        {
            members = new List<MemberWalletDisplayItem>();
        }
        public Guid Id { get; set; }
        public string MemberCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public IList<MemberWalletDisplayItem> members { get; set; }
        public MemberWalletDisplayItem memberWallet { get; set; }
    }
}
