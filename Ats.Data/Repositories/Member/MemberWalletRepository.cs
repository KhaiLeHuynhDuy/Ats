using Ats.Data.EntityFramework;
using Ats.Domain.Member;
using Ats.Domain.Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ats.Data.Repositories.Member
{

    public class MemberWalletRepository : Repository<MemberWallet, Guid>, IMemberWalletRepository
    {
        private SCDataContext _context;
        public MemberWalletRepository(SCDataContext context) : base(context)
        {
            _context = context;
        }

        public MemberWallet GetWallet(string email)
        {
            throw new NotImplementedException();
        }

        //public List<MemberWallet> mbWallet(Guid WalletId)check data walletMember
        //{

        //    var MemberLoyaltyTransactions = (from s in _context.MemberLoyaltyTransaction
        //                                     join m in _context.MemberWallet on s.WalletId equals m.Id   
        //                                     join mb in _context.Member on m.MemberId equals mb.Id
        //                                     where s.WalletId == WalletId


        //                                     select new MemberWallet
        //                                     {

        //                                         Id = m.Id,
        //                                         Point = s.Point,
        //                                         ActiveEnd = s.TransactionDate
        //                                     }).ToList();



        //    if (MemberLoyaltyTransactions.Count > 0)
        //    {
        //        return MemberLoyaltyTransactions;
        //    }

        //    return new List<MemberWallet>();
        //}
    }
}
