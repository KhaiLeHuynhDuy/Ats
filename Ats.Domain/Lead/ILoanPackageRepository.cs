using Ats.Domain.Lead.Models;
using Ats.Domain.Organization.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Domain.Lead
{
    public interface ILoanPackageRepository : IRepository<LoanPackage, Int32>
    {
    }
}
