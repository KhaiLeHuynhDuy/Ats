using Ats.Models.Account;
using Ats.Models.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IWalletService
    {
        Guid CreateWallet(string username);
        List<WalletViewModel> Search(string keyword, string orderField, bool IsAscOrder, int page, int size, out int total);
        WalletViewModel GetWallet(Guid id);
        List<TransactionViewModel> GetTransactions(Guid walletId, int page, int size, out int total);
        void Transfer(string emailFrom, string emailTo, double amount);
        void DeleteWallet(Guid id);
    }
}
