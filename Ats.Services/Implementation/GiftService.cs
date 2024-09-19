using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Gift.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Ats.Commons;
using Ats.Domain.Channel.Models;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Ats.Domain.Member.Models;
using Ats.Domain.Voucher.Models;

namespace Ats.Services.Implementation
{
    public class GiftService : BaseService, IGiftService
    {
        private IConfiguration _config;
        private IHostingEnvironment _hostingEnvironment;
        private int pageSize;

        public GiftService(IUnitOfWork unitOfWork, IConfiguration config, IHostingEnvironment hostingEnvironment, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            //pageSize = _config.GetValue<int>("PageSize");
        }

        public List<GiftViewModel> Search(string searchText, int? giftcategoryId, bool? giftType, int? channelId, int? stockFrom, int? stockTo,
           string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");         
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();

            var query = UnitOfWork.GiftRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                                        && ((!String.IsNullOrEmpty(x.GiftCode) && x.GiftCode.ToLower().Contains(searchText.ToLower()))                                       
                                        || (!String.IsNullOrEmpty(x.GiftName) && x.GiftName.ToLower().Contains(searchText.ToLower())))))                                     
                                        && (giftcategoryId == 0 || giftcategoryId == null ? true : x.GiftCategoryId == giftcategoryId)
                                        && (giftType == null ? true : x.GiftType == giftType)
                                        && (channelId == 0 || channelId == null ? true : x.ChannelId == channelId)                              
                                        && ((stockFrom == null && stockTo == null) ||
                                        (stockFrom == null && stockTo != null && x.AvailableStock <= stockTo) ||
                                        (stockTo == null && stockFrom != null && x.AvailableStock >= stockFrom) ||
                                        (stockFrom != null && stockTo != null && x.AvailableStock >= stockFrom && x.AvailableStock <= stockTo)));

            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.GiftCode) : query.OrderByDescending(x => x.GiftCode);
                        break;
                    case "type":
                        query = IsAscOrder ? query.OrderBy(x => x.GiftType) : query.OrderByDescending(x => x.GiftType);
                        break;
                    case "category":
                        query = IsAscOrder ? query.OrderBy(x => x.GiftCategory.GiftName) : query.OrderByDescending(x => x.GiftCategory.GiftName);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.GiftName) : query.OrderByDescending(x => x.GiftName);
                        break;
                    case "channel":
                        query = IsAscOrder ? query.OrderBy(x => x.Channel.ChannelName) : query.OrderByDescending(x => x.Channel.ChannelName);
                        break;
                    case "effectivefrom":
                        query = IsAscOrder ? query.OrderBy(x => x.EffectiveDateBegin) : query.OrderByDescending(x => x.EffectiveDateBegin);
                        break;
                    case "effectiveto":
                        query = IsAscOrder ? query.OrderBy(x => x.EffectiveDateEnd) : query.OrderByDescending(x => x.EffectiveDateEnd);
                        break;
                    case "stock":
                        query = IsAscOrder ? query.OrderBy(x => x.AvailableStock) : query.OrderByDescending(x => x.AvailableStock);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<GiftViewModel> data = new List<GiftViewModel>();
            foreach (var item in datas)
            {
                GiftViewModel gift = new GiftViewModel();
                gift.Id = item.Id;
                gift.GiftCode = item.GiftCode;
                gift.GiftName = item.GiftName;
                gift.GiftCategoryId = item.GiftCategoryId;
                gift.GiftCategoryName = item.GiftCategory != null ? item.GiftCategory.GiftName : "";
                gift.GiftType = item.GiftType;               
                gift.Amount = item.Amount;
                gift.Discount = item.Discount;
                gift.ChannelId = item.ChannelId;
                gift.ChannelName = item.Channel != null ? item.Channel.ChannelName : "";
               
                gift.ValidityPeriodType = item.ValidityPeriodType;
                gift.AvailableStock = item.AvailableStock;
                gift.NumberRedeemed = item.NumberRedeemed;
                gift.StockAdjustment = item.StockAdjustment;
                gift.RetailMarketPrice = item.RetailMarketPrice;
                gift.PurchasePrice = item.PurchasePrice;
        
                gift.uniqueFileName = item.Image;
                gift.EffectiveDateBegins = string.Format("{0:dd/MM/yyyy}", item.EffectiveDateBegin);//
                gift.EffectiveDateEnds = string.Format("{0:dd/MM/yyyy}", item.EffectiveDateEnd);
                gift.AfterEffectiveDay = item.AfterEffectiveDay;
                gift.AfterReceptionDay = item.AfterReceptionDay;

                gift.Active = item.Active;
                gift.Specification = item.Specification;
                gift.TermAndConditions = item.TermAndConditions;
                gift.Desc = item.Desc;             
                data.Add(gift);
            }

            return data;
        }

        public List<GiftViewModel> Gets()
        {
            _logger.LogDebug($"Get all Gift");
            var gift = UnitOfWork.GiftRepo.GetAll().Where(x => x.Active).Select(x => new GiftViewModel()
            {
                Id = x.Id,
                GiftCode = x.GiftCode,
                GiftName = x.GiftName,
                ChannelId = x.ChannelId,
                GiftCategoryId = x.GiftCategoryId,
                AvailableStock = x.AvailableStock,
                uniqueFileName = x.Image,
                Desc = x.Desc,
                Active = x.Active,
            }).OrderBy(x => x.GiftName).ToList();

            return gift;
        }

        public GiftViewModel Get(Guid id)
        {
            _logger.LogDebug($"Detail Gift (Id: {id})");
            var entity = UnitOfWork.GiftRepo.GetById(id);
            if (entity != null)
            {
                var model = new GiftViewModel()
                {
                    Id = entity.Id,
                    GiftCode = entity.GiftCode,
                    GiftName = entity.GiftName,
                    GiftCategoryId = entity.GiftCategoryId,
                    GiftType = entity.GiftType,
                    ChannelId = entity.ChannelId,                          
                    AvailableStock = entity.AvailableStock,
                    NumberRedeemed = entity.NumberRedeemed,
                    StockAdjustment = entity.StockAdjustment,
                    RetailMarketPrice = entity.RetailMarketPrice,
                    ValidityPeriodType = entity.ValidityPeriodType,
                    PurchasePrice = entity.PurchasePrice,
                    Active = entity.Active,
                    Specification = entity.Specification,
                    TermAndConditions = entity.TermAndConditions,            
                    Desc = entity.Desc,
                    uniqueFileName = entity.Image

                };

                if (entity.GiftCategory != null)
                {
                    model.GiftCategoryName = entity.GiftCategory.GiftName;
                }
                else
                {
                    model.GiftCategoryName = " ";
                }
                if (entity.Channel != null)
                {
                    model.ChannelName = entity.Channel.ChannelName;
                }
                else
                {
                    model.ChannelName = " ";
                }
                if (entity.GiftType == true)
                {
                    model.Amount = entity.Amount;
                    model.Discount = 0;
                }
                else
                {
                    model.Discount = entity.Discount;
                    model.Amount = 0;
                }
                if (entity.ValidityPeriodType == true)
                {
                    model.AfterEffectiveDay = entity.AfterEffectiveDay;
                    model.AfterReceptionDay = entity.AfterReceptionDay;
                    model.EffectiveDateBegin = null;
                    model.EffectiveDateEnd = null;
                }
                else
                {
                    model.EffectiveDateBegin = entity.EffectiveDateBegin;
                    model.EffectiveDateEnd = entity.EffectiveDateEnd;
                    model.AfterEffectiveDay = 0;
                    model.AfterReceptionDay = 0;
                }
                return model;
            }
            return null;
        }
        public void Create(GiftViewModel model)
        {
            string uniqueFileName = UploadedFile(model);
            _logger.LogDebug($"Create Gift (Name: {model.GiftName})");

            Gift entity = new Gift();
            entity.Id = Guid.NewGuid();
            entity.GiftCode = model.GiftCode;
            entity.GiftName = model.GiftName;
            entity.GiftCategoryId = model.GiftCategoryId; 
            entity.ChannelId = model.ChannelId;
            ////GiftType
            entity.GiftType = model.GiftType;
            if (entity.GiftType== true)
            { entity.Amount = model.Amount; }
            else { entity.Discount = model.Discount; }
            ////ValidityPeriodType
            entity.ValidityPeriodType = model.ValidityPeriodType;
            if (entity.ValidityPeriodType == true)
            {
                entity.AfterEffectiveDay = model.AfterEffectiveDay;
                entity.AfterReceptionDay = model.AfterReceptionDay;
            }
            else
            {
                entity.EffectiveDateBegin = model.EffectiveDateBegin;
                entity.EffectiveDateEnd = model.EffectiveDateEnd;
            }

            entity.Image = uniqueFileName;
            entity.Specification = model.Specification;
            entity.TermAndConditions = model.TermAndConditions;
            entity.Desc = model.Desc;
            
            entity.AvailableStock = model.AvailableStock;
            entity.NumberRedeemed = model.NumberRedeemed;
            entity.StockAdjustment = model.StockAdjustment;
            entity.RetailMarketPrice = model.RetailMarketPrice;
            entity.PurchasePrice = model.PurchasePrice;

            if (model.GiftCodeAutomatically)
            {
                entity.GiftCode = this.GenerateGiftCode();
                _logger.LogDebug($"Creating Gift (CodeAuto: {model.GiftCode})");
            }
            entity.Active = true;

            UnitOfWork.GiftRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void Update(GiftViewModel model)
        {
            _logger.LogDebug($"Edit (Id: {model.Id})");
            var entity = UnitOfWork.GiftRepo.GetById(model.Id);
            if (entity != null)
            {
                model.uniqueFileName = UploadedFile(model);
                entity.GiftCode = model.GiftCode;
                entity.GiftName = model.GiftName;
                entity.GiftCategoryId = model.GiftCategoryId;
                entity.ChannelId = model.ChannelId;
           
                entity.AvailableStock = model.AvailableStock;
                entity.NumberRedeemed = model.NumberRedeemed;
                entity.StockAdjustment = model.StockAdjustment;

                entity.RetailMarketPrice = model.RetailMarketPrice;
                entity.PurchasePrice = model.PurchasePrice;
            
                entity.Specification = model.Specification;
                entity.TermAndConditions = model.TermAndConditions;
                entity.Desc = model.Desc;             
                entity.Image = model.uniqueFileName;
                entity.Active = true;

                ////VoucherType
                entity.GiftType = model.GiftType;
                if (entity.GiftType == true)
                {
                    entity.Amount = model.Amount;
                    entity.Discount = 0;
                }
                else
                {
                    entity.Discount = model.Discount;
                    entity.Amount = 0;
                }
                ////ValidityPeriodType
                entity.ValidityPeriodType = model.ValidityPeriodType;
                if (entity.ValidityPeriodType == true)
                {
                    entity.AfterEffectiveDay = model.AfterEffectiveDay;
                    entity.AfterReceptionDay = model.AfterReceptionDay;
                    entity.EffectiveDateBegin = null;
                    entity.EffectiveDateEnd = null;
                }
                else
                {
                    entity.EffectiveDateBegin = model.EffectiveDateBegin;
                    entity.EffectiveDateEnd = model.EffectiveDateEnd;
                    entity.AfterEffectiveDay = 0;
                    entity.AfterReceptionDay = 0;
                }

                UnitOfWork.GiftRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void Delete(Guid id)
        {
            _logger.LogDebug($"Delete (Id: {id})");
            var entity = UnitOfWork.GiftRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.GiftRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<GiftCategory> GetGiftCategories()
        {
            _logger.LogDebug($"Get Gift Categories Enter.");
            var giftCategories = UnitOfWork.GiftCategoryRepo.GetAll().Where(x => x.Active).Select(x => new GiftCategory()
            {
                Id = x.Id,
                GiftName = x.GiftName
            }).OrderBy(x => x.GiftName).ToList();

            return giftCategories;
        }

        public List<MemberChannel> GetChannel()
        {
            _logger.LogDebug($"Get Voucher Categories Enter.");
            var channels = UnitOfWork.MemberChannelRepo.GetAll().Where(x => x.Active).Select(x => new MemberChannel()
            {
                Id = x.Id,
                ChannelName = x.ChannelName
            }).OrderBy(x => x.ChannelName).ToList();

            return channels;
        }

        private string GenerateGiftCode()
        {
            var generator = new RandomGenerator();
            return generator.RandomCode();
        }

        private string UploadedFile(GiftViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public void Send(GiftViewModel model)
        {
            _logger.LogDebug($"Voucher Detail service (Id: {model.Id})");
            var gift = UnitOfWork.GiftRepo.GetById(model.Id);
            var channel = UnitOfWork.MemberChannelRepo.GetAll().Where(m => m.Id == gift.ChannelId).FirstOrDefault();


            List<Member> member = UnitOfWork.MemberRepo.GetAll().Where(m => m.ChannelId == channel.Id).ToList();
            foreach (var item in member)
            {
                var membergiftcheck = UnitOfWork.MemberGiftRepo.GetAll().Where(m => m.MemberId == item.Id && m.GiftId == gift.Id).FirstOrDefault();
                if (membergiftcheck == null)
                {
                    MemberGift mbGift = new MemberGift();
                    mbGift.Id = new Guid();
                    mbGift.MemberId = item.Id;
                    mbGift.GiftId = gift.Id;
                    mbGift.EffectiveFrom = gift.EffectiveDateBegin;
                    mbGift.EffectiveTo = gift.EffectiveDateEnd;
                    UnitOfWork.MemberGiftRepo.Insert(mbGift);
                    UnitOfWork.SaveChanges();
                }
            }
        }






    }
}
