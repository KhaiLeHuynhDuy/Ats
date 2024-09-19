using Ats.Data.EntityFramework;
using Ats.Domain.Loan;
using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Loan
{
    public class BorrowerRepository : Repository<Borrower, Guid>, IBorrowerRepository
    {
        public BorrowerRepository(SCDataContext context) : base(context)
        {
        }
        public int GetMaxBorrowerId()
        {
            try
            {
                return this.GetAll().Max(x => x.BorrowerId);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
