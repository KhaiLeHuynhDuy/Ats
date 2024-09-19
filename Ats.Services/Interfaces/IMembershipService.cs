using Ats.Commons.Models;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.Account;
using Ats.Models.LoyaltyTier;
using Ats.Models.Member;
using Ats.Models.MemberWallet;
using Ats.Models.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Ats.Commons.Constants;

namespace Ats.Services.Interfaces
{
    public interface IMembershipService : IBaseService
    {
        List<MemberViewModel> Search(MemberSearchView memberSearch, string searchText,
            int? province, int? occupation, int? jobtitle, int? district, 
            bool? gender, int? storeId, int? channelId, bool? valid, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<MemberViewModel> Reset(MemberSearchView memberSearch, string searchText,
           int? province, int? occupation, int? jobtitle, int? district,
           bool? gender, int? storeId, int? channelId,bool? valid, string orderField, bool IsAscOrder, int page, int size, out int total);
        MemberViewModel GetMember(Guid id);        
        MemberDetailViewModel GetMemberDetail(Guid id);


        List<MemberWalletHistoryViewModel> RedeemPoint(Guid id, int page, int size, out int total);
        List<GiftVoucherCouponViewModel> GiftVoucherCoupon(Guid id, int page, int size, out int total);
        List<MemberLoyaltyDetailViewModel> Loyalty(Guid id, int page, int size, out int total);
        void CreateMemberLoyalty(MemberLoyaltyDetailViewModel model, Guid MemberId);
        void UpdateMemberLoyalty(MemberLoyaltyDetailViewModel model);
        MemberLoyaltyDetailViewModel GetMemberLoyalty(Guid id);

        void CreateMember(MemberRecruitmentViewModel model, Guid registrationUser);
        void DeleteMember(Guid id);
        void UpdateMember(MemberDetailViewModel model);

        void UpdateMemberDetail(MemberDetailViewModel model);

        Task<object> ImportMemberExcel(MemberColumnMappingViewModel Members, IFormFile formFile, Guid userId);
        List<MemberDetailViewModel> GetListMembers();
  
        List<MemberDetailViewModel> GetMembers();
        int GetNewMembersMonths(DateTime monthStar, DateTime monthEnd);
        int GetAllMembersOfYear(string years);
        int GetNewMembersOfMonth(int month, string years);
        int GetMembersOfMonthInActive(int month, string years);
        int GetStatisticsNewMembers(DateTime dayb, DateTime daye);
        int GetStatisticsNewMembers();
        int GetStatisticsNewMembersThisMonth(DateTime firstDayOfMonth, DateTime lastDayOfMonth);
    }
}