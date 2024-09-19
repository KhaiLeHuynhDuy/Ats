using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Address.Models;
using Ats.Domain.Lead.Models;
using Ats.Domain.Loan.Models;
using Ats.Models.Lead;
using Ats.Models.Product;
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
    public class LoanProductService : BaseService, ILoanProductService
    {
        private IConfiguration _config;
        private int pageSize;

        public LoanProductService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }    
        
        public List<LoanPackage> GetLoanProducts()
        {
            _logger.LogDebug($"GetLoanProducts Enter.");
            var loanProducts = UnitOfWork.LoanPackageRepo.GetAll().Where(x => x.IsActive).Select(x => new LoanPackage()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return loanProducts;
        }
        public LoanPackage GetLoanProduct(int id)
        {
            _logger.LogDebug($"Occupation Detail service (Id: {id})");
            var entity = UnitOfWork.LoanPackageRepo.GetById(id);
            if (entity != null)
            {
                var model = new LoanPackage()
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

        #region Product Category
        public List<ProductCategoryViewModel> SearchProductCategory(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Product Categories service Search={searchText}, Page={page}");
            var query = UnitOfWork.LoanProductCategoryRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
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

            var data = query.Select(x => new ProductCategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<ProductCategoryViewModel> GetProductCategories()
        {
            _logger.LogDebug($"Get Product Categories");
            var productCategories = UnitOfWork.LoanProductCategoryRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductCategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return productCategories;
        }

        public ProductCategoryViewModel GetProductCategory(int id)
        {
            _logger.LogDebug($"Product Category Detail service (Id: {id})");
            var entity = UnitOfWork.LoanProductCategoryRepo.GetById(id);
            if (entity != null)
            {
                var model = new ProductCategoryViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                foreach (LoanProductProperty item in entity.ProductProperties.OrderBy(x => x.Name))
                {
                    var p = new ProductPropertyViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ShortName = item.ShortName,
                        DataType = item.DataType,
                        Description = item.Description,
                        IsActive = item.IsActive,
                    };

                    model.ProductProperties.Add(p);
                }
                return model;
            }
            return null;
        }
        public void CreateProductCategory(ProductCategoryViewModel model)
        {
            _logger.LogDebug($"Create Product Category service (Name: {model.Name})");
            var entity = new LoanProductCategory
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };

            UnitOfWork.LoanProductCategoryRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductCategory(ProductCategoryViewModel model)
        {
            _logger.LogDebug($"Edit Product Category service (Id: {model.Id})");
            var entity = UnitOfWork.LoanProductCategoryRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                UnitOfWork.LoanProductCategoryRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteProductCategory(int id)
        {
            _logger.LogDebug($"Delete Product Category service (Id: {id})");
            var entity = UnitOfWork.LoanProductCategoryRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.LoanProductCategoryRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Product Property
        public List<ProductPropertyViewModel> SearchProductProperty(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Product Properties service Search={searchText}, Page={page}");
            var query = UnitOfWork.LoanProductPropertyRepo.GetAll().Where(x => (string.IsNullOrEmpty(searchText) ||
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
            List<ProductPropertyViewModel> data = new List<ProductPropertyViewModel>();
            foreach (var item in datas)
            {
                ProductPropertyViewModel productProperty = new ProductPropertyViewModel();
                productProperty.Id = item.Id;
                productProperty.Name = item.Name;
                productProperty.ShortName = item.ShortName;
                if (item.ProductCategory != null) productProperty.ProductCategoryName = item.ProductCategory.Name;
                productProperty.Description = item.Description;
                productProperty.DataType = item.DataType;
                productProperty.IsActive = item.IsActive;
                data.Add(productProperty);
            }
            return data;
        }
        public List<ProductPropertyViewModel> GetProductProperties()
        {
            _logger.LogDebug($"Get Product Properties");
            var productProperties = UnitOfWork.LoanProductPropertyRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductPropertyViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return productProperties;
        }
        public ProductPropertyViewModel GetProductProperty(int id)
        {
            _logger.LogDebug($"Product Property Detail service (Id: {id})");
            var entity = UnitOfWork.LoanProductPropertyRepo.GetById(id);
            if (entity != null)
            {
                var model = new ProductPropertyViewModel();
                model.Id = entity.Id;
                model.Name = entity.Name;
                model.ShortName = entity.ShortName;
                model.ProductCategoryId = entity.ProductCategoryId;
                model.Description = entity.Description;
                model.DataType = entity.DataType;
                model.IsActive = entity.IsActive;

                return model;
            }
            return null;
        }
        public void CreateProductProperty(ProductPropertyViewModel model)
        {
            _logger.LogDebug($"Create Product Property service (Name: {model.Name})");
            var entity = new LoanProductProperty
            {
                Id = model.Id,
                Name = model.Name,
                ShortName = model.ShortName,
                ProductCategoryId = model.ProductCategoryId,
                Description = model.Description,
                DataType = model.DataType,
                IsActive = model.IsActive

            };

            UnitOfWork.LoanProductPropertyRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProductProperty(ProductPropertyViewModel model)
        {
            _logger.LogDebug($"Edit ProductProperty service (Id: {model.Id})");
            var entity = UnitOfWork.LoanProductPropertyRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.ShortName = model.ShortName;
                entity.ProductCategoryId = model.ProductCategoryId;
                entity.Description = model.Description;
                entity.DataType = model.DataType;
                entity.IsActive = model.IsActive;

                UnitOfWork.LoanProductPropertyRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteProductProperty(int id)
        {
            _logger.LogDebug($"Delete Product Property service (Id: {id})");
            var entity = UnitOfWork.LoanProductPropertyRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.LoanProductPropertyRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Product
        public List<LoanProductViewModel> SearchProduct(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Products service Search={searchText}, Page={page}");
            var query = UnitOfWork.LoanProductRepo.GetAll().Where(x => (string.IsNullOrEmpty(searchText) ||
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
                    case "proCate":
                        query = IsAscOrder ? query.OrderBy(x => x.ProductCategory.Name) : query.OrderByDescending(x => x.ProductCategory.Name);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<LoanProductViewModel> data = new List<LoanProductViewModel>();
            foreach (var item in datas)
            {
                LoanProductViewModel product = new LoanProductViewModel();
                product.Id = item.Id;
                product.Code = item.Code;
                product.Name = item.Name;
                product.ShortName = item.ShortName;
                if (item.ProductCategory != null) product.ProductCategoryName = item.ProductCategory.Name;
                product.MinAmount = item.MinAmount;
                product.MaxAmount = item.MaxAmount;
                product.Duration = item.Duration;
                product.Description = item.Description;
                product.IsActive = item.IsActive;
                data.Add(product);
            }
            return data;
        }
        public LoanProductViewModel GetProduct(int id)
        {
            _logger.LogDebug($"Product Detail service (Id: {id})");
            var entity = UnitOfWork.LoanProductRepo.GetById(id);
            if (entity != null)
            {
                var model = new LoanProductViewModel()
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Name = entity.Name,
                    ShortName = entity.ShortName,
                    MinAmount = entity.MinAmount,
                    MaxAmount = entity.MaxAmount,
                    Duration = entity.Duration,
                    ProductCategoryId = entity.ProductCategoryId,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                foreach (LoanProductAttribute item in entity.ProductAttributes)
                {
                    var p = new ProductAttributeModel();
                    p.Id = item.Id;
                    if (item.ProductProperty != null)
                    {
                        p.DataType = item.DataType;
                        p.ProductPropertyName = item.ProductProperty.Name;
                        p.ProductPropertyShortName = item.ProductProperty.ShortName;
                    }
                    p.Value = item.Value;
                    model.ProductAttributes.Add(p);
                }

                return model;
            }
            return null;
        }
        public void CreateProduct(LoanProductViewModel model)
        {
            _logger.LogDebug($"Create Product service (Name: {model.Name})");
            var entity = new LoanProduct
            {
                Id = model.Id,
                Name = model.Name,
                ShortName = model.ShortName,
                ProductCategoryId = model.ProductCategoryId,
                Description = model.Description,
                IsActive = model.IsActive

            };

            UnitOfWork.LoanProductRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateProduct(LoanProductViewModel model)
        {
            _logger.LogDebug($"Edit Product service (Id: {model.Id})");
            var entity = UnitOfWork.LoanProductRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.ShortName = model.ShortName;
                entity.ProductCategoryId = model.ProductCategoryId;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;

                UnitOfWork.LoanProductRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteProduct(int id)
        {
            _logger.LogDebug($"Delete Product service (Id: {id})");
            var entity = UnitOfWork.LoanProductRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.LoanProductRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Product Attribute
        public bool CheckExistAddProductAttribute(int productId, int productPropertyId)
        {
            _logger.LogDebug($"Check existed Product Attribute");
            return UnitOfWork.LoanProductAttributeRepo.CheckExistAddProductAttribute(productId, productPropertyId);
        }
        public void AddProductAttribute(ProductAttributeModel model)
        {
            _logger.LogDebug($"Add new Product Attribute");
            var entity = new LoanProductAttribute();
            entity.ProductId = model.ProductId;
            entity.ProductPropertyId = model.ProductPropertyId;

            entity.DataType = model.DataType;
            entity.Value = model.Value;

            UnitOfWork.LoanProductAttributeRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public ProductAttributeModel GetProductAttribute(int id)
        {
            _logger.LogDebug($"Get Product attribute (Id: {id})");
            var entity = UnitOfWork.LoanProductAttributeRepo.GetById(id);
            if (entity != null)
            {
                ProductAttributeModel model = new ProductAttributeModel()
                {
                    Id = entity.Id,
                    ProductId = entity.ProductId,
                    ProductPropertyId = entity.ProductPropertyId,
                    ProductPropertyName = entity.ProductProperty.Name,
                    DataType = entity.DataType,
                    Value = entity.Value
                };
                return model;
            }
            return null;
        }
        public void UpdateProductAttribute(ProductAttributeModel model)
        {
            _logger.LogDebug($"Update Product Attribute (Id: {model.Id})");
            var entity = UnitOfWork.LoanProductAttributeRepo.GetAll().Where(x => x.Id == model.Id).FirstOrDefault();
            if (entity != null)
            {
                entity.DataType = model.DataType;
                entity.Value = model.Value;

                UnitOfWork.LoanProductAttributeRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteProductAttribute(int id)
        {
            _logger.LogDebug($"Delete Product Attribute service (Id: {id})");
            var entity = UnitOfWork.LoanProductAttributeRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.LoanProductAttributeRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

    }
}
