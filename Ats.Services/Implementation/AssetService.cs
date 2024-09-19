using Ats.Domain;
using Ats.Domain.Loan.Models;
using Ats.Models.Asset;
using Ats.Services.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class AssetService : BaseService, IAssetService
    {
        private IConfiguration _config;
        private int pageSize;

        public AssetService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
        #region Asset Category
        public List<AssetTypeViewModel> SearchAssetType(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Asset Types service Search={searchText}, Page={page}");
            var query = UnitOfWork.AssetTypeRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
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

            var data = query.Select(x => new AssetTypeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public AssetTypeViewModel GetAssetType(int id)
        {
            _logger.LogDebug($"Asset Type Detail service (Id: {id})");
            var entity = UnitOfWork.AssetTypeRepo.GetById(id);
            if (entity != null)
            {
                var model = new AssetTypeViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                foreach (AssetProperty item in entity.AssetProperties.OrderBy(x => x.Name))
                {
                    var p = new AssetPropertyViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ShortName = item.ShortName,
                        DataType = item.DataType,
                        Description = item.Description,
                        IsActive = item.IsActive,
                    };

                    model.AssetProperties.Add(p);
                }
                return model;
            }
            return null;
        }
        public void CreateAssetType(AssetTypeViewModel model)
        {
            _logger.LogDebug($"Create Asset Type service (Name: {model.Name})");
            var entity = new AssetType
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };

            UnitOfWork.AssetTypeRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateAssetType(AssetTypeViewModel model)
        {
            _logger.LogDebug($"Edit Asset Type service (Id: {model.Id})");
            var entity = UnitOfWork.AssetTypeRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                UnitOfWork.AssetTypeRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteAssetType(int id)
        {
            _logger.LogDebug($"Delete Asset Type service (Id: {id})");
            var entity = UnitOfWork.AssetTypeRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.AssetTypeRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Asset Property
        public List<AssetPropertyViewModel> SearchAssetProperty(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Asset Properties service Search={searchText}, Page={page}");
            var query = UnitOfWork.AssetPropertyRepo.GetAll().Where(x => (string.IsNullOrEmpty(searchText) ||
                                (x.Name != null && x.Name.ToLower().Contains(searchText.ToLower())))
                                /*&& x.ShortName != null && x.ShortName.ToLower().Contains(searchText.ToLower())*/);
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
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<AssetPropertyViewModel> data = new List<AssetPropertyViewModel>();
            foreach (var item in datas)
            {
                AssetPropertyViewModel assetProperty = new AssetPropertyViewModel();
                assetProperty.Id = item.Id;
                assetProperty.Name = item.Name;
                assetProperty.ShortName = item.ShortName;
                if (item.AssetType != null) assetProperty.AssetTypeName = item.AssetType.Name;
                assetProperty.Description = item.Description;
                assetProperty.DataType = item.DataType;
                assetProperty.IsActive = item.IsActive;
                data.Add(assetProperty);
            }
            return data;
        }
        public List<AssetPropertyViewModel> GetAssetProperties()
        {
            _logger.LogDebug($"Get Asset Properties");
            var assetProperties = UnitOfWork.AssetPropertyRepo.GetAll().Where(x => x.IsActive).Select(x => new AssetPropertyViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return assetProperties;
        }
        public AssetPropertyViewModel GetAssetProperty(int id)
        {
            _logger.LogDebug($"Asset Property Detail service (Id: {id})");
            var entity = UnitOfWork.AssetPropertyRepo.GetById(id);
            if (entity != null)
            {
                var model = new AssetPropertyViewModel();
                model.Id = entity.Id;
                model.Name = entity.Name;
                model.ShortName = entity.ShortName;
                model.AssetTypeId = entity.AssetTypeId;
                model.Description = entity.Description;
                model.DataType = entity.DataType;
                model.IsActive = entity.IsActive;

                return model;
            }
            return null;
        }
        public void CreateAssetProperty(AssetPropertyViewModel model)
        {
            _logger.LogDebug($"Create Asset Property service (Name: {model.Name})");
            var entity = new AssetProperty
            {
                Id = model.Id,
                Name = model.Name,
                ShortName = model.ShortName,
                AssetTypeId = model.AssetTypeId,
                Description = model.Description,
                DataType = model.DataType,
                IsActive = model.IsActive

            };

            UnitOfWork.AssetPropertyRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateAssetProperty(AssetPropertyViewModel model)
        {
            _logger.LogDebug($"Edit AssetProperty service (Id: {model.Id})");
            var entity = UnitOfWork.AssetPropertyRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.ShortName = model.ShortName;
                entity.AssetTypeId = model.AssetTypeId;
                entity.Description = model.Description;
                entity.DataType = model.DataType;
                entity.IsActive = model.IsActive;

                UnitOfWork.AssetPropertyRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteAssetProperty(int id)
        {
            _logger.LogDebug($"Delete Asset Property service (Id: {id})");
            var entity = UnitOfWork.AssetPropertyRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.AssetPropertyRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Asset
        public List<AssetViewModel> SearchAsset(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Assets service Search={searchText}, Page={page}");
            var query = UnitOfWork.AssetRepo.GetAll().Where(x => (string.IsNullOrEmpty(searchText) ||
                                (x.Name != null && x.Name.ToLower().Contains(searchText.ToLower())))
                                /*&& x.ShortName != null && x.ShortName.ToLower().Contains(searchText.ToLower())*/);
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
                    case "shortName":
                        query = IsAscOrder ? query.OrderBy(x => x.ShortName) : query.OrderByDescending(x => x.ShortName);
                        break;
                    case "assetType":
                        query = IsAscOrder ? query.OrderBy(x => x.AssetType.Name) : query.OrderByDescending(x => x.AssetType.Name);
                        break;
                    case "loan":
                        query = IsAscOrder ? query.OrderBy(x => x.Loan.Amount) : query.OrderByDescending(x => x.Loan.Amount);
                        break;
                    case "warehouse":
                        query = IsAscOrder ? query.OrderBy(x => x.Warehouse.Name) : query.OrderByDescending(x => x.Warehouse.Name);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<AssetViewModel> data = new List<AssetViewModel>();
            foreach (var item in datas)
            {
                AssetViewModel asset = new AssetViewModel();
                asset.Id = item.Id;
                asset.Code = item.Code;
                asset.Name = item.Name;
                asset.ShortName = item.ShortName;
                if (item.AssetType != null) asset.AssetTypeName = item.AssetType.Name;
                if (item.Loan != null) asset.LoanAmount = item.Loan.Amount;
                if (item.Warehouse != null) asset.WarehouseName = item.Warehouse.Name;
                asset.Description = item.Description;
                asset.IsActive = item.IsActive;
                data.Add(asset);
            }
            return data;
        }
        public AssetViewModel GetAsset(int id)
        {
            _logger.LogDebug($"Asset Detail service (Id: {id})");
            var entity = UnitOfWork.AssetRepo.GetById(id);
            if (entity != null)
            {
                var model = new AssetViewModel()
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Name = entity.Name,
                    ShortName = entity.ShortName,
                    AssetTypeId = entity.AssetTypeId,
                    LoanId = entity.LoanId,
                    WarehouseId = entity.WarehouseId,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                foreach (AssetAttribute item in entity.AssetAttributes)
                {
                    var a = new AssetAttributeModel();
                    a.Id = item.Id;
                    if (item.AssetProperty != null)
                    {
                        a.DataType = item.DataType;
                        a.AssetPropertyName = item.AssetProperty.Name;
                        a.AssetPropertyShortName = item.AssetProperty.ShortName;
                    }
                    a.Value = item.Value;
                    model.AssetAttributes.Add(a);
                }

                return model;
            }
            return null;
        }
        public void CreateAsset(AssetViewModel model)
        {
            _logger.LogDebug($"Create Asset service (Name: {model.Name})");
            var entity = new Asset
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name,
                ShortName = model.ShortName,
                AssetTypeId = model.AssetTypeId,
                LoanId = model.LoanId,
                WarehouseId = model.WarehouseId,
                Description = model.Description,
                IsActive = model.IsActive

            };

            UnitOfWork.AssetRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateAsset(AssetViewModel model)
        {
            _logger.LogDebug($"Edit Asset service (Id: {model.Id})");
            var entity = UnitOfWork.AssetRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Code = model.Code;
                entity.Name = model.Name;
                entity.ShortName = model.ShortName;
                entity.AssetTypeId = model.AssetTypeId;
                entity.LoanId = model.LoanId;
                entity.WarehouseId = model.WarehouseId;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;

                UnitOfWork.AssetRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteAsset(int id)
        {
            _logger.LogDebug($"Delete Asset service (Id: {id})");
            var entity = UnitOfWork.AssetRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.AssetRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Asset Attribute
        public bool CheckExistAddAssetAttribute(int productId, int productPropertyId)
        {
            _logger.LogDebug($"Check existed Asset Attribute");
            return UnitOfWork.AssetAttributeRepo.CheckExistAddAssetAttribute(productId, productPropertyId);
        }
        public void AddAssetAttribute(AssetAttributeModel model)
        {
            _logger.LogDebug($"Add new Asset Attribute");
            var entity = new AssetAttribute();
            entity.AssetId = model.AssetId;
            entity.AssetPropertyId = model.AssetPropertyId;

            entity.DataType = model.DataType;
            entity.Value = model.Value;

            UnitOfWork.AssetAttributeRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public AssetAttributeModel GetAssetAttribute(int id)
        {
            _logger.LogDebug($"Get Asset attribute (Id: {id})");
            var entity = UnitOfWork.AssetAttributeRepo.GetById(id);
            if (entity != null)
            {
                AssetAttributeModel model = new AssetAttributeModel()
                {
                    Id = entity.Id,
                    AssetId = entity.AssetId,
                    AssetPropertyId = entity.AssetPropertyId,
                    AssetPropertyName = entity.AssetProperty.Name,
                    DataType = entity.DataType,
                    Value = entity.Value
                };
                return model;
            }
            return null;
        }
        public void UpdateAssetAttribute(AssetAttributeModel model)
        {
            _logger.LogDebug($"Update Asset Attribute (Id: {model.Id})");
            var entity = UnitOfWork.AssetAttributeRepo.GetAll().Where(x => x.Id == model.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.DataType = model.DataType;
                entity.Value = model.Value;

                UnitOfWork.AssetAttributeRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteAssetAttribute(int id)
        {
            _logger.LogDebug($"Delete Asset Attribute service (Id: {id})");
            var entity = UnitOfWork.AssetAttributeRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.AssetAttributeRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion
    }
}
