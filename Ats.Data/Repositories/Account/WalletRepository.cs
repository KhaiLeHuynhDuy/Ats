using Ats.Data.EntityFramework;
using Ats.Domain.Account;
using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ats.Data.Repositories.Account
{
    public class WalletRepository : Repository<Wallet, Guid>, IWalletRepository
    {
        public WalletRepository(SCDataContext context) : base(context)
        {
        }

        public Wallet GetWallet(string email)
        {
            var query = (from user in DataContext.User
                         join wallet in DataContext.Wallet on user.Id equals wallet.UserId
                         where String.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase)
                         select wallet).AsEnumerable();

            return query.FirstOrDefault();
        }
    }
}
