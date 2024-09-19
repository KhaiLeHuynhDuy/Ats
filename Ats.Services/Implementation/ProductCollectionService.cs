using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Domain;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ats.Services.Implementation
{
    public class ProductCollectionService : BaseService, IProductCollectionService
    {
        private IConfiguration _config;
        private int pageSize;

        private IStoreService _storeservice;
        public ProductCollectionService(IUnitOfWork unitOfWork, IStoreService storeService, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");

            _storeservice = storeService;
        }

        public List<ProductCollectionViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.ProductCollectionRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                              x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()));


            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "collectionname":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                    case "startfrom":
                        query = IsAscOrder ? query.OrderBy(x => x.EffectiveDateBegin) : query.OrderByDescending(x => x.EffectiveDateBegin);
                        break;
                    case "endto":
                        query = IsAscOrder ? query.OrderBy(x => x.EffectiveDateEnd) : query.OrderByDescending(x => x.EffectiveDateEnd);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<ProductCollectionViewModel> data = new List<ProductCollectionViewModel>();
            foreach (var item in datas)
            {
                ProductCollectionViewModel itemProductCollection = new ProductCollectionViewModel();
                itemProductCollection.Id = item.Id;
                itemProductCollection.Name = item.Name;
                itemProductCollection.IsActive = item.IsActive;
                itemProductCollection.Description = item.Description;

                itemProductCollection.EffectiveDateBegins = string.Format("{0:dd/MM/yyyy}", item.EffectiveDateBegin);//
                itemProductCollection.EffectiveDateEnds = string.Format("{0:dd/MM/yyyy}", item.EffectiveDateEnd);

                data.Add(itemProductCollection);
            }

            return data;
        }

        public ProductCollectionViewModel Get(Guid id, Guid iditem)
        {
            _logger.LogDebug($"Detail ProductCollection (Id: {id})");
            var entity = UnitOfWork.ProductCollectionRepo.GetById(id);
            if (entity != null)
            {
                var model = new ProductCollectionViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    Description = entity.Description,
                    EffectiveDateBegin = entity.EffectiveDateBegin,
                    EffectiveDateEnd = entity.EffectiveDateEnd,

                };

                if (iditem == Guid.Empty)
                {
                    model.Items = entity.ProductItems.Select(x => new DisplayItem()  // lấy hết
                    {
                        Value = x.ProductId.ToString(),
                        Selected = true
                    }).ToList();
                }
                else
                {
                    model.Items = entity.ProductItems.Where(m => m.Id == iditem).Select(x => new DisplayItem() // chỉ lấy select
                    {
                        Value = x.ProductId.ToString(),
                        Selected = true
                    }).ToList();
                }
                return model;
            }
            return null;
        }

        public List<ProductCollectionViewModel> Gets()
        {
            _logger.LogDebug($"Get all Gift");
            var collection = UnitOfWork.ProductCollectionRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductCollectionViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                Description = x.Description,

            }).OrderBy(x => x.Name).ToList();

            return collection;
        }
        public List<Product> GetProducts(Guid id)
        {
            List<Product> CheckAProduct = new List<Product>();
            var getall = UnitOfWork.ProductRepo.GetAll().ToList();
            for (int i = 0; i < getall.Count(); i++)
            {
                var collectItem = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(m => m.ProductCollectionId == id && m.ProductId == getall[i].Id).ToList();

                if (collectItem.Count() == 0)
                {
                    CheckAProduct.Add(getall[i]);
                }

            }
            return CheckAProduct;
        }
        public List<ProductViewModel> GetListProducts(List<ProductViewModel> list, Guid id)
        {
            List<ProductViewModel> CheckAProduct = new List<ProductViewModel>();
            //var getall = UnitOfWork.ProductRepo.GetAll().ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                var collectItem = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(m => m.ProductCollectionId == id && m.ProductId == list[i].Id).ToList();

                if (collectItem.Count() == 0)
                {
                    CheckAProduct.Add(list[i]);
                }

            }
            return CheckAProduct;
        }


        public List<ProductCollectionItemViewModel> GetListItemOfCollection(List<ProductCollectionItemViewModel> list, Guid id)
        {
            var productitems = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(m => m.ProductCollectionId == id).Select(x => new ProductCollectionItemViewModel()
            {
                Id = x.Id,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive,
                Price = x.Price,
                Stock = x.Stock,
                ProductName = x.Product.Name,
                ProductCollectionName = x.ProductCollection.Name,
                ProductId = x.ProductId,
                Remark = x.Remark,
                ProductCollectionId = x.ProductCollectionId,

            }).OrderByDescending(x => x.Price).ToList();

            return productitems;
        }


        public List<Product> GetProductsCollection(Guid id)
        {
            List<Product> ProductCollection = new List<Product>();
            var getall = UnitOfWork.ProductRepo.GetAll().ToList();
            for (int i = 0; i < getall.Count(); i++)
            {
                var collectItem = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(m => m.ProductCollectionId == id && m.ProductId == getall[i].Id).ToList();

                if (collectItem.Count() != 0)
                {
                    ProductCollection.Add(getall[i]);
                }

            }
            return ProductCollection;

        }

        public List<ProductCollectionItem> GetProductsCollectionItem(Guid id)
        {
            List<ProductCollectionItem> ProductCollectionItem = new List<ProductCollectionItem>();
            ProductCollectionItem = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(m => m.ProductCollectionId == id).ToList();

            return ProductCollectionItem;

        }

        public List<ProductCollectionItemViewModel> GetItems(Guid id)
        {
            List<ProductCollectionItemViewModel> ProductCollectionItem = new List<ProductCollectionItemViewModel>();

            var item = UnitOfWork.ProductCollectionItemRepo.GetAll().Select(x => new ProductCollectionItemViewModel()
            {
                Id = x.Id,
                ProductCollectionId = x.ProductCollectionId,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                ProductCollectionName = x.ProductCollection.Name,
                Remark = x.Remark,
                Stock = x.Stock,
                Price = x.Price,
                DisplayOrder = x.DisplayOrder

            }).Where(m => m.ProductCollectionId == id).OrderBy(x => x.Price).ToList();

            return item;

        }

        public void Create(ProductCollectionViewModel model)
        {
            _logger.LogDebug($"Create ProductCollection (Name: {model.Name})");

            ProductCollection entity = new ProductCollection();
            entity.Id = Guid.NewGuid();
            entity.Name = model.Name;
            entity.IsActive = true;
            entity.Description = model.Description;
            entity.EffectiveDateBegin = model.EffectiveDateBegin;
            entity.EffectiveDateEnd = model.EffectiveDateEnd;

            UnitOfWork.ProductCollectionRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void CreateItem(ProductCollectionSearchViewModel model)
        {
            var checkCLI = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(m => m.ProductId == model.Product.Id && m.ProductCollectionId == model.ProductCollection.Id).FirstOrDefault();
            if (checkCLI == null)
            {
                ProductCollectionItem entity = new ProductCollectionItem();
                entity.Id = Guid.NewGuid();
                entity.ProductId = model.Product.Id;
                entity.ProductCollectionId = model.ProductCollection.Id;
                entity.Price = model.ProductCollectionItem.Price;
                entity.DisplayOrder = model.Product.DisplayOrder;
                entity.IsActive = true;
                entity.Remark = "";

                UnitOfWork.ProductCollectionItemRepo.Insert(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void Update(ProductCollectionViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.ProductCollectionRepo.GetById(model.Id);
            if (entity != null)
            {

                entity.Name = model.Name;
                entity.IsActive = model.IsActive;
                entity.Description = model.Description;
                entity.EffectiveDateBegin = model.EffectiveDateBegin;
                entity.EffectiveDateEnd = model.EffectiveDateEnd;

                UnitOfWork.ProductCollectionRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public void Delete(Guid id, Guid iditem)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            if (iditem != Guid.Empty) // delete item
            {
                var collectionitem = UnitOfWork.ProductCollectionItemRepo.GetById(iditem);
                if (collectionitem != null)
                {
                    UnitOfWork.ProductCollectionItemRepo.Delete(collectionitem);
                    UnitOfWork.SaveChanges();
                }
            }
            else
            {
                var entity = UnitOfWork.ProductCollectionRepo.GetById(id);
                if (entity != null)
                {
                    var item = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(m => m.ProductCollectionId == id).ToList();
                    if (item.Count() != 0)
                    {
                        for (int i = 0; i < item.Count(); i++)
                        {
                            var collectionitem = UnitOfWork.ProductCollectionItemRepo.GetById(item[i].Id);
                            UnitOfWork.ProductCollectionItemRepo.Delete(collectionitem);
                            UnitOfWork.SaveChanges();
                        }
                    }
                    UnitOfWork.ProductCollectionRepo.Delete(entity);
                    UnitOfWork.SaveChanges();
                }
            }
        }

        private void CreateCollectionItem(ProductCollectionSearchViewModel model, Product productItem)
        {
            ProductCollectionItem entity = new ProductCollectionItem();
            entity.Id = Guid.NewGuid();
            entity.ProductId = productItem.Id;
            entity.ProductCollectionId = model.ProductCollection.Id;
            entity.Price = model.ProductCollectionItem.Price;
            entity.DisplayOrder = model.ProductCollectionItem.DisplayOrder;
            entity.Stock = model.ProductCollectionItem.Stock;
            entity.IsActive = true;
            entity.Remark = model.ProductCollectionItem.Remark;
            UnitOfWork.ProductCollectionItemRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        private void EditCollectionItem(ProductCollectionSearchViewModel model, Product productItem, ProductCollectionItem edititem)
        { 
            edititem.Id = edititem.Id; 
            edititem.ProductId = productItem.Id;
            edititem.ProductCollectionId = model.ProductCollection.Id;
            edititem.Price = model.ProductCollectionItem.Price;
            edititem.DisplayOrder = model.ProductCollectionItem.DisplayOrder;
            edititem.Stock = model.ProductCollectionItem.Stock;
            edititem.IsActive = true;
            edititem.Remark = model.ProductCollectionItem.Remark;
            UnitOfWork.ProductCollectionItemRepo.Update(edititem);
            UnitOfWork.SaveChanges();
        }


        public void CreateProductItem(ProductCollectionSearchViewModel model)
        {
            foreach (DisplayItem item in model.ProductCollection.Items)
            {
                if (item.Selected)
                {
                    var checkCLI = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(m => m.ProductCollectionId == model.ProductCollection.Id && m.ProductId == int.Parse(item.Value)).FirstOrDefault();
                    var product = UnitOfWork.ProductRepo.GetById(int.Parse(item.Value));

                    if (model.ProductCollectionItem.Id == Guid.Empty) //Create
                    {
                        if (checkCLI == null)
                        {
                            CreateCollectionItem(model, product);
                        }
                        else
                        {
                            EditCollectionItem(model, product, checkCLI);
                        }
                    }
                    else // Edit and Create ( khi edit chon them product )
                    { 
                        if (checkCLI == null) //Create
                        {
                            CreateCollectionItem(model, product);
                        }
                        else //Edit
                        {
                            EditCollectionItem(model, product, checkCLI);
                        }
                    }
                }
            }
        }


    }
}
