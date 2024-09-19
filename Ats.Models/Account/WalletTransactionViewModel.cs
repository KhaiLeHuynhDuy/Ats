using System;
using System.Collections.Generic;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Models.Account
{
    public class WalletTransactionViewModel : BaseSearchViewModel
    {
        public List<TransactionViewModel> Transations { get; set; }
        public WalletViewModel Wallet { get; set; }
    }
}
