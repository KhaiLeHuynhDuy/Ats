using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Store
{
    public interface IPurchasedTransactionRepository : IRepository<Models.PurchasedTransaction,Guid>
    {
    }
}
