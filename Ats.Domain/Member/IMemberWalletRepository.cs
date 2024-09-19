using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Member
{
    public interface IMemberWalletRepository : IRepository<MemberWallet,Guid>
    {
        MemberWallet GetWallet(string email);

        //List<MemberWallet> mbWallet(Guid WalletId);check data walletMember
    }
}
