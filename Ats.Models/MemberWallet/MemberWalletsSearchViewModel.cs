using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models.Account;
using Ats.Models.MemberWallets;

namespace Ats.Models.MemberWallet
{
   public class MemberWalletsSearchViewModel : BaseSearchViewModel
    {
        //public MemberWalletsSearchViewModel()
        //{
        //    members = new List<MemberWalletDisplayItem>();
        //}
        public List<MemberWalletsViewModel> MemberWallets { get; set; }
        public MemberWalletsViewModel MemberWallet { get; set; }
        public List<MemberLoyaltyTransactionsViewModel> MemberLoyaltys { get; set; }
        public MemberLoyaltyTransactionsViewModel MemberLoyalty { get; set; }

        public List<MemberWalletHistoryViewModel> MemberWalletsHistory{ get; set; }
        public MemberWalletHistoryViewModel MemberWalletsHistorys{ get; set; }

        public WalletTransferViewModel Transfer { get; set; }

        public List<MemberWalletDisplayItem> members { get; set; }
        public MemberWalletDisplayItem memberWallet { get; set; }
    }
}
