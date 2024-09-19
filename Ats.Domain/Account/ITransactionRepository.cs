using Ats.Domain.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Account
{
    public interface ITransactionRepository : IRepository<Transaction, int>
    {
    }
}
