using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Accounts.Repositories;
using Ats.Domain.Identity.Models;
using System;
using System.Linq;

namespace Ats.Data.Accounts.Repositories
{
    public class ProfileRepository : Repository<Profile, System.Guid>, IProfileRepository
    {
        public ProfileRepository(SCDataContext context) : base(context)
        {
        }

        public Profile GetUserProfile(Guid userId)
        {
            throw new NotImplementedException("Need to remove User ouf of profile");
        }

        public void UpdateUserProfile(Guid profileId, string firstName, string lastName, string lang, string countryCode, string timezoneCode, PROFILE_TYPE userType, string mobile, string phone)
        {
            if (profileId == null || profileId == Guid.Empty)
                throw new ArgumentNullException("profileId");

            Profile profile = this.GetById(profileId);
            if (profile == null)
                throw new InvalidOperationException($"Profile ({profileId}) doest not exist.");

            if (profile.IsDeleted)
                throw new InvalidOperationException($"Profile ({profileId}) has been deleted.");

            profile.Lang = lang;
            profile.CountryCode = countryCode;
            profile.TimezoneCode = timezoneCode;
            profile.ProfileType = userType;            

            this.DataContext.Update<Profile, Guid>(profile, x => x.Lang, x => x.CountryCode, x => x.TimezoneCode);
        }
        public User GetUserByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException("userId");

            User u = this.DataContext.Get<User>().Where(x => x.Id == userId).FirstOrDefault();
            return u;
        }
        //public void DeleteUserGroupByIdUserAndIdRoleGroup(string idUser, Guid idRoleGroup)
        //{
        //    try
        //    {
        //        Models.User user = this.DataContext.Get<Models.User>().Where(x => x.Id == idUser).FirstOrDefault();
        //        var roleDeleted = user.UserGroups.Where(x => x.Id == idRoleGroup).FirstOrDefault();
        //        var result = user.RoleGroups.Remove(roleDeleted);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        //public void CreateUserGroupBuIdUserAndIdRoleGroup(string idUser, Guid idRoleGroup)
        //{
        //    try
        //    {
        //        if (String.IsNullOrEmpty(idUser))
        //            throw new ArgumentNullException("idUser");
        //        if (idRoleGroup == Guid.Empty)
        //            throw new ArgumentNullException("idRoleGroup");
        //        Models.User user = this.DataContext.Get<Models.User>().Where(x => x.Id == idUser).FirstOrDefault();
        //        if (user == null)
        //            throw new ArgumentNullException($"The system don't have the userId: {idUser}");
        //        Models.Group role = this.DataContext.Get<Models.Group>().Where(x => x.Id == idRoleGroup).FirstOrDefault();
        //        if (role == null)
        //            throw new ArgumentNullException($"The system don't have the roleId: {idRoleGroup}");
        //        user.RoleGroups.Add(role);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
