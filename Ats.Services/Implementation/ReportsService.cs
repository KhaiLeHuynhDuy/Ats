using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Models.Store;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Ats.Models.Member;
using Ats.Models.Reports;
using Ats.Domain.Member.Models;
using Ats.Commons;
using Ats.Commons.Utilities;
using System.Linq.Expressions;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Ats.Models.ResponeResult;
using Microsoft.EntityFrameworkCore;

namespace Ats.Services.Implementation
{
    public class ReportsService : BaseService, IReportsService
    {
        private IConfiguration _config;
        private int pageSize;

        public ReportsService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public async Task<List<MemberReportsViewModel>> ExportMemberReport( string dateFrom, string dateTo)
        {
            _logger.LogDebug($"Export Member Report service");

            DateTime? _dateFrom, _dateTo;
            if (dateFrom != null) { _dateFrom = DateTime.Parse(dateFrom).StartOfMonth(); }
            else { _dateFrom = null; }
            if (dateTo != null) { _dateTo = DateTime.Parse(dateTo).EndOfMonth(); }
            else { _dateTo = null; }

            var query = UnitOfWork.MemberRepo.GetAll().Where(x => (_dateFrom != null && dateTo == null && x.RegisterDate >= _dateFrom)
           || _dateTo != null && _dateFrom == null && x.RegisterDate <= _dateTo
           || _dateTo != null && _dateFrom != null && x.RegisterDate >= _dateFrom && x.RegisterDate <= _dateTo
            ).OrderByDescending(y=>y.RegisterDate);
            var datas = query.ToList();
            List<MemberReportsViewModel> data = new List<MemberReportsViewModel>();
            

            foreach (var item in datas)
            {
                MemberReportsViewModel itemMember = new MemberReportsViewModel();
                {
                    itemMember.Id = item.Id;
                    itemMember.MemberCode = item.MemberCode;
                    itemMember.FirstName = item.FirstName;
                    itemMember.LastName = item.LastName;
                    itemMember.Email = item.Email;
                    itemMember.PhoneNumber = item.PhoneNumber;
                    itemMember.MaleOrFemale = item.Gender != null ? item.Gender != true ? "Nữ" : "Nam" : "";
                    itemMember.Point = item.MemberWallets.First().Point;
                    itemMember.TierName = item.MemberLoyaltyTiers.OrderByDescending(p => p.ActiveFrom <= DateTime.UtcNow && DateTime.UtcNow <= p.ActiveEnd ? p.ActiveEnd : null).FirstOrDefault().LoyaltyTier.TierName;
                    itemMember.CreateDate = string.Format("{0:dd/MM/yyyy}", item.RegisterDate);
                    itemMember.DistrictName = item.Address.District != null ? item.Address.District.Name : "";
                    itemMember.ProvinceName = item.Address.Province != null ? item.Address.Province.Name : "";
                    itemMember.YOB = item.YOB != 0 ? item.YOB.ToString() : null;
                    itemMember.StringStatus = item.MemberWallets.FirstOrDefault().Active != true ? "Ngừng Hoạt Động" : "Đã Kích Hoạt";

                }
                data.Add(itemMember);
            }
        
            return await Task.Run(() => data);
        }

        public async Task<List<ProductQrReportsViewModel>> ExportProductQrReport(string[] productId, string dateFrom, string dateTo)
        {
            _logger.LogDebug($"Export Product Qr Report");
            DateTime? _dateFrom, _dateTo;
            if (dateFrom != null) { _dateFrom = DateTime.Parse(dateFrom).StartOfMonth(); }
            else { _dateFrom = null; }
            if (dateTo != null) { _dateTo = DateTime.Parse(dateTo).EndOfMonth(); }
            else { _dateTo = null; }

            var QrProduct = UnitOfWork.MemberLoyaltyTransactionRepo.GetAll().Where(q => q.PointTypeId == (int)LOYAlTY_POINT_TYPE.CUSTOM_POINTS);
            var LoyaltyTrans = QrProduct.Select(x => new LoyaltyTransViewModel
            {
                Id = x.Id,
                ProductId = int.Parse(x.RefId)
            }).ToList();

            //string value =  productId.JoinToString();
            //string[] vals = value.Split(';');
            int Product = 0;
            if (productId.Length != 0)
            {
                var Products = Array.ConvertAll(productId, s => int.TryParse(s, out Product) ? Product : 0).Where(x => x != 0).ToArray();
                var newLoyaltyTrans = LoyaltyTrans.Where(x => Products.Contains(x.ProductId)).Select(t => t.Id);
                var getall = QrProduct.Where(x => newLoyaltyTrans.Contains(x.Id));
                var query2 = getall.Where(x => (_dateFrom == null && _dateTo == null && x.Id != null)
                  || (_dateFrom != null && _dateTo == null && x.AddedTimestamp >= _dateFrom)
                  || (_dateTo != null && _dateFrom == null && x.AddedTimestamp <= _dateTo)
                  || (_dateTo != null && _dateFrom != null && x.AddedTimestamp >= _dateFrom && x.AddedTimestamp <= _dateTo)

                  ).OrderByDescending(x => x.AddedTimestamp);


             var datas = query2.ToList();
                List<ProductQrReportsViewModel> productQrReports = new List<ProductQrReportsViewModel>();
                foreach (var item in datas)
                {
                    ProductQrReportsViewModel itemQr = new ProductQrReportsViewModel();
                    {
                        itemQr.STT = productQrReports.Count() + 1;
                        itemQr.CodeScanDate = item.AddedTimestamp.ToString();
                        itemQr.ProductName = UnitOfWork.ProductRepo.GetAll().Where(p => p.Id.ToString() == item.RefId).FirstOrDefault().Name;
                        itemQr.ProductCode = UnitOfWork.ProductRepo.GetAll().Where(p => p.Id.ToString() == item.RefId).FirstOrDefault().Code;
                        itemQr.FullProduct = $"{itemQr.ProductCode} - {itemQr.ProductName}";

                        itemQr.MemberCode = item.MemberWallet.Member.MemberCode;
                        itemQr.LastName = item.MemberWallet.Member.LastName != null ? item.MemberWallet.Member.LastName : "";
                        itemQr.FirstName = item.MemberWallet.Member.FirstName != null ? item.MemberWallet.Member.FirstName : "";
                        itemQr.FullName = itemQr.LastName + " " + itemQr.FirstName;
                        itemQr.BillNo = item.InvoiceNo;
                        itemQr.PointTypeId = item.PointTypeId;


                    }

                    productQrReports.Add(itemQr);
                }
                return await Task.Run(() => productQrReports);
            }
            var query = QrProduct.Where(x => x.Id != null
               && (_dateFrom != null && _dateTo == null && x.AddedTimestamp >= _dateFrom)
               || (_dateTo != null && _dateFrom == null && x.AddedTimestamp <= _dateTo)
               || (_dateTo != null && _dateFrom != null && x.AddedTimestamp >= _dateFrom)
               ).OrderByDescending(x => x.AddedTimestamp);

        
            var datass = query.ToList();
         
            List<ProductQrReportsViewModel> data = new List<ProductQrReportsViewModel>();
            foreach (var item in datass)
            {
                ProductQrReportsViewModel itemQr = new ProductQrReportsViewModel();
                {
                    itemQr.STT = data.Count() + 1;
                    itemQr.CodeScanDate = item.AddedTimestamp.ToString();
                    itemQr.ProductName = UnitOfWork.ProductRepo.GetAll().Where(p => p.Id.ToString() == item.RefId).FirstOrDefault().Name;
                    itemQr.ProductCode = UnitOfWork.ProductRepo.GetAll().Where(p => p.Id.ToString() == item.RefId).FirstOrDefault().Code;
                    itemQr.FullProduct = $"{itemQr.ProductCode} - {itemQr.ProductName}";

                    itemQr.MemberCode = item.MemberWallet.Member.MemberCode;
                    itemQr.LastName = item.MemberWallet.Member.LastName != null ? item.MemberWallet.Member.LastName : "";
                    itemQr.FirstName = item.MemberWallet.Member.FirstName != null ? item.MemberWallet.Member.FirstName : "";
                    itemQr.FullName = itemQr.LastName + " " + itemQr.FirstName;
                    itemQr.BillNo = item.InvoiceNo;
                    itemQr.PointTypeId = item.PointTypeId;

                }

                data.Add(itemQr);
            }

            return await Task.Run(() => data);
        }

        public async Task<List<RedemptionReportsViewModel>> ExportRedemptionReport(Guid? collectionId)
        {
            var CollectionItem = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(x => x.Id == x.ProductOrders.FirstOrDefault().ProductItemId);
            var query = CollectionItem.Where(x => collectionId != null && x.ProductCollectionId == collectionId);
         
            var datas = query.ToList();
            List<RedemptionReportsViewModel> data = new List<RedemptionReportsViewModel>();
            foreach (var item in datas)
            {
                RedemptionReportsViewModel itemRedem = new RedemptionReportsViewModel();
                {
                    itemRedem.ProductCode = item.Product.Code;
                    itemRedem.ProductName = item.Product.Name;
                    itemRedem.TotalNewOrder = item.ProductOrders != null ? item.ProductOrders.Where(x => x.OrderStatus == PRODUCT_ORDER_STATUS.NEW).Count() : 0;
                    itemRedem.TotalInTransit = item.ProductOrders != null ? item.ProductOrders.Where(x => x.OrderStatus == PRODUCT_ORDER_STATUS.ON_DELIVERY).Count() : 0;
                    itemRedem.TotalCompleted = item.ProductOrders != null ? item.ProductOrders.Where(x => x.OrderStatus == PRODUCT_ORDER_STATUS.COMPLETE).Count() : 0;
                    itemRedem.TotalCanceled = item.ProductOrders != null ? item.ProductOrders.Where(x => x.OrderStatus == PRODUCT_ORDER_STATUS.CANCEL).Count() : 0;


                    //itemRedem.Status= item.pr
                }

                data.Add(itemRedem);
            }
            return await Task.Run(() => data);
        }

        public List<ProductCollectionViewModel> GetCollections()
        {
            _logger.LogDebug($"Get list Collection ");
            var collections = UnitOfWork.ProductCollectionRepo.GetAll().Where(x => x.IsActive == true ).Select(x => new ProductCollectionViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return collections;
        }

        public List<ProductViewModel> GetProducts()
        {
            _logger.LogDebug($"Get list product ");
            var jobTitles = UnitOfWork.ProductRepo.GetAll().Where(x => x.IsActive == true && x.AllowEarnPoint == true).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Code=x.Code,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return jobTitles;
        }

        public  List<MemberReportsViewModel> SearchMemberReport(string searchText, string? dateFrom, string? dateTo, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Member Report service Search={searchText}, Page={page}");

            DateTime? _dateFrom, _dateTo;
            if (dateFrom != null) { _dateFrom = DateTime.Parse(dateFrom).StartOfMonth(); }
            else { _dateFrom = null; }
            if (dateTo != null) { _dateTo = DateTime.Parse(dateTo).EndOfMonth(); }
            else { _dateTo = null; }
            var query = UnitOfWork.MemberRepo.GetAll().Where(x =>  (_dateFrom != null && dateTo == null && x.RegisterDate >= _dateFrom)
            || _dateTo != null && _dateFrom == null && x.RegisterDate <= _dateTo
            || _dateTo != null && _dateFrom != null && x.RegisterDate >= _dateFrom && x.RegisterDate <= _dateTo
            ).OrderByDescending(x=>x.RegisterDate);

            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    

                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberReportsViewModel> data = new List<MemberReportsViewModel>();


            foreach (var item in datas)
            {
                MemberReportsViewModel itemMember = new MemberReportsViewModel();
                {
                    itemMember.Id = item.Id;
                    itemMember.MemberCode = item.MemberCode;
                    itemMember.FirstName = item.FirstName;
                    itemMember.LastName = item.LastName;
                    itemMember.Email = item.Email;
                    itemMember.PhoneNumber = item.PhoneNumber;
                    itemMember.Gender = item.Gender;
                    itemMember.Point = item.MemberWallets.First().Point;
                    itemMember.TierName = item.MemberLoyaltyTiers.OrderByDescending(p => p.ActiveFrom <= DateTime.UtcNow && DateTime.UtcNow <= p.ActiveEnd ? p.ActiveEnd : null).FirstOrDefault().LoyaltyTier.TierName;
                    itemMember.CreateDate = string.Format("{0:dd/MM/yyyy}", item.RegisterDate);
                    itemMember.DistrictName = item.Address.District != null ? item.Address.District.Name : "";
                    itemMember.ProvinceName = item.Address.Province != null ? item.Address.Province.Name : "";
                    itemMember.YOB = item.YOB != 0 ? item.YOB.ToString() : null;
                    itemMember.Active = item.MemberWallets.FirstOrDefault().Active;
                }
                data.Add(itemMember);
            }
            return  data;
        }

        public List<ProductQrReportsViewModel> SearchProductQrReport(string searchText, string[] productId, string? dateFrom, string? dateTo, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Product Qr Report service Search={searchText}, Page={page}");
            DateTime? _dateFrom, _dateTo;
            if (dateFrom != null) { _dateFrom = DateTime.Parse(dateFrom).StartOfMonth(); }
            else { _dateFrom = null; }
            if (dateTo != null) { _dateTo = DateTime.Parse(dateTo).EndOfMonth(); }
            else { _dateTo = null; }
            int stt = 1;
            var QrProduct = UnitOfWork.MemberLoyaltyTransactionRepo.GetAll().Where(q => q.PointTypeId == (int)LOYAlTY_POINT_TYPE.CUSTOM_POINTS);
            var LoyaltyTrans = QrProduct.Select(x => new LoyaltyTransViewModel
            {  Id= x.Id,
               ProductId = int.Parse(x.RefId)
            }).ToList();

            //string value =  productId.JoinToString();
            //string[] vals = value.Split(';');
            int Product = 0;
            if(productId.Length != 0)
            {
                var Products = Array.ConvertAll(productId, s => int.TryParse(s, out Product) ? Product : 0).Where(x => x != 0).ToArray();
                var newLoyaltyTrans = LoyaltyTrans.Where(x => Products.Contains(x.ProductId)).Select(t => t.Id);
                var getall = QrProduct.Where(x => newLoyaltyTrans.Contains(x.Id));

                var query2 = getall.Where(x => (_dateFrom == null && _dateTo == null && x.Id != null)
                  || (_dateFrom != null && _dateTo == null && x.AddedTimestamp >= _dateFrom)
                  || (_dateTo != null && _dateFrom == null && x.AddedTimestamp <= _dateTo)
                  || (_dateTo != null && _dateFrom != null && x.AddedTimestamp >= _dateFrom && x.AddedTimestamp <= _dateTo)
              
                  ).OrderByDescending(x => x.AddedTimestamp);

                total = query2.Count();
        
                var datass = query2.Skip((page - 1) * size).Take(size).ToList();
                List<ProductQrReportsViewModel> productQrReports= new List<ProductQrReportsViewModel>();
                foreach (var item in datass)
                {

                    ProductQrReportsViewModel itemQr = new ProductQrReportsViewModel();
                    {


                        itemQr.STT = productQrReports.Count() + 1;
                        itemQr.CodeScanDate = item.AddedTimestamp.ToString();
                        itemQr.ProductName = UnitOfWork.ProductRepo.GetAll().Where(p => p.Id.ToString() == item.RefId).FirstOrDefault().Name;
                        itemQr.ProductCode = UnitOfWork.ProductRepo.GetAll().Where(p => p.Id.ToString() == item.RefId).FirstOrDefault().Code;
                        itemQr.FullProduct = $"{itemQr.ProductCode} - {itemQr.ProductName}";

                        itemQr.MemberCode = item.MemberWallet.Member.MemberCode;
                        itemQr.LastName = item.MemberWallet.Member.LastName != null ? item.MemberWallet.Member.LastName : "";
                        itemQr.FirstName = item.MemberWallet.Member.FirstName != null ? item.MemberWallet.Member.FirstName : "";
                        itemQr.FullName = itemQr.LastName + " " + itemQr.FirstName;
                        itemQr.BillNo = item.InvoiceNo;
                        itemQr.PointTypeId = item.PointTypeId;


                    }
                    productQrReports.Add(itemQr);
                }

                return productQrReports;
            }
            var query = QrProduct.Where(x => x.Id != null
               && (_dateFrom != null && _dateTo == null && x.AddedTimestamp >= _dateFrom)
               || (_dateTo != null && _dateFrom == null && x.AddedTimestamp <= _dateTo)
               || (_dateTo != null && _dateFrom != null && x.AddedTimestamp >= _dateFrom)
               ).OrderByDescending(x => x.AddedTimestamp);

            total = query.Count();
           
          
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<ProductQrReportsViewModel> data = new List<ProductQrReportsViewModel>();
            foreach (var item in datas)
            {
                ProductQrReportsViewModel itemQr = new ProductQrReportsViewModel();
                {
                    //itemQr.STT = itemQr.MemberCode.Count();
                    itemQr.STT = data.Count() + 1;
                    itemQr.CodeScanDate = item.AddedTimestamp.ToString();
                    itemQr.ProductName = UnitOfWork.ProductRepo.GetAll().Where(p => p.Id.ToString() == item.RefId).FirstOrDefault().Name;
                    itemQr.ProductCode = UnitOfWork.ProductRepo.GetAll().Where(p => p.Id.ToString() == item.RefId).FirstOrDefault().Code;
                    itemQr.FullProduct = $"{itemQr.ProductCode} - {itemQr.ProductName}";

                    itemQr.MemberCode = item.MemberWallet.Member.MemberCode;
                    itemQr.LastName = item.MemberWallet.Member.LastName != null ? item.MemberWallet.Member.LastName : "";
                    itemQr.FirstName= item.MemberWallet.Member.FirstName != null ? item.MemberWallet.Member.FirstName : "";
                    itemQr.FullName = itemQr.LastName + " "+ itemQr.FirstName;
                    itemQr.BillNo = item.InvoiceNo;
                    itemQr.PointTypeId = item.PointTypeId;

                }

                data.Add(itemQr);
            }

            return  data;
        }

        public  List<RedemptionReportsViewModel> SearchRedemptionReport(string searchText, Guid? collectionId, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            var CollectionItem = UnitOfWork.ProductCollectionItemRepo.GetAll().Where(x => x.Id == x.ProductOrders.FirstOrDefault().ProductItemId);
            var query = CollectionItem.Where(x => collectionId !=null  && x.ProductCollectionId == collectionId);

            total = query.Count();          
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<RedemptionReportsViewModel> data = new List<RedemptionReportsViewModel>();
            foreach (var item in datas)
            {
                RedemptionReportsViewModel itemRedem = new RedemptionReportsViewModel();
                {
                    itemRedem.ProductCode = item.Product.Code;
                    itemRedem.ProductName = item.Product.Name;
                    itemRedem.TotalNewOrder = item.ProductOrders != null ? item.ProductOrders.Where(x => x.OrderStatus == PRODUCT_ORDER_STATUS.NEW).Count(): 0;
                    itemRedem.TotalInTransit = item.ProductOrders != null ? item.ProductOrders.Where(x => x.OrderStatus == PRODUCT_ORDER_STATUS.ON_DELIVERY).Count() : 0;
                    itemRedem.TotalCompleted = item.ProductOrders != null ? item.ProductOrders.Where(x => x.OrderStatus == PRODUCT_ORDER_STATUS.COMPLETE).Count() : 0;
                    itemRedem.TotalCanceled = item.ProductOrders != null ? item.ProductOrders.Where(x => x.OrderStatus == PRODUCT_ORDER_STATUS.CANCEL).Count() : 0;


                    //itemRedem.Status= item.pr
                }

                data.Add(itemRedem);
            }
            return data;
        }
    }
}
