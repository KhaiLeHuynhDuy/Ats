using Ats.Data.EntityFramework;
using Ats.Domain.Loan;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Loan
{
    public class LoanRepository : Repository<Domain.Loan.Models.Loan, Guid>, ILoanRepository
    {
        public LoanRepository(SCDataContext context) : base(context)
        {
        }
    }
}
