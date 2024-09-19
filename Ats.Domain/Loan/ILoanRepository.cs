using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loan
{
    public interface ILoanRepository : IRepository<Models.Loan, Guid>
    {
    }
}
