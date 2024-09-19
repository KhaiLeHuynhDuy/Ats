using Ats.Data.EntityFramework;
using Ats.Domain.Lead;
using Ats.Domain.Lead.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Lead
{
    public class LoanBookRepository : Repository<LoanBook, Guid>, ILoanBookRepository
    {
        public LoanBookRepository(SCDataContext context) : base(context)
        {
        }
    }
}
