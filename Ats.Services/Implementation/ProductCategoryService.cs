using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
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
    public class ProductCategoryService : BaseService, IProductCategoryService
    {
        private IConfiguration _config;
        private int pageSize;

        public ProductCategoryService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<ProductCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.ProductCategoryRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
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
                    case "displayorder":
                        query = IsAscOrder ? query.OrderBy(x => x.DisplayOrder) : query.OrderByDescending(x => x.DisplayOrder);
                        break;
                }
            }

            var data = query.Select(x => new ProductCategoryViewModel()
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                DisplayOrder = x.DisplayOrder,
                Description = x.Description,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<ProductCategoryViewModel> GetProductCategorys()
        {
            _logger.LogDebug($"Get all product category");
            var productCategories = UnitOfWork.ProductCategoryRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductCategoryViewModel()
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Name = x.Name,
                DisplayOrder = x.DisplayOrder,
                Description = x.Description,
                IsActive = x.IsActive
            }).OrderBy(x => x.Name).ToList();

            return productCategories;
        }

        public ProductCategoryViewModel GetProductCategory(int id)
        {
            _logger.LogDebug($"Detail service (Id: {id})");
            var entity = UnitOfWork.ProductCategoryRepo.GetById(id);
            if (entity != null)
            {
                var model = new ProductCategoryViewModel()
                {
                    Id = entity.Id,
                    ParentId = entity.ParentId,
                    Name = entity.Name,
                    DisplayOrder = entity.DisplayOrder,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                return model;
            }
            return null;
        }
        public void CreateProductCategory(ProductCategoryViewModel model)
        {
            //_logger.LogDebug($"Create (Name: {model.Name})");
            var entity = new ProductCategory
            {
                Id = model.Id,
                Name = model.Name,
                ParentId = model.ParentId,
                DisplayOrder = model.DisplayOrder,
                Description = model.Description,
                IsActive = model.IsActive
            };

            UnitOfWork.ProductCategoryRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductCategory(ProductCategoryViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.ProductCategoryRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.ParentId = model.ParentId;
                entity.DisplayOrder = model.DisplayOrder;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                UnitOfWork.ProductCategoryRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteProductCategory(int id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.ProductCategoryRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.ProductCategoryRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
