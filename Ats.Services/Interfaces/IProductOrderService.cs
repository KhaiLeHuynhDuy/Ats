using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IProductOrderService 
    {
        List<ProductOrderViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int status, int size, out int total); 
        ProductOrderViewModel GetProductOrder(Guid id);
        void UpdateStatus(ProductOrderSearchViewModel model);
        ProductOrderViewModel GetPrice(Guid id, Guid item, Guid member);
        ProductOrderViewModel CheckMemberItem(ProductOrderViewModel model);
        List<ProductOrderViewModel> GetProductOrders();
        int GetStatisticsProductOrders(DateTime dayb, DateTime daye);
        int GetStatisticsProductOrders();
        int GetStatisticsProductOrdersThisMonth(DateTime firstDayOfMonth, DateTime lastDayOfMonth);
        //bool CreateProductOrder(ProductOrderViewModel model);
        bool Create(ProductOrderViewModel model);
        bool UpdateProductOrder(ProductOrderViewModel model, Claim userid);
        void DeleteProductOrder(Guid id);
        

        
    }
}
