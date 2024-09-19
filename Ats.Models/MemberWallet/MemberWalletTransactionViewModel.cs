using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models.MemberWallets;
using static Ats.Commons.Constants;

namespace Ats.Models.Account
{
    public class MemberWalletTransactionViewModel : BaseSearchViewModel
    {
        public List<TransactionViewModel> Transations { get; set; }
        public MemberWalletsViewModel MemberWallet { get; set; }
    }
}
