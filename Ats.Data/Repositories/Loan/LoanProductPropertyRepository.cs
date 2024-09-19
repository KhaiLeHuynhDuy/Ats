using Ats.Data.EntityFramework;
using Ats.Domain.Loan;
using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Loan
{
    public class LoanProductPropertyRepository : Repository<LoanProductProperty, int>, ILoanProductPropertyRepository
    {
        public LoanProductPropertyRepository(SCDataContext context) : base(context)
        {
        }
    }
}
