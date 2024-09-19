using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Organization.Models;
using Ats.Models.Organization;
using Ats.Models.Warehouse;
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
    public class OrganizationService : BaseService, IOrganizationService
    {
        private IConfiguration _config;
        private int pageSize;

        public OrganizationService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<OrganizationViewModel> Search(string searchText, ORGANIZATION_TYPE? type, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Organizations service Search={searchText}, type={type}, Page={page}");
            var query = UnitOfWork.OrganizationRepo.GetAll().Where(x => (string.IsNullOrEmpty(searchText) ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower())) &&
                                    (type == null || x.OrganizationType == type));
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
                    case "type":
                        query = IsAscOrder ? query.OrderBy(x => x.OrganizationType) : query.OrderByDescending(x => x.OrganizationType);
                        break;
                }
            }

            var data = query.Select(x => new OrganizationViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                OrganizationType = x.OrganizationType,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public OrganizationViewModel GetOrganization(Guid id)
        {
            _logger.LogDebug($"Organization Detail service (Id: {id})");
            var entity = UnitOfWork.OrganizationRepo.GetById(id);
            if (entity != null)
            {
                var model = new OrganizationViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    OrganizationType = entity.OrganizationType,
                    IsActive = entity.IsActive
                };

                foreach (Warehouse item in entity.Warehouses.OrderBy(x => x.Name))
                {
                    var w = new WarehouseViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ShortName = item.ShortName,
                        WarehouseType = item.WarehouseType,
                        IsActive = item.IsActive
                    };
                    model.Warehouses.Add(w);
                }
                return model;
            }
            return null;
        }
        public void CreateOrganization(OrganizationViewModel model)
        {
            _logger.LogDebug($"Create Organization service (Name: {model.Name})");
            var entity = new Organization
            {
                Id = model.Id,
                Name = model.Name,      
                OrganizationType = model.OrganizationType,
                IsActive = model.IsActive
            };

            UnitOfWork.OrganizationRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateOrganization(OrganizationViewModel model)
        {
            _logger.LogDebug($"Edit Organization service (Id: {model.Id})");
            var entity = UnitOfWork.OrganizationRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.OrganizationType = model.OrganizationType;
                entity.IsActive = model.IsActive;
                UnitOfWork.OrganizationRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteOrganization(Guid id)
        {
            _logger.LogDebug($"Delete Organization service (Id: {id})");
            var entity = UnitOfWork.OrganizationRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.OrganizationRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
