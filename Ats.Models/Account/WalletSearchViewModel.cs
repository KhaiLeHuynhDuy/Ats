using System;
using System.Collections.Generic;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Models.Account
{
    public class WalletSearchViewModel : BaseSearchViewModel
    {
        public List<WalletViewModel> Wallets { get; set; }
        public WalletViewModel Wallet { get; set; }

        public WalletTransferViewModel Transfer { get; set; }
    }
}
