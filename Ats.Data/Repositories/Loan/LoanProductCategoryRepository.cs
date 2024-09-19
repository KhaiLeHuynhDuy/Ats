using Ats.Data.EntityFramework;
using Ats.Domain.Loan;
using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Loan
{
    public class LoanProductCategoryRepository : Repository<LoanProductCategory, int>, ILoanProductCategoryRepository
    {
        public LoanProductCategoryRepository(SCDataContext context) : base(context)
        {
        }
    }
}
