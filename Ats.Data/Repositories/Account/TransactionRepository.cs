using Ats.Data.EntityFramework;
using Ats.Domain.Account;
using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Account
{
    public class TransactionRepository : Repository<Transaction, int>, ITransactionRepository
    {
        public TransactionRepository(SCDataContext context) : base(context)
        {
        }
    }
}
