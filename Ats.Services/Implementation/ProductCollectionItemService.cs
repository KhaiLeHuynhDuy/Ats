using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Domain;
using Ats.Domain.Store.Models;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ats.Services.Implementation
{
    public class ProductCollectionItemService : BaseService, IProductCollectionItemService
    {
        private IConfiguration _config;
        private int pageSize;

        private IStoreService _storeservice;
        public ProductCollectionItemService(IUnitOfWork unitOfWork, IStoreService storeService, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");

            _storeservice = storeService;
        }

        public List<ProductCollectionItemViewModel> Search(string searchText, int? productId, Guid? productCollectionId, int? stockFrom, int? stockTo,
            string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                                        && ((!String.IsNullOrEmpty(x.Remark) && x.Remark.ToLower().Contains(searchText.ToLower()))
                                        || (!String.IsNullOrEmpty(x.Product.Name) && x.Product.Name.ToLower().Contains(searchText.ToLower())) ||
                                        (!String.IsNullOrEmpty(x.ProductCollection.Name) && x.ProductCollection.Name.ToLower().Contains(searchText.ToLower())))))
                                        && (productId == 0 || productId == null ? true : x.ProductId == productId)
                                        && (productCollectionId == Guid.Empty || productCollectionId == null ? true : x.ProductCollectionId == productCollectionId) 
                                        );


            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                   
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<ProductCollectionItemViewModel> data = new List<ProductCollectionItemViewModel>();
            foreach (var item in datas)
            {
                ProductCollectionItemViewModel itemProductCollectionItem = new ProductCollectionItemViewModel();
                itemProductCollectionItem.Id = item.Id;
                itemProductCollectionItem.Stock = item.Stock;
                itemProductCollectionItem.Price = item.Price;
                itemProductCollectionItem.IsActive = item.IsActive;
                itemProductCollectionItem.DisplayOrder = item.DisplayOrder;
                itemProductCollectionItem.Remark = item.Remark;
               
                itemProductCollectionItem.ProductId = item.ProductId;
                itemProductCollectionItem.ProductName = item.Product != null ? item.Product.Name : "";

                itemProductCollectionItem.ProductCollectionId = item.ProductCollectionId;
                itemProductCollectionItem.ProductCollectionName = item.ProductCollection != null ? item.ProductCollection.Name : "";

                data.Add(itemProductCollectionItem);
            }

            return data;
        }
        public List<ProductCollectionItemViewModel> SearchFollowStock(string searchText, int? productId, Guid? productCollectionId, int? stockFrom, int? stockTo,
            string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                                        && ((!String.IsNullOrEmpty(x.Remark) && x.Remark.ToLower().Contains(searchText.ToLower()))
                                        || (!String.IsNullOrEmpty(x.Product.Name) && x.Product.Name.ToLower().Contains(searchText.ToLower())) ||
                                        (!String.IsNullOrEmpty(x.ProductCollection.Name) && x.ProductCollection.Name.ToLower().Contains(searchText.ToLower())))))
                                        && (productId == 0 || productId == null ? true : x.ProductId == productId)
                                        && (productCollectionId == Guid.Empty || productCollectionId == null ? true : x.ProductCollectionId == productCollectionId)
                                        );


            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;

                }
            }
            var datas = query.Where(m => m.Stock > 0 && (m.ProductCollection.EffectiveDateBegin <= DateTime.Today && m.ProductCollection.EffectiveDateEnd >= DateTime.Today)
                                    || (m.ProductCollection.EffectiveDateBegin <= DateTime.Today && m.ProductCollection.EffectiveDateEnd == null)).Skip((page - 1) * size).Take(size).ToList();
            List<ProductCollectionItemViewModel> data = new List<ProductCollectionItemViewModel>();
            foreach (var item in datas)
            {
                ProductCollectionItemViewModel itemProductCollectionItem = new ProductCollectionItemViewModel();
                itemProductCollectionItem.Id = item.Id;
                itemProductCollectionItem.Stock = item.Stock;
                itemProductCollectionItem.Price = item.Price;
                itemProductCollectionItem.IsActive = item.IsActive;
                itemProductCollectionItem.DisplayOrder = item.DisplayOrder;
                itemProductCollectionItem.Remark = item.Remark;

                itemProductCollectionItem.ProductId = item.ProductId;
                itemProductCollectionItem.ProductName = item.Product != null ? item.Product.Name : "";

                itemProductCollectionItem.ProductCollectionId = item.ProductCollectionId;
                itemProductCollectionItem.ProductCollectionName = item.ProductCollection != null ? item.ProductCollection.Name : "";

                data.Add(itemProductCollectionItem);
            }

            return data;
        }

        public List<ProductCollectionItemViewModel> SearchItemOfCollect(string searchText, int? productId, Guid? productCollectionId, int? stockFrom, int? stockTo,
            string orderField, bool IsAscOrder, int page, int size, out int total, Guid id)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();



            var query = UnitOfWork.ProductCollectionItemRepo.GetAll().Where( x => x.ProductCollectionId == id && (x.ProductCollectionId == id && String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                                        &&  ((!String.IsNullOrEmpty(x.Remark) && x.Remark.ToLower().Contains(searchText.ToLower())) ||
                                        (!String.IsNullOrEmpty(x.Product.Name) && x.Product.Name.ToLower().Contains(searchText.ToLower())) ||
                                        (!String.IsNullOrEmpty(x.Price.ToString()) && x.Price.ToString().ToLower().Contains(searchText.ToLower())) ||
                                        (!String.IsNullOrEmpty(x.ProductCollection.Name) && x.ProductCollection.Name.ToLower().Contains(searchText.ToLower())))))
                                        //&& (productId == 0 || productId == null ? true : x.ProductId == productId)
                                        //&& (productCollectionId == Guid.Empty || productCollectionId == null ? true : x.ProductCollectionId == productCollectionId) 
                                        );


            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "productname":
                        query = IsAscOrder ? query.OrderBy(x => x.Product.Name) : query.OrderByDescending(x => x.Product.Name);
                        break;
                    case "displayorder":
                        query = IsAscOrder ? query.OrderBy(x => x.DisplayOrder) : query.OrderByDescending(x => x.DisplayOrder);
                        break;
                    case "price":
                        query = IsAscOrder ? query.OrderBy(x => x.Price) : query.OrderByDescending(x => x.Price);
                        break;
                    case "stock":
                        query = IsAscOrder ? query.OrderBy(x => x.Stock) : query.OrderByDescending(x => x.Stock);
                        break;
                    case "active":
                        query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                        break; 


                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<ProductCollectionItemViewModel> data = new List<ProductCollectionItemViewModel>();
            foreach (var item in datas)
            {
                ProductCollectionItemViewModel itemProductCollectionItem = new ProductCollectionItemViewModel();
                itemProductCollectionItem.Id = item.Id;
                itemProductCollectionItem.Stock = item.Stock;
                itemProductCollectionItem.Price = item.Price;
                itemProductCollectionItem.IsActive = item.IsActive;
                itemProductCollectionItem.DisplayOrder = item.DisplayOrder;
                itemProductCollectionItem.Remark = item.Remark;

                itemProductCollectionItem.ProductId = item.ProductId;
                itemProductCollectionItem.ProductName = item.Product != null ? item.Product.Name : "";

                itemProductCollectionItem.ProductCollectionId = item.ProductCollectionId;
                itemProductCollectionItem.ProductCollectionName = item.ProductCollection != null ? item.ProductCollection.Name : "";

                data.Add(itemProductCollectionItem);
            }

            return data;
        }


        public List<ProductCollectionItemViewModel> Gets()
        {
            _logger.LogDebug($"Get all Product CollectionItem ");
            var productCollectionItems = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductCollectionItemViewModel()
            {
                Id = x.Id,
                ProductCollectionId = x.ProductCollectionId,
                ProductId = x.ProductId,
                ProductCollectionName = x.ProductCollection.Name,
                ProductName = x.Product.Name,
                Stock = x.Stock,
                Remark = x.Remark,
                DisplayOrder = x.DisplayOrder
            }).OrderBy(x => x.Id).ToList();

            return productCollectionItems;
        }


        public ProductCollectionItemViewModel Get(Guid id)
        {
            _logger.LogDebug($"Detail Product Collection Item (Id: {id})");
            var entity = UnitOfWork.ProductCollectionItemRepo.GetById(id);
            if (entity != null)
            {
                var model = new ProductCollectionItemViewModel()
                {
                    Id = entity.Id,                 
                    ProductId = entity.ProductId,
                    ProductCollectionId = entity.ProductCollectionId,
                    Stock = entity.Stock,
                    Price = entity.Price,
                    IsActive = entity.IsActive,
                    DisplayOrder = entity.DisplayOrder,
                    Remark = entity.Remark,
                    
                   
                };
               
                if (entity.Product != null)
                {
                    model.ProductName = entity.Product.Name;
                }
                else
                {
                    model.ProductName = " ";
                }

                if (entity.ProductCollection != null)
                {
                    model.ProductCollectionName = entity.ProductCollection.Name;
                }
                else
                {
                    model.ProductCollectionName = " ";
                }

                return model;
            }
            return null;
        }

        public ProductCollectionItemViewModel GetProductItem(Guid iditem)
        {
            _logger.LogDebug($"Detail Product Collection Item (Id: {iditem})");
            var entity = UnitOfWork.ProductCollectionItemRepo.GetById(iditem);
            if (entity != null)
            {
                var model = new ProductCollectionItemViewModel()
                {
                    Id = entity.Id,
                    ProductId = entity.ProductId,
                    ProductCollectionId = entity.ProductCollectionId,
                    Stock = entity.Stock,
                    Price = entity.Price,
                    IsActive = entity.IsActive,
                    DisplayOrder = entity.DisplayOrder,
                    Remark = entity.Remark, 
                }; 
                if (entity.Product != null)  {  model.ProductName = entity.Product.Name;  }
                else {   model.ProductName = " ";  }
                if (entity.ProductCollection != null) {  model.ProductCollectionName = entity.ProductCollection.Name; }
                else  {  model.ProductCollectionName = " "; } 
                return model;
            }
            return null;
        }

        public void Create(ProductCollectionItemViewModel model)
        {
            
            _logger.LogDebug($"Create Gift (Name: {model})");
            //var product = UnitOfWork.ProductRepo.GetById(model.ProductId);

            ProductCollectionItem entity = new ProductCollectionItem();
            entity.Id = Guid.NewGuid();
           
            entity.Price = model.Price;
            entity.Stock = model.Stock;
            entity.IsActive = model.IsActive;
            entity.DisplayOrder = model.DisplayOrder;
            entity.Remark = model.Remark;

            entity.ProductId  = model.ProductId;
            entity.ProductCollectionId = model.ProductCollectionId;
           
            UnitOfWork.ProductCollectionItemRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }


        public void Update(ProductCollectionItemViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.ProductCollectionItemRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Price = model.Price;
                entity.Stock = model.Stock;
                 
                entity.DisplayOrder = model.DisplayOrder;
                entity.Remark = model.Remark;
                entity.IsActive = model.IsActive;

                entity.ProductId = model.ProductId;
                entity.ProductCollectionId = model.ProductCollectionId;

                UnitOfWork.ProductCollectionItemRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void Delete(Guid id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.ProductCollectionItemRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.ProductCollectionItemRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }     
    }
}
