using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Gift;

namespace Ats.Data.Repositories.Gift
{
    public class GiftCategoryRepository : Repository<Ats.Domain.Gift.Models.GiftCategory, int>, IGiftCategoryRepository
    {
        public GiftCategoryRepository(SCDataContext context) : base(context)
        {
        }
    }
}
