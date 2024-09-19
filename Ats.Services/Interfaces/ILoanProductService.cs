using Ats.Domain.Lead.Models;
using Ats.Models.Lead;
using Ats.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;
using static Ats.Commons.Constants;

namespace Ats.Services.Interfaces
{
    public interface ILoanProductService
    {
        List<LoanPackage> GetLoanProducts();
        LoanPackage GetLoanProduct(int id);

        #region Product Category
        List<ProductCategoryViewModel> SearchProductCategory(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ProductCategoryViewModel> GetProductCategories();
        ProductCategoryViewModel GetProductCategory(int id);
        void CreateProductCategory(ProductCategoryViewModel model);
        void UpdateProductCategory(ProductCategoryViewModel model);
        void DeleteProductCategory(int id);
        #endregion

        #region Product Property
        List<ProductPropertyViewModel> SearchProductProperty(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        List<ProductPropertyViewModel> GetProductProperties();
        ProductPropertyViewModel GetProductProperty(int id);
        void CreateProductProperty(ProductPropertyViewModel model);
        void UpdateProductProperty(ProductPropertyViewModel model);
        void DeleteProductProperty(int id);
        #endregion

        #region Loan Product
        List<LoanProductViewModel> SearchProduct(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        LoanProductViewModel GetProduct(int id);
        void CreateProduct(LoanProductViewModel model);
        void UpdateProduct(LoanProductViewModel model);
        void DeleteProduct(int id);
        #endregion

        #region Product Attribute
        bool CheckExistAddProductAttribute(int productId, int productPropertyId);
        void AddProductAttribute(ProductAttributeModel model);
        ProductAttributeModel GetProductAttribute(int id);
        void UpdateProductAttribute(ProductAttributeModel model);
        void DeleteProductAttribute(int id);
        #endregion

    }
}
