using Ats.Models.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Services.Interfaces
{
    public interface ITeamService
    {
        List<TeamViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        TeamViewModel GetTeam(Guid id);
        List<TeamViewModel> GetTeams();
        void CreateTeam(TeamViewModel model);
        void UpdateTeam(TeamViewModel model);
        void DeleteTeam(Guid id);
        //Team user
        bool CheckExistAddTeamUser(Guid teamId, Guid userId);
        void AddTeamUser(TeamUserUpdateModel model);
        TeamUserUpdateModel GetTeamUserDetail(Guid id);
        void UpdateTeamUser(TeamUserUpdateModel model);
        void DeleteTeamUser(Guid id);
    }
}
