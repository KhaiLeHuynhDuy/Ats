using Ats.Data.EntityFramework;
using Ats.Domain.Loan;
using Ats.Domain.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Loan
{
    public class AssetAttributeRepository : Repository<AssetAttribute, int>, IAssetAttributeRepository
    {
        public AssetAttributeRepository(SCDataContext context) : base(context)
        {
        }
        public bool CheckExistAddAssetAttribute(int assetId, int assetPropertyId)
        {
            return GetAll().Any(x => x.AssetId.Equals(assetId) && x.AssetPropertyId.Equals(assetPropertyId));
        }
    }
}
