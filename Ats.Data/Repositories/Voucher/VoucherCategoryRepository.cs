using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Voucher;

namespace Ats.Data.Repositories.Voucher
{
    public class VoucherCategoryRepository : Repository<Ats.Domain.Voucher.Models.VoucherCategory, int>, IVoucherCategoryRepository
    {
        public VoucherCategoryRepository(SCDataContext context) : base(context)
        {
        }
    }
}
