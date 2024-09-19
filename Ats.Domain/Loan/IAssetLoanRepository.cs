using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Loan
{
    public interface IAssetLoanRepository : IRepository<AssetLoan, int>
    {
    }
}
