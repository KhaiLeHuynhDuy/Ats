using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Lead.Models;
using Ats.Models.OriginalFile;
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
    public class OriginalFileService : BaseService, IOriginalFileService
    {
        private IConfiguration _config;
        private int pageSize;

        public OriginalFileService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
        #region Original File
        #endregion
        public List<OriginalFileViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all OriginalFiles service Search={searchText}, Page={page}");
            var query = UnitOfWork.OriginalFileRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
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

            var data = query.Select(x => new OriginalFileViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public OriginalFileViewModel GetOriginalFile(int id)
        {
            _logger.LogDebug($"OriginalFile Detail service (Id: {id})");
            var entity = UnitOfWork.OriginalFileRepo.GetById(id);
            if (entity != null)
            {
                var model = new OriginalFileViewModel()
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
        public void CreateOriginalFile(OriginalFileViewModel model)
        {
            _logger.LogDebug($"Create OriginalFile service (Name: {model.Name})");
            var entity = new OriginalFile
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };

            UnitOfWork.OriginalFileRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateOriginalFile(OriginalFileViewModel model)
        {
            _logger.LogDebug($"Edit OriginalFile service (Id: {model.Id})");
            var entity = UnitOfWork.OriginalFileRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                UnitOfWork.OriginalFileRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteOriginalFile(int id)
        {
            _logger.LogDebug($"Delete OriginalFile service (Id: {id})");
            var entity = UnitOfWork.OriginalFileRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.OriginalFileRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #region Original File Addition
        public List<OriginalFileViewModel> SearchAddition(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Original Files Addition service Search={searchText}, Page={page}");
            var query = UnitOfWork.OriginalFileAdditionRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
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

            var data = query.Select(x => new OriginalFileViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public OriginalFileViewModel GetOriginalFileAddition(int id)
        {
            _logger.LogDebug($"Original File Addition Detail service (Id: {id})");
            var entity = UnitOfWork.OriginalFileAdditionRepo.GetById(id);
            if (entity != null)
            {
                var model = new OriginalFileViewModel()
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

        public void CreateOriginalFileAddition(OriginalFileViewModel model)
        {
            _logger.LogDebug($"Create OriginalFile Addition service (Name: {model.Name})");
            var entity = new OriginalFileAddition
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };

            UnitOfWork.OriginalFileAdditionRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateOriginalFileAddition(OriginalFileViewModel model)
        {
            _logger.LogDebug($"Edit OriginalFile Addition service (Id: {model.Id})");
            var entity = UnitOfWork.OriginalFileAdditionRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                UnitOfWork.OriginalFileAdditionRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void DeleteOriginalFileAddition(int id)
        {
            _logger.LogDebug($"Delete OriginalFile Addition service (Id: {id})");
            var entity = UnitOfWork.OriginalFileAdditionRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.OriginalFileAdditionRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion
    }
}
