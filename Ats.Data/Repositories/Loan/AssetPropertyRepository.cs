using Ats.Data.EntityFramework;
using Ats.Domain.Loan;
using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Data.Repositories.Loan
{
    public class AssetPropertyRepository : Repository<AssetProperty, int>, IAssetPropertyRepository
    {
        public AssetPropertyRepository(SCDataContext context) : base(context)
        {
        }
    }
}
