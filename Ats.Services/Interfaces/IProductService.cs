using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Brand;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IProductService 
    {
        string UploadedFile(ProductViewModel model);

        List<ProductViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ProductViewModel> SearchForOrder(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ProductViewModel> GetActiveProductCategory();
        ProductViewModel GetProduct(int id);
        void CreateProduct(ProductViewModel model);
        void UpdateProduct(ProductViewModel model);
        void DeleteProduct(int id);
        List<ProductViewModel> GetProducts();

        List<ProductViewModel> GetProductsAllowEarnPoint();

        #region List Category
        List<ProductCategoryViewModel> GetProductCategories();
        List<Unit> GetUnitCategories();
        List<BrandViewModel> GetBrandCategories();
        #endregion List Category
    }
}
