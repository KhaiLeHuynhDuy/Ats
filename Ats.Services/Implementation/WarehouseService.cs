using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Organization.Models;
using Ats.Models.Asset;
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
    public class WarehouseService : BaseService, IWarehouseService
    {
        private IConfiguration _config;
        private int pageSize;

        public WarehouseService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<WarehouseViewModel> Search(string searchText, WAREHOUSE_TYPE? type, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Warehouses service Search={searchText}, type={type}, Page={page}");
            var query = UnitOfWork.WarehouseRepo.GetAll().Where(x => (string.IsNullOrEmpty(searchText) ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower())) &&
                                    (type == null || x.WarehouseType == type));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                    case "type":
                        query = IsAscOrder ? query.OrderBy(x => x.WarehouseType) : query.OrderByDescending(x => x.WarehouseType);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<WarehouseViewModel> data = new List<WarehouseViewModel>();
            foreach (var item in datas)
            {
                WarehouseViewModel warehouse = new WarehouseViewModel();
                warehouse.Id = item.Id;
                warehouse.Name = item.Name;
                warehouse.ShortName = item.ShortName;
                if (item.Organization != null) warehouse.OrganizationName = item.Organization.Name;
                warehouse.WarehouseType = item.WarehouseType;
                warehouse.IsActive = item.IsActive;
                data.Add(warehouse);
            }
            return data;
        }

        public WarehouseViewModel GetWarehouse(Guid id)
        {
            _logger.LogDebug($"Warehouse Detail service (Id: {id})");
            var entity = UnitOfWork.WarehouseRepo.GetById(id);
            if (entity != null)
            {
                var model = new WarehouseViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    ShortName = entity.ShortName,
                    OrganizationId = entity.OrganizationId,
                    WarehouseType = entity.WarehouseType,
                    IsActive = entity.IsActive,
                };

                foreach (Asset item in entity.Assets.OrderBy(x => x.AddedTimestamp))
                {
                    var asset = new AssetViewModel();
                    asset.Id = item.Id;
                    asset.Code = item.Code;
                    asset.Name = item.Name;
                    asset.ShortName = item.ShortName;
                    if (item.AssetType != null) asset.AssetTypeName = item.AssetType.Name;
                    if (item.Loan != null) asset.LoanAmount = item.Loan.Amount;
                    asset.IsActive = item.IsActive;

                    model.Assets.Add(asset);
                }
                return model;
            }
            return null;
        }
        public void CreateWarehouse(WarehouseViewModel model)
        {
            _logger.LogDebug($"Create Warehouse service (Name: {model.Name})");
            var entity = new Warehouse
            {
                Id = model.Id,
                Name = model.Name,
                ShortName = model.ShortName,
                WarehouseType = model.WarehouseType,
                OrganizationId = model.OrganizationId,
                IsActive = model.IsActive,
            };

            UnitOfWork.WarehouseRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateWarehouse(WarehouseViewModel model)
        {
            _logger.LogDebug($"Edit Warehouse service (Id: {model.Id})");
            var entity = UnitOfWork.WarehouseRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.ShortName = model.ShortName;
                entity.WarehouseType = model.WarehouseType;
                entity.OrganizationId = model.OrganizationId;
                entity.IsActive = model.IsActive;

                UnitOfWork.WarehouseRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteWarehouse(Guid id)
        {
            _logger.LogDebug($"Delete Warehouse service (Id: {id})");
            var entity = UnitOfWork.WarehouseRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.WarehouseRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
