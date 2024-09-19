using Ats.Commons.Utilities;
using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Models.Team;
using Ats.Services.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class TeamService : BaseService, ITeamService
    {
        private IConfiguration _config;
        private int pageSize;

        public TeamService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<TeamViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Teams service Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.TeamRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                }
            }

            var data = query.Select(x => new TeamViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public TeamViewModel GetTeam(Guid id)
        {
            _logger.LogDebug($"Team Detail service (Id: {id})");
            var entity = UnitOfWork.TeamRepo.GetById(id);
            if (entity != null)
            {
                var model = new TeamViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsActive = entity.IsActive
                };
                foreach (TeamUser item in entity.TeamUsers.OrderBy(x => x.User.FirstName))
                {
                    var m = new TeamUserModel()
                    {
                        Id = item.Id,
                        FirstName = item.User.FirstName,
                        LastName = item.User.LastName,
                        Email = item.User.Email,
                        PhoneNumber = item.User.PhoneNumber,
                        UserName = item.User.UserName,
                        Tag = item.TeamRole.ToName(),
                        StartDate = item.DateFrom,
                        EndDate = item.DateTo,
                    };

                    model.TeamUsers.Add(m);
                }
                return model;
            }
            return null;
        }
        public void CreateTeam(TeamViewModel model)
        {
            _logger.LogDebug($"Create Team service (Name: {model.Name})");
            var entity = new Team
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive
            };

            UnitOfWork.TeamRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateTeam(TeamViewModel model)
        {
            _logger.LogDebug($"Edit Team service (Id: {model.Id})");
            var entity = UnitOfWork.TeamRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.IsActive = model.IsActive;
                UnitOfWork.TeamRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteTeam(Guid id)
        {
            _logger.LogDebug($"Delete Team service (Id: {id})");
            var entity = UnitOfWork.TeamRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.TeamRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public bool CheckExistAddTeamUser(Guid teamId, Guid userId)
        {
            _logger.LogDebug($"Check existed Team User");
            return UnitOfWork.TeamUserRepo.CheckExistAddTeamUser(teamId, userId);
        }
        public void AddTeamUser(TeamUserUpdateModel model)
        {
            _logger.LogDebug($"Add new Team User");
            var entity = new TeamUser
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                TeamId = model.TeamId,
                TeamRole = model.TeamRole,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
            };

            UnitOfWork.TeamUserRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public TeamUserUpdateModel GetTeamUserDetail(Guid id)
        {
            _logger.LogDebug($"Get Team User detail (Id: {id})");
            var entity = UnitOfWork.TeamUserRepo.GetById(id);
            if (entity != null)
            {
                TeamUserUpdateModel model = new TeamUserUpdateModel()
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    TeamId = entity.TeamId,
                    TeamRole = entity.TeamRole,     
                    DateFrom = entity.DateFrom,
                    DateTo = entity.DateTo
                };
                return model;
            }
            return null;  
        }
        public void UpdateTeamUser(TeamUserUpdateModel model)
        {
            _logger.LogDebug($"Update Team User (Id: {model.Id})");
            var entity = UnitOfWork.TeamUserRepo.GetAll().Where(x => x.Id == model.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.DateFrom = model.DateFrom;
                entity.DateTo = model.DateTo;
                entity.TeamRole = model.TeamRole;

                UnitOfWork.TeamUserRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteTeamUser(Guid id)
        {
            _logger.LogDebug($"Delete Team User service (Id: {id})");
            var entity = UnitOfWork.TeamUserRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.TeamUserRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<TeamViewModel> GetTeams()
        {
            _logger.LogDebug($"Get teams");
            var teams = UnitOfWork.TeamRepo.GetAll().Where(x => x.IsActive).Select(x => new TeamViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return teams;
        }
    }
}
