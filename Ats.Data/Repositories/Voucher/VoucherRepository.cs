using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Voucher;

namespace Ats.Data.Repositories.Voucher
{
    public class VoucherRepository : Repository<Ats.Domain.Voucher.Models.Voucher, Guid>, IVoucherRepository
    {
        public VoucherRepository(SCDataContext context) : base(context)
        {
        }
    }
}
