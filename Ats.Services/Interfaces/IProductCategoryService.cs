using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IProductCategoryService
    {
        List<ProductCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        ProductCategoryViewModel GetProductCategory(int id);
        void CreateProductCategory(ProductCategoryViewModel model);
        void UpdateProductCategory(ProductCategoryViewModel model);
        void DeleteProductCategory(int id);
        List<ProductCategoryViewModel> GetProductCategorys();
    }
}
