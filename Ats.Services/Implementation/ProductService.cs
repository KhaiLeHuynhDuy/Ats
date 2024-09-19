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
using System.IO;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Ats.Models.Brand;
using Ats.Commons.Utilities;
namespace Ats.Services.Implementation
{
    public class ProductService : BaseService, IProductService
    {
        private IConfiguration _config;
        private int pageSize;
        private IHostingEnvironment _hostingEnvironment;

        public ProductService(IUnitOfWork unitOfWork, IConfiguration config, IHostingEnvironment hostingEnvironment, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<ProductViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
           
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.ProductRepo.GetAll().Where(x => String.IsNullOrEmpty(searchText) ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower())||
                                x.Code !=null && x.Code.ToLower().Contains(searchText.ToLower())).OrderByDescending(x=>x.DisplayOrder);
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                  
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.Code) : query.OrderByDescending(x => x.Code);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.Name.Trim().ToLower()) : query.OrderByDescending(x => x.Name.Trim().ToLower());
                        break;
                    case "category":
                        query = IsAscOrder ? query.OrderBy(x => x.ProductCategory.Name) : query.OrderByDescending(x => x.ProductCategory.Name);
                        break;
                    //case "brand":
                    //    query = IsAscOrder ? query.OrderBy(x => x.Brand.BrandName) : query.OrderByDescending(x => x.Brand.BrandName);
                    //    break;
                    case "unit":
                        query = IsAscOrder ? query.OrderBy(x => x.Unit.Name) : query.OrderByDescending(x => x.Unit.Name);
                        break;
                    case "displayorder":
                        query = IsAscOrder ? query.OrderBy(x => x.DisplayOrder) : query.OrderByDescending(x => x.DisplayOrder);
                        break;
                    case "retailprice":
                        query = IsAscOrder ? query.OrderBy(x => x.RetailPrice) : query.OrderByDescending(x => x.RetailPrice);
                        break;
                    case "purchasedprice":
                        query = IsAscOrder ? query.OrderBy(x => x.PurchasedPrice) : query.OrderByDescending(x => x.PurchasedPrice);
                        break;
                    case "currency":
                        query = IsAscOrder ? query.OrderBy(x => x.Currency) : query.OrderByDescending(x => x.Currency);
                        break;
                    case "allowearnpoint":
                        query = IsAscOrder ? query.OrderBy(x => x.AllowEarnPoint) : query.OrderByDescending(x => x.AllowEarnPoint);
                        break;
                    case "allowredeem":
                        query = IsAscOrder ? query.OrderBy(x => x.AllowRedeem) : query.OrderByDescending(x => x.AllowRedeem);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                        break;
                }
            }
            
            

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<ProductViewModel> data = new List<ProductViewModel>();
            foreach (var item in datas)
            {
                ProductViewModel product = new ProductViewModel();
                product.Id = item.Id;
                if (item.ProductCategory != null) product.ProductCategoryName = item.ProductCategory.Name;
                product.Code = item.Code;
                product.ShortName = item.ShortName;
                product.Name = item.Name;
                //if (item.Brand != null) product.BrandCategoryName = item.Brand.BrandName;
                if (item.Unit != null) product.UnitCategoryName = item.Unit.Name;
                product.RetailPrice = item.RetailPrice;
                product.PurchasedPrice = item.PurchasedPrice;
                product.ImageUrl = item.ImageUrl;
                product.Currency = item.Currency;
                product.DisplayOrder = item.DisplayOrder;
                product.Description = item.Description;
                product.AllowEarnPoint = item.AllowEarnPoint;
                product.AllowRedeem = item.AllowRedeem;
                product.IsActive = item.IsActive;
                data.Add(product);
            }

            return data;
        }
        public List<ProductViewModel> SearchForOrder(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            var query = UnitOfWork.ProductRepo.GetAll().Where( x => string.IsNullOrEmpty(searchText)  ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()) );
            total = query.Where(m => m.AllowRedeem ==true).Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "price":
                        query = IsAscOrder ? query.OrderBy(x => x.PurchasedPrice) : query.OrderByDescending(x => x.PurchasedPrice);
                        break;
                    case "productname":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Where(m => m.AllowRedeem ==true).Take(size).ToList();
            List<ProductViewModel> data = new List<ProductViewModel>();
            foreach (var item in datas)
            {
                ProductViewModel product = new ProductViewModel();
                product.Id = item.Id;
                if (item.ProductCategory != null) product.ProductCategoryName = item.ProductCategory.Name;
                product.Code = item.Code;
                product.ShortName = item.ShortName;
                product.Name = item.Name;
                //if (item.Brand != null) product.BrandCategoryName = item.Brand.BrandName;
                if (item.Unit != null) product.UnitCategoryName = item.Unit.Name;
                product.RetailPrice = item.RetailPrice;
                product.PurchasedPrice = item.PurchasedPrice;
                product.ImageUrl = item.ImageUrl;
                product.Currency = item.Currency;
                product.Description = item.Description;
                product.IsActive = item.IsActive;
                product.DisplayOrder = item.DisplayOrder;
                data.Add(product);
            }

            return data;
        }

        public List<ProductViewModel> GetActiveProductCategory()
        {
            _logger.LogDebug($"GetActiveProductCategory entered");
            List<ProductViewModel> ret = UnitOfWork.ProductRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                ProductCategoryId = x.ProductCategoryId,
                Code = x.Code,
                ShortName = x.ShortName,
                Name = x.Name,
                //BrandId = x.BrandId,
                RetailPrice = x.RetailPrice,
                PurchasedPrice = x.PurchasedPrice,
                
                Currency = x.Currency,
                IsActive = x.IsActive,
                UnitId = x.UnitId,
                Description = x.Description
                
            }).ToList();
            return ret;

        }

        public List<ProductViewModel> GetProducts()
        {
            _logger.LogDebug($"GetAlls");
            var products = UnitOfWork.ProductRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                ProductCategoryId = x.ProductCategoryId,
                ProductName = x.Name,
                Code = x.Code,
                ShortName = x.ShortName,
                Name = x.Name,
                //BrandId = x.BrandId,
                UnitId = x.UnitId,
                RetailPrice = x.RetailPrice,
                PurchasedPrice = x.PurchasedPrice,
                ImageUrl = x.ImageUrl,
                Currency = x.Currency,
                Description = x.Description,
                IsActive = x.IsActive,
                DisplayOrder = x.DisplayOrder
            }).OrderBy(x => x.Name).ToList();

            return products;
        }

        public ProductViewModel GetProduct(int id)
        {
            _logger.LogDebug($"Detail (Id: {id})");
            var entity = UnitOfWork.ProductRepo.GetById(id);

            if (entity != null)
            {
                var model = new ProductViewModel()
                {
                    Id = entity.Id,
                    ProductCategoryId = entity.ProductCategoryId,
                    Code = entity.Code,
                    ShortName = entity.ShortName,
                    Name = entity.Name,
                    //BrandId = entity.BrandId,
                    UnitId = entity.UnitId,
                    RetailPrice = entity.RetailPrice,
                    PurchasedPrice = entity.PurchasedPrice,
                    uniqueFileName = entity.ImageUrl,
                    Currency = entity.Currency,
                    Description = entity.Description,
                    AllowRedeem = entity.AllowRedeem,
                    AllowEarnPoint =entity.AllowEarnPoint,
                    IsActive = entity.IsActive,
                    DisplayOrder = entity.DisplayOrder
                };

                return model;
            }
            return null;
        }
        public void CreateProduct(ProductViewModel model)
        {
            _logger.LogDebug($"Create Product (Name: {model.Name})");
            //string uniqueFileName = UploadedFile(model);
            var product = UnitOfWork.ProductRepo.GetAll().Select(x => x.Id).ToList();
            string uniqueFileName = UploadedFile(model);
            var entity = new Product
            {

                Id = model.Id,
                ProductCategoryId = model.ProductCategoryId,
                Code = model.Code,
                ShortName = model.ShortName,
                Name = model.Name,
                //BrandId = model.BrandId,
                UnitId = model.UnitId,
                RetailPrice = model.RetailPrice,
                PurchasedPrice = model.PurchasedPrice,
                ImageUrl = uniqueFileName,
                Currency = model.Currency,
                Description = model.Description,
                AllowEarnPoint = model.AllowEarnPoint,
                AllowRedeem = model.AllowRedeem,
                IsActive = model.IsActive,

                //DisplayOrder = model.DisplayOrder
            };

            if (model.DisplayOrder == 0)
            {
                entity.DisplayOrder = product.Count() + 1;
            }
            else { entity.DisplayOrder = model.DisplayOrder; }
            UnitOfWork.ProductRepo.Insert(entity);
        
            UnitOfWork.SaveChanges();
        }

        //private string UploadedFile(ProductViewModel model)
        //{
        //    string uniqueFileName = null;

        //    if (model.ImageUrl != null)
        //    {
        //        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageUrl.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            model.ImageUrl.CopyTo(fileStream);
        //        }
        //    }
        //    return uniqueFileName;
        //}
        public void UpdateProduct(ProductViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.ProductRepo.GetById(model.Id);
            if (entity != null)
            {
                model.uniqueFileName = UploadedFile(model);
                entity.ProductCategoryId = model.ProductCategoryId;
                entity.Code = model.Code;
                entity.ShortName = model.ShortName;
                entity.Name = model.Name;
                //entity.BrandId = model.BrandId;
                entity.UnitId = model.UnitId;
                entity.RetailPrice = model.RetailPrice;
                entity.PurchasedPrice = model.PurchasedPrice;
                if(model.uniqueFileName != null)
                {
                    entity.ImageUrl = model.uniqueFileName;
                }
                entity.Currency = model.Currency;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                entity.AllowEarnPoint = model.AllowEarnPoint;
                entity.AllowRedeem = model.AllowRedeem;

                entity.DisplayOrder = model.DisplayOrder;

                UnitOfWork.ProductRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteProduct(int id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.ProductRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.ProductRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public string UploadedFile(ProductViewModel model)
        {
            string uniqueFileName = null;

            if (model.ImageProfile != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageProfile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageProfile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        #region List Category
        public List<ProductCategoryViewModel> GetProductCategories()
        {
            _logger.LogDebug($"Get Product Categories Enter.");
            var productCategories = UnitOfWork.ProductCategoryRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductCategoryViewModel()
            {
                CateId = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return productCategories;
        }
        //public List<BrandViewModel> GetBrandCategories()
        //{
        //    _logger.LogDebug($"Get Brand Categories Enter.");
        //    var brandCategories = UnitOfWork.BrandRepo.GetAll().Where(x => x.Active).Select(x => new BrandViewModel()
        //    {
        //        BrandId = x.Id,
        //        BrandName = x.BrandName
        //    }).OrderBy(x => x.BrandName).ToList();

        //    return brandCategories;
        //}
        public List<Unit> GetUnitCategories()
        {
            _logger.LogDebug($"Get Unit Categories Enter.");
            var unitCategories = UnitOfWork.UnitRepo.GetAll().Select(x => new Unit()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return unitCategories;
        }

        public List<ProductViewModel> GetProductsAllowEarnPoint()
        {
            _logger.LogDebug($"GetAlls ProductsAllowEarnPoint");
            var products = UnitOfWork.ProductRepo.GetAll().Where(x => x.IsActive && x.AllowEarnPoint).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                ProductName = x.Name,
                
            }).ToList();

            return products;
        }

        public List<BrandViewModel> GetBrandCategories()
        {
            throw new NotImplementedException();
        }
        #endregion List Category


    }
}
