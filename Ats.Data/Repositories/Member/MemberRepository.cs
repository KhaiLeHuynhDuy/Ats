using Ats.Data.EntityFramework;
using Ats.Domain.Member;
using Ats.Domain.Member.Models;
using Ats.Domain.Member.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Member
{
    public class MemberRepository : Repository<Domain.Member.Models.Member, Guid>, IMemberRepository
    {
        private SCDataContext _context;
        public MemberRepository(SCDataContext context) : base(context)
        {
            _context = context;
        }
        //public int GetMaxMemberId()
        //{
        //    try
        //    {
        //        return this.GetAll().Max(x => x.MemberId);
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        //public List<Domain.Member.Models.Member> mmbWallet(Guid Id)
        //{

        //    var Member = (from s in _context.Member
        //                                     join m in _context.MemberWallet on s.Id equals m.MemberId
        //                                     join a in _context.MemberLoyaltyTransaction on m.Id equals a.WalletId
        //                                     where s.Id == Id

        //                                     select new 
        //                                     {
        //                                         Id = m.Id,
        //                                         Phone = s.PhoneNumber,
        //                                         Point =a.Point,
        //                                         ActiveEnd = a.TransactionDate
        //                                     }).ToList();

        //    if (Member.Count > 0)
        //    {
        //        return ;
        //    }

        //    return new List<Domain.Member.Models.Member>();
        //}
    }
}
