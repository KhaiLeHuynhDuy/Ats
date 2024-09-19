using Ats.Domain.Member.Models;
using Ats.Models.Account;
using Ats.Models.MemberWallet;
using Ats.Models.MemberWallets;
using Ats.Models.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface IMemberWalletService
    {
        List<MemberWalletHistoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<MemberWalletHistoryViewModel> GetMemberWalletHistory(Guid Id, string keyword, string orderField, bool IsAscOrder, int page, int size, out int total);
        MemberWalletsViewModel GetMemberWallet(Guid IdMember);
        Guid CreateMemberWallet(MemberWalletsViewModel model);
        int UpdateMemberWallet(MemberWalletsViewModel model);
        void DeleteMemberWallet(Guid id);
        List<MemberWalletsViewModel> GetMemberWallets();
        int GetMemberWalletsInactiveOfYear(string year);
        List<TransactionViewModel> GetTransactions(Guid walletId, int page, int size, out int total);
        void Transfer(string emailFrom, string emailTo, double amount);

        List<MemberWalletDisplayItem> SearchMemberSend(string searchText, int page, int size);
        //List<MemberWallet> mbWallet(Guid WalletId);check data walletMember

    }
}
