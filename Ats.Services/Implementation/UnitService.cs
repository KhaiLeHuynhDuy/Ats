using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Domain;
using Ats.Domain.Store.Models;
using Ats.Models.Unit;
using Ats.Services.Extensions;
using Microsoft.Extensions.Configuration;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{

    public class UnitService : BaseService, IUnitService
    {
        private IConfiguration _config;
        private int pageSize;

        public UnitService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
    
        public void CreateUnit(UnitViewModel model)
        {
            //_logger.LogDebug($"Create (Name: {model.Name})");
            var entity = new Unit
            {
                Id = model.Id,
                Name = model.Name,
                IsActive = model.IsActive,
                Desc = model.Desc
            };

            UnitOfWork.UnitRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteUnity(int id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.UnitRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.UnitRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public UnitViewModel GetUnit(int id)
        {
            _logger.LogDebug($"Detail service (Id: {id})");
            var entity = UnitOfWork.UnitRepo.GetById(id);
            if (entity != null)
            {
                var model = new UnitViewModel()
                {
                    Id = entity.Id,                 
                    Name = entity.Name,
                    IsActive = entity.IsActive,               
                    Desc = entity.Desc,
                };

                return model;
            }
            return null;
        }

        public List<UnitViewModel> GetUnits()
        {
            _logger.LogDebug($"Get all Unit category");
            var units = UnitOfWork.UnitRepo.GetAll().Where(x=>x.IsActive).Select(x => new UnitViewModel()
            {
                UnitId = x.Id,
                Name = x.Name,
                Desc = x.Desc
            }).OrderBy(x => x.Name).ToList();
            return units;
            //throw new NotImplementedException();
        }

        //IsActive
        public List<UnitViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.UnitRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "desc":
                        query = IsAscOrder ? query.OrderBy(x => x.Desc) : query.OrderByDescending(x => x.Desc);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                    case "IsActive":
                        query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                        break;
                }
            }

            var data = query.Select(x => new UnitViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                Desc = x.Desc
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }
        public void UpdateUnit(UnitViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.UnitRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Desc = model.Desc;
                entity.IsActive = model.IsActive;
                UnitOfWork.UnitRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
