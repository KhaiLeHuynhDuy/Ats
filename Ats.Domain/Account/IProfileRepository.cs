using Ats.Domain;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Identity.Models;
using System;

namespace Ats.Domain.Accounts.Repositories
{
    public interface IProfileRepository : IRepository<Profile, System.Guid>
    {
        Profile GetUserProfile(Guid userId);

        void UpdateUserProfile(Guid profileId, string firstName, string lastName, string lang, string countryCode, string timezoneCode, PROFILE_TYPE userType, string mobile, string phone);

        User GetUserByUserId(Guid userId);
    }
}
