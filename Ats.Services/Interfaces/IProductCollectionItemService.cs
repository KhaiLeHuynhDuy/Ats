using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IProductCollectionItemService
    {
        List<ProductCollectionItemViewModel> Search(string searchText, int? productId, Guid? productCollectionId, int? stockFrom, int? stockTo, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ProductCollectionItemViewModel> SearchFollowStock(string searchText, int? productId, Guid? productCollectionId, int? stockFrom, int? stockTo, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ProductCollectionItemViewModel> SearchItemOfCollect(string searchText, int? productId, Guid? productCollectionId, int? stockFrom, int? stockTo, string orderField, bool IsAscOrder, int page, int size, out int total, Guid id);
        ProductCollectionItemViewModel Get(Guid id);
        ProductCollectionItemViewModel GetProductItem(Guid iditem);
        void Create(ProductCollectionItemViewModel model);
        void Update(ProductCollectionItemViewModel model);
        void Delete(Guid id);

        List<ProductCollectionItemViewModel> Gets();
    }
}
