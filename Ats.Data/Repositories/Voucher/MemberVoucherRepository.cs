using System;
using System.Collections.Generic;
using System.Text;
using Ats.Data.EntityFramework;
using Ats.Domain.Voucher;

namespace Ats.Data.Repositories.Voucher
{
    public class MemberVoucherRepository : Repository<Ats.Domain.Voucher.Models.MemberVoucher, Guid>, IMemberVoucherRepository
    {
        public MemberVoucherRepository(SCDataContext context) : base(context)
        {
        }
    }
}
