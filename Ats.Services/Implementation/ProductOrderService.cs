using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using System.Security.Claims;
using Ats.Models.MemberWallets;
using Ats.Domain.Member.Models;
using Ats.Commons.Utilities;

namespace Ats.Services.Implementation
{
    public class ProductOrderService : BaseService, IProductOrderService
    {
        private IConfiguration _config;
        private int pageSize;

        public ProductOrderService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<ProductOrderViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int status, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.ProductOrderRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.ProductItem.Product.Name != null && x.ProductItem.Product.Name.ToLower().Contains(searchText.ToLower()) ||
                                x.Member.LastName != null && x.Member.LastName.ToLower().Contains(searchText.ToLower()) ||
                                x.Member.FirstName != null && x.Member.FirstName.ToLower().Contains(searchText.ToLower()) ||
                                x.Member.MemberCode != null && x.Member.MemberCode.ToLower().Contains(searchText.ToLower()) ||
                                x.PhoneNo != null && x.PhoneNo.ToLower().Contains(searchText.ToLower()) ||
                                x.Receiver != null && x.Receiver.ToLower().Contains(searchText.ToLower()) ||
                                x.ProductItem.ProductCollection.Name != null && x.ProductItem.ProductCollection.Name.ToLower().Contains(searchText.ToLower())).OrderByDescending(m=> m.AddedTimestamp);
            total = query.Count();
            if (status == 0) { total = query.Count(); }
            if (status == 1) { total = query.Where(m => m.OrderStatus == PRODUCT_ORDER_STATUS.NEW).Count(); }
            if (status == 2) { total = query.Where(m => m.OrderStatus == PRODUCT_ORDER_STATUS.ON_DELIVERY).Count(); }
            if (status == 3) { total = query.Where(m => m.OrderStatus == PRODUCT_ORDER_STATUS.COMPLETE).Count(); }
            if (status == 4) { total = query.Where(m => m.OrderStatus == PRODUCT_ORDER_STATUS.CANCEL).Count(); }
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "membercode":
                        query = IsAscOrder ? query.OrderBy(x => x.Member) : query.OrderByDescending(x => x.Member);
                        break;
                    case "membername":
                        query = IsAscOrder ? query.OrderBy(x => x.Member) : query.OrderByDescending(x => x.Member);
                        break;
                    case "productname":
                        query = IsAscOrder ? query.OrderBy(x => x.ProductItem) : query.OrderByDescending(x => x.ProductItem);
                        break;
                    case "price":
                        query = IsAscOrder ? query.OrderBy(x => x.Price) : query.OrderByDescending(x => x.Price);
                        break;
                    case "createdate":
                        query = IsAscOrder ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                        break;
                    case "receiver":
                        query = IsAscOrder ? query.OrderBy(x => x.Receiver) : query.OrderByDescending(x => x.Receiver);
                        break;
                    case "phoneno":
                        query = IsAscOrder ? query.OrderBy(x => x.PhoneNo) : query.OrderByDescending(x => x.PhoneNo);
                        break;
                    case "shippingaddress":
                        query = IsAscOrder ? query.OrderBy(x => x.ShippingAddress) : query.OrderByDescending(x => x.ShippingAddress);
                        break;
                    case "orderstatus":
                        query = IsAscOrder ? query.OrderBy(x => x.OrderStatus) : query.OrderByDescending(x => x.OrderStatus);
                        break;

                    case "quantity":
                        query = IsAscOrder ? query.OrderBy(x => x.Quantity) : query.OrderByDescending(x => x.Quantity);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            if (status == 1) { datas = query.Where(m => m.OrderStatus == PRODUCT_ORDER_STATUS.NEW).Skip((page - 1) * size).Take(size).ToList(); }
            if (status == 2) { datas = query.Where(m => m.OrderStatus == PRODUCT_ORDER_STATUS.ON_DELIVERY).Skip((page - 1) * size).Take(size).ToList(); ; }
            if (status == 3) { datas = query.Where(m => m.OrderStatus == PRODUCT_ORDER_STATUS.COMPLETE).Skip((page - 1) * size).Take(size).ToList(); }
            if (status == 4) { datas = query.Where(m => m.OrderStatus == PRODUCT_ORDER_STATUS.CANCEL).Skip((page - 1) * size).Take(size).ToList(); }
            List<ProductOrderViewModel> data = new List<ProductOrderViewModel>();
            foreach (var item in datas)
            {
                var productcollectionitem = UnitOfWork.ProductCollectionItemRepo.GetById(item.ProductItemId);
                var collection = UnitOfWork.ProductCollectionRepo.GetById(productcollectionitem.ProductCollection.Id);
                var product = UnitOfWork.ProductRepo.GetById(productcollectionitem.Product.Id);
                var member = UnitOfWork.MemberRepo.GetById(item.MemberId);


                ProductOrderViewModel productorder = new ProductOrderViewModel();
                productorder.Id = item.Id;
                productorder.ProductItemId = item.ProductItemId;
                productorder.MemberId = item.MemberId;
                productorder.Date = item.Date;
                productorder.Quantity = item.Quantity;
                productorder.Price = item.Price;
                productorder.Remark = item.Remark;
                productorder.ShippingAddress = item.ShippingAddress;
                productorder.OrderStatus = item.OrderStatus;
                productorder.Receiver = item.Receiver;
                productorder.PhoneNo = item.PhoneNo;


                productorder.NameProduct = product.Name;
                productorder.NameMember = member.LastName + " " + member.FirstName;
                productorder.FirstName = member.FirstName;
                productorder.LastName = member.LastName;
                productorder.MemberCode = member.MemberCode;
                productorder.NameCollection = collection.Name;
                productorder.CreateOrder = item.AddedTimestamp;
                productorder.DateCreate = string.Format("{0:dd/MM/yyyy}", item.Date);

                data.Add(productorder);
            }
            return data;
        }


        public List<ProductOrderViewModel> GetProductOrders()
        {
            _logger.LogDebug($"GetAlls");
            var products = UnitOfWork.ProductOrderRepo.GetAll().Select(x => new ProductOrderViewModel()
            {
                Id = x.Id,
                ProductItemId = x.ProductItemId,
                MemberId = x.MemberId,
                Date = x.Date,
                Quantity = x.Quantity,
                Price = x.Price,
                Remark = x.Remark,
                ShippingAddress = x.ShippingAddress,
                OrderStatus = x.OrderStatus,
                PhoneNo = x.PhoneNo,
                Receiver = x.Receiver,

            }).OrderBy(x => x.Price).ToList();

            return products;
        }

        public ProductOrderViewModel GetProductOrder(Guid id)
        {
            _logger.LogDebug($"Detail (Id: {id})");
            var entity = UnitOfWork.ProductOrderRepo.GetById(id);

            var productcollectionitem = UnitOfWork.ProductCollectionItemRepo.GetById(entity.ProductItemId);
            var collection = UnitOfWork.ProductCollectionRepo.GetById(productcollectionitem.ProductCollection.Id);
            var collectionitem = UnitOfWork.ProductCollectionItemRepo.GetById(entity.ProductItemId);
            var product = UnitOfWork.ProductRepo.GetById(productcollectionitem.Product.Id);
            var member = UnitOfWork.MemberRepo.GetById(entity.MemberId);
          
            if (entity != null)
            {
                var model = new ProductOrderViewModel()
                {  
                    Id = entity.Id,
                    ProductItemId = entity.ProductItemId,
                    MemberId = entity.MemberId,
                    Date = entity.Date,
                    Quantity = entity.Quantity,
                    Price = productcollectionitem.Price,
                    Remark = entity.Remark,
                    ShippingAddress = entity.ShippingAddress,
                    OrderStatus = entity.OrderStatus,
                    zaloId = member.ZaloId,
                    ChangedUserId = entity.ChangedUserId,
                    NameCollection = collection.Name,
                    NameProduct = product.Name,
                    NameMember = member.FirstName + " " + member.LastName,
                    CheckMemberId = entity.MemberId,
                    CheckProductItemId = entity.ProductItemId,
                    CheckPrice = entity.Price,
                    TotalPoint = (entity.Price * entity.Quantity).ToString(),
                    PhoneNo = entity.PhoneNo,
                    Receiver = entity.Receiver,
                    DateCreate = string.Format("{0:dd/MM/yyyy}", entity.Date),
                };
              

                return model;
            }
            return null;
        }
        public void UpdateStatus(ProductOrderSearchViewModel model)
        {
            //_logger.LogDebug($"Detail (Id: {model.ProductOrder.Id})");
            foreach (DisplayItem item in model.CheckBoxChangeStatus)
            {
                if (item.Selected)
                {
                    var order = UnitOfWork.ProductOrderRepo.GetById(Guid.Parse(item.Value));
                    if (model.Status == 2) { order.OrderStatus = PRODUCT_ORDER_STATUS.ON_DELIVERY; }
                    if (model.Status == 3) { order.OrderStatus = PRODUCT_ORDER_STATUS.COMPLETE; }
                    if (model.Status == 4)
                    {
                        order.OrderStatus = PRODUCT_ORDER_STATUS.CANCEL; // change status

                        var member = UnitOfWork.MemberRepo.GetById(order.MemberId); // trả điểm member
                        var wallet = UnitOfWork.MemberWalletRepo.GetAll().Where(m => m.MemberId == order.MemberId).FirstOrDefault();
                        var productitem = UnitOfWork.ProductCollectionItemRepo.GetById(order.ProductItemId);
                        double Total = (item.Price * order.Quantity);
                        wallet.Point += Total;
                        UnitOfWork.MemberWalletRepo.Update(wallet);
                        UnitOfWork.SaveChanges();

                        int pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_ROLLBACK_PRODUCT;
                        AddMemberLoyaltyTransaction(wallet.Id, member, order.Id, Total, pointtypeid);

                        productitem.Stock += (int)order.Quantity;
                        UnitOfWork.ProductCollectionItemRepo.Update(productitem);
                        UnitOfWork.SaveChanges();

                    }
                    UnitOfWork.ProductOrderRepo.Update(order);
                    UnitOfWork.SaveChanges();
                }
            }
        }
        public void AddMemberLoyaltyTransaction(Guid walletid, Member member, Guid idorder, double point, int pointtypeid)
        {
            var order = UnitOfWork.ProductOrderRepo.GetAll().Where(m => m.Id == idorder).FirstOrDefault();
            

            MemberLoyaltyTransaction mlt = new MemberLoyaltyTransaction();
            mlt.Id = Guid.NewGuid();
            mlt.WalletId = walletid;
            mlt.PointTypeId = pointtypeid; // trả điểm redemtion
            mlt.ChannelId = member.ChannelId;
            mlt.ChannelId = member.StoreId;
            mlt.Amount = 0; mlt.Point = point; mlt.Rate = 0;
            mlt.RefId = order.ProductItem.Product.Id.ToString();
            mlt.TransactionDate = DateTime.Now;
            UnitOfWork.MemberLoyaltyTransactionRepo.Insert(mlt);
            UnitOfWork.SaveChanges();
        }

        public ProductOrderViewModel GetPrice(Guid id, Guid item, Guid member)
        {
            _logger.LogDebug($"Price (Id: {id})");
            var ItemCollect = UnitOfWork.ProductCollectionItemRepo.GetById(id);
            var product = UnitOfWork.ProductRepo.GetById(ItemCollect.Product.Id);


            if (product != null)
            {
                var model = new ProductOrderViewModel()
                {
                    MemberId = member,
                    ProductItemId = id,
                    NameProduct = product.Name,
                    Price = ItemCollect.Price,

                };
                if (member != Guid.Empty)
                {
                    var profile = UnitOfWork.MemberRepo.GetById(member);
                    var wallet = UnitOfWork.MemberWalletRepo.GetAll().Where(m => m.MemberId == profile.Id).FirstOrDefault();
                    model.NameMember = profile.FirstName + profile.LastName;
                    model.Point = wallet.Point;

                }
                return model;
            }
            return null;
        }

        //public bool CreateProductOrder(ProductOrderViewModel model)
        //{
        //    if (model.MemberId == Guid.Empty)
        //    {
        //        return false;
        //    }
        //    var point = UnitOfWork.MemberWalletRepo.GetAll().Where(m => m.MemberId == model.MemberId).FirstOrDefault();
        //    var member = UnitOfWork.MemberRepo.GetAll().Where(m => m.Id == model.MemberId).FirstOrDefault();
        //    double diem = point.Point;
        //    double giatien = model.Price;
        //    if (diem >= giatien)
        //    {

        //        var entity = new ProductOrder
        //        {
        //            Id = new Guid(),
        //            ProductItemId = model.ProductItemId,
        //            MemberId = model.MemberId,
        //            Date = model.Date,
        //            Quality = model.Quality,
        //            Price = model.Price,
        //            Remark = model.Remark,
        //            ShippingAddress = model.ShippingAddress,
        //            OrderStatus = model.OrderStatus,
        //            PhoneNo = model.PhoneNo,
        //            Receiver = model.Receiver,
        //        };

        //        UnitOfWork.ProductOrderRepo.Insert(entity);
        //        UnitOfWork.SaveChanges();

        //        point.Point = diem - giatien;
        //        UnitOfWork.MemberWalletRepo.Update(point);
        //        UnitOfWork.SaveChanges();


        //        int pointtypeid = 14;
        //        AddMemberLoyaltyTransaction(point.Id, member, entity.Id, model.Price, pointtypeid);

        //        return true;
        //    }
        //    return false;
        //}




        public bool Create(ProductOrderViewModel model)
        {
            DateTime date = model.Date ?? DateTime.Now;
            if (model.MemberId == Guid.Empty && model.ProductItemId == Guid.Empty)
            {
                return false;
            }
            //if (model.DateOrder == null)
            //{
            //    model.DateOrder = DateTime.Today.ToString();
            //}
            //var _dateorder = DateTime.Parse(model.DateOrder);
            var point = UnitOfWork.MemberWalletRepo.GetAll().Where(m => m.MemberId == model.MemberId).FirstOrDefault();
            var member = UnitOfWork.MemberRepo.GetAll().Where(m => m.Id == model.MemberId).FirstOrDefault();
            var item = UnitOfWork.ProductCollectionItemRepo.GetById(model.ProductItemId);
        
            double diem = point.Point;
            double giatien = (item.Price * model.Quantity); 
            if (diem >= giatien)
            {
                   
                var entity = new ProductOrder
                {
                    Id = new Guid(),
                    ProductItemId = model.ProductItemId,
                    MemberId = model.MemberId,
                    Date = date,
               
                    Quantity = model.Quantity,
                    Price = item.Price,
                    Remark = model.Remark,
                    OrderStatus = model.OrderStatus,
                 
                    
                    
                };
                if(model.Quantity == 0) { entity.Quantity = 1; }
                if (model.Receiver == null) { entity.Receiver = member.FirstName + " " + member.LastName; }
                else { entity.Receiver = model.Receiver; }
                if (model.PhoneNo == null) { entity.PhoneNo = member.PhoneNumber; }
                else { entity.PhoneNo = model.PhoneNo; }
                if (model.ShippingAddress == null) { entity.ShippingAddress = member.Address.Address1 + " " + member.Address.District.Name + " " + member.Address.Province.Name; }
                else { entity.ShippingAddress = model.ShippingAddress; }

                UnitOfWork.ProductOrderRepo.Insert(entity);
                UnitOfWork.SaveChanges();

                point.Point = diem - giatien;
                UnitOfWork.MemberWalletRepo.Update(point);
                UnitOfWork.SaveChanges();

                int pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_POINTS_PRODUCT;
                AddMemberLoyaltyTransaction(point.Id, member, entity.Id, giatien, pointtypeid);

                item.Stock -= (int)model.Quantity;
                UnitOfWork.ProductCollectionItemRepo.Update(item);
                UnitOfWork.SaveChanges();

                return true;
            }
            return false;
        }


        public ProductOrderViewModel CheckMemberItem(ProductOrderViewModel model)
        {


            var entity = new ProductOrderViewModel
            {
                Id = model.Id,
                ProductItemId = model.ProductItemId,
                MemberId = model.MemberId,
                Price = model.Price,
            };

            return entity;

        }


        public bool UpdateProductOrder(ProductOrderViewModel model, Claim userid)
        {
            // Nếu change memebr hoặc item, thì phải giữ lại member cũ hoặc item cũ để trã điểm member.
            // Neu change memebr thi check: membermoi du diem hay kh? neu du thi cho phep edit va tra diem lai cho member cu.
            // Neu change item thi kiểm tra point cua member co du doi price cuar item kh? trừ tiếp hoặc trả lại sô dư cho member.
            // chi được change 1 trong hai ( PRICE hoặc ITEM)
            _logger.LogDebug($"Edit (Id: {model.Id})");
            int pointtypeid;
            var entity = UnitOfWork.ProductOrderRepo.GetById(model.Id);
            var collectionitem = UnitOfWork.ProductCollectionItemRepo.GetById(model.ProductItemId);
            var memberwallet = UnitOfWork.MemberWalletRepo.GetAll().Where(m => m.MemberId == model.MemberId).FirstOrDefault();
            var olditem = UnitOfWork.ProductCollectionItemRepo.GetById(model.CheckProductItemId);
            var user = UnitOfWork.UserRepo.GetAll().Where(m => m.UserName == userid.Subject.Name).FirstOrDefault();
            var member = UnitOfWork.MemberRepo.GetAll().Where(m => m.Id == model.MemberId).FirstOrDefault();
            var formermember = UnitOfWork.MemberWalletRepo.GetAll().Where(m => m.MemberId == model.CheckMemberId).FirstOrDefault();
            var memberold = UnitOfWork.MemberRepo.GetById(model.CheckMemberId);
            DateTime date = model.Date ?? DateTime.Now;
            if (entity != null)
            {
               
                    entity.Id = model.Id;
                    entity.ProductItemId = model.ProductItemId;
                    entity.MemberId = model.MemberId;
                    entity.Date = date;
                if (model.OrderStatus != PRODUCT_ORDER_STATUS.NEW)
                    {
                        entity.OrderStatus = model.OrderStatus;

                    }
                    entity.Price = collectionitem.Price;
                    entity.Remark = model.Remark;
                    //Edit Product Order
                    UnitOfWork.ProductOrderRepo.Update(entity);
                    UnitOfWork.SaveChanges();
            

                if (memberwallet.Point >= (collectionitem.Price * model.Quantity))
                {


                    if (model.Receiver == null) { entity.Receiver = member.FirstName + " " + member.LastName; }
                    else { entity.Receiver = model.Receiver; }
                    if (model.PhoneNo == null) { entity.PhoneNo = member.PhoneNumber; }
                    else { entity.PhoneNo = model.PhoneNo; }
                    if (model.ShippingAddress == null) { entity.ShippingAddress = member.Address.Address1; }
                    else { entity.ShippingAddress = model.ShippingAddress; }

                    if (entity.OrderStatus != model.OrderStatus && model.CheckMemberId == model.MemberId) // kiểm tra có thay đổi status hay không và member/product kh thay đổi
                    {

                        if (model.OrderStatus == PRODUCT_ORDER_STATUS.CANCEL) // kiểm tra nếu thay đổi status thành cancel thì trả điểm member
                        {
                            //không thay đổi member và product, chỉ thay đổi status (trã điểm cho member)
                            if (model.CheckMemberId == model.MemberId && model.ProductItemId == model.CheckProductItemId)
                            {
                                double Total = (entity.Price * entity.Quantity);
                                memberwallet.Point += Total; // khong change product và item nên vẫn lấy model.price trả lại điểm cho member.
                                UnitOfWork.MemberWalletRepo.Update(memberwallet);
                                UnitOfWork.SaveChanges();

                                pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_ROLLBACK_PRODUCT;
                                AddMemberLoyaltyTransaction(memberwallet.Id, member, entity.Id, Total, pointtypeid);

                                collectionitem.Stock += (int)model.Quantity;
                                UnitOfWork.ProductCollectionItemRepo.Update(collectionitem);
                                UnitOfWork.SaveChanges();
                            }
                        }

                    }

                    if (model.CheckMemberId != model.MemberId && entity.OrderStatus == model.OrderStatus) // change member và không thay đổi status
                    {

                        if (model.ProductItemId != model.CheckProductItemId) // change product item
                        {
                            // trả điểm lại cho member cũ
                            double TotalPoint = (entity.Price * entity.Quantity);
                            formermember.Point += TotalPoint;
                            UnitOfWork.MemberWalletRepo.Update(formermember);
                            UnitOfWork.SaveChanges();
                            pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_ROLLBACK_PRODUCT; // minus point
                            AddMemberLoyaltyTransaction(formermember.Id, memberold, entity.Id, TotalPoint, pointtypeid);

                            olditem.Stock += (int)entity.Quantity;
                            collectionitem.Stock -= (int)model.Quantity; // trả số lượng cho sản phẩm củ và trừ số lượng của sản phẩm mới.
                            UnitOfWork.ProductCollectionItemRepo.Update(olditem);
                            UnitOfWork.ProductCollectionItemRepo.Update(collectionitem);
                            UnitOfWork.SaveChanges();
                        }
                        else // không change product item
                        {
                            // trả điểm lại cho member cũ 
                            double TotalPointChanePr = (entity.Price * entity.Quantity);
                            formermember.Point += TotalPointChanePr;
                            UnitOfWork.MemberWalletRepo.Update(formermember);
                            UnitOfWork.SaveChanges();
                            pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_ROLLBACK_PRODUCT; // minus point
                            AddMemberLoyaltyTransaction(formermember.Id, memberold, entity.Id, TotalPointChanePr, pointtypeid);
                        }

                        double TotalPointNew = model.Price * model.Quantity;
                        // cập nhật điểm cho member mới
                        memberwallet.Point -= TotalPointNew;
                        UnitOfWork.MemberWalletRepo.Update(memberwallet);
                        UnitOfWork.SaveChanges();

                        pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_POINTS_PRODUCT; // minus point
                        AddMemberLoyaltyTransaction(memberwallet.Id, member, entity.Id, TotalPointNew, pointtypeid);

                    }
                    if (model.ProductItemId != model.CheckProductItemId && model.CheckMemberId == model.MemberId && entity.OrderStatus == model.OrderStatus) // change item
                    {
                        double TotalOld = olditem.Price * entity.Quantity;
                        double TotalNew = collectionitem.Price * model.Quantity;
                        double Total = TotalOld - TotalNew;
                        memberwallet.Point += Total;
                        UnitOfWork.MemberWalletRepo.Update(memberwallet);
                        UnitOfWork.SaveChanges();
                        if (Total > 0)
                        {
                            pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_ROLLBACK_PRODUCT; // minus point
                        }
                        else
                        {
                            pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_POINTS_PRODUCT; // minus point
                        }
                        var duong = System.Math.Abs(Total);

                        AddMemberLoyaltyTransaction(memberwallet.Id, member, entity.Id, duong, pointtypeid); // ghi lịch sử giao dịch điểm
                        collectionitem.Stock -= (int)model.Quantity; // trả số lượng cho sản phẩm củ và trừ số lượng của sản phẩm mới.
                        olditem.Stock += (int)entity.Quantity;

                        UnitOfWork.ProductCollectionItemRepo.Update(olditem);
                        UnitOfWork.ProductCollectionItemRepo.Update(collectionitem);
                        UnitOfWork.SaveChanges();
                    }
                   

                    return true;

                }

            }
            return false;
        }
        public void DeleteProductOrder(Guid id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.ProductOrderRepo.GetById(id);
            var productitem = UnitOfWork.ProductCollectionItemRepo.GetById(entity.ProductItemId);
            if (entity != null)
            {
                if(entity.OrderStatus != PRODUCT_ORDER_STATUS.CANCEL && entity.OrderStatus != PRODUCT_ORDER_STATUS.COMPLETE) // xóa nếu kh phải cancel và complete thì trả điểm member + lưu lịch sử điểm
                {
                    var member = UnitOfWork.MemberRepo.GetById(entity.MemberId);
                    var wallet = UnitOfWork.MemberWalletRepo.GetAll().Where(m => m.MemberId == entity.MemberId).FirstOrDefault();
                    var total = (entity.Price * entity.Quantity);
                    wallet.Point += total ;
                    UnitOfWork.MemberWalletRepo.Update(wallet);
                    UnitOfWork.SaveChanges();

                    int pointtypeid = (int)LOYAlTY_POINT_TYPE.REDEMPTION_ROLLBACK_PRODUCT; // minus point
                    AddMemberLoyaltyTransaction(wallet.Id, member, entity.Id, total, pointtypeid);

                }

                UnitOfWork.ProductOrderRepo.Delete(entity);
                productitem.Stock += (int)entity.Quantity;
                UnitOfWork.ProductCollectionItemRepo.Update(productitem);
                UnitOfWork.SaveChanges();


            }
        }


        public int GetStatisticsProductOrders(DateTime dayb, DateTime daye)
        {
            //DateTime dateB = DateTime.Parse(dayb);
            //DateTime dateE = DateTime.Parse(daye);
            _logger.LogDebug($"GetAlls");
            var order = UnitOfWork.ProductOrderRepo.GetAll().Select(x => new ProductOrderViewModel()
            {
                Id = x.Id,
                ProductItemId = x.ProductItemId,
                MemberId = x.MemberId,
                Date = x.Date,
                OrderStatus = x.OrderStatus,
            }).Where(m => m.Date >= dayb && m.Date <= daye).Count();
            return order;
        }
        public int GetStatisticsProductOrders()
        {
            var order = UnitOfWork.ProductOrderRepo.GetAll().Select(x => new ProductOrderViewModel()
            {
                Id = x.Id
            }).Count();
            return order;
        }
        public int GetStatisticsProductOrdersThisMonth(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        { 
            var order = UnitOfWork.ProductOrderRepo.GetAll().Select(x => new ProductOrderViewModel()
            {
                Id = x.Id,
                Date = x.Date
            }).Where(m => m.Date > firstDayOfMonth || m.Date == firstDayOfMonth && m.Date < lastDayOfMonth || m.Date == lastDayOfMonth).Count();
            return order;
        }
    }
}
