using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Models.Occupation;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class OccupationService : BaseService, IOccupationService
    {
        private IConfiguration _config;
        private int pageSize;

        public OccupationService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<OccupationViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Occupations service Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.OccupationRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
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

            var data = query.Select(x => new OccupationViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<OccupationViewModel> GetOccupations()
        {
            _logger.LogDebug($"GetOccupations");
            var occupations = UnitOfWork.OccupationRepo.GetAll().Where(x => x.IsActive).Select(x=> new OccupationViewModel() { 
            Id = x.Id,
            Name = x.Name
            }).OrderBy(x => x.Name).ToList();
            
            return occupations;
        }

        public OccupationViewModel GetOccupation(int id)
        {
            _logger.LogDebug($"Occupation Detail service (Id: {id})");
            var entity = UnitOfWork.OccupationRepo.GetById(id);
            if (entity != null)
            {
                var model = new OccupationViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                return model;
            }
            return null;
        }
        public void CreateOccupation(OccupationViewModel model)
        {
            _logger.LogDebug($"Create Occupation service (Name: {model.Name})");
            var entity = new Occupation
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };

            UnitOfWork.OccupationRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateOccupation(OccupationViewModel model)
        {
            _logger.LogDebug($"Edit Occupation service (Id: {model.Id})");
            var entity = UnitOfWork.OccupationRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                UnitOfWork.OccupationRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteOccupation(int id)
        {
            _logger.LogDebug($"Delete Occupation service (Id: {id})");
            var entity = UnitOfWork.OccupationRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.OccupationRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }        
    }
}
