using Ats.Domain;
using Ats.Domain.Member.Models;
using Ats.Domain.Identity.Models;
using System;

namespace Ats.Domain.Member.Repositories
{
    public interface IMemberRepository : IRepository<Ats.Domain.Member.Models.Member, System.Guid>
    {
        //Ats.Domain.Member.Models.Member GetUserProfile(Guid userId);

        //void UpdateUserProfile(Guid profileId, string firstName, string lastName, string lang, string countryCode, string timezoneCode, MEMBER_TYPE userType, string mobile, string phone);

        //User GetUserByUserId(Guid userId);
        //int GetMaxMemberId();
        //int Count();
    }
}
