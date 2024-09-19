using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Store.Models;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IProductCollectionService
    {
        List<ProductCollectionViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        ProductCollectionViewModel Get(Guid id, Guid iditem); 
        void Create(ProductCollectionViewModel model); 
        void CreateItem(ProductCollectionSearchViewModel model);
        void Update(ProductCollectionViewModel model);
        void Delete(Guid id, Guid itemid);
      
        List<ProductCollectionViewModel> Gets();
        List<Product> GetProducts(Guid id);
        List<ProductViewModel> GetListProducts(List<ProductViewModel> list, Guid id);
        List<ProductCollectionItemViewModel> GetListItemOfCollection(List<ProductCollectionItemViewModel> list, Guid id);
        List<Product> GetProductsCollection(Guid id);
        List<ProductCollectionItem> GetProductsCollectionItem(Guid id);
        List<ProductCollectionItemViewModel> GetItems(Guid id);



        void CreateProductItem(ProductCollectionSearchViewModel model); 
    }
}
