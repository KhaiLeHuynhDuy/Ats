using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ats.Domain.Channel.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Store;
using Microsoft.AspNetCore.Http;

namespace Ats.Services.Interfaces
{
    public interface IStoreService
    {
        List<StoreViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        StoreViewModel GetStore(int id);
        void CreateStore(StoreViewModel model);
        void UpdateStore(StoreViewModel model);
        void DeleteStore(int id);
        List<StoreViewModel> GetStores();
        List<Store> GetStoreCategories();

        Task<object> ImportPurchasedTransCSV(PurchasedTransColumnMappingViewModel purchasedTrans, IFormFile formFile, Guid userId);
    }
}
