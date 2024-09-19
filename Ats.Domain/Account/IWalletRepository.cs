using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Account
{
    public interface IWalletRepository : IRepository<Wallet, Guid>
    {
        Wallet GetWallet(string email);
    }
}
