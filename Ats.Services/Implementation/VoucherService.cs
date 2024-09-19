using Ats.Domain;
using Ats.Domain.Voucher.Models;
using Ats.Models.Voucher;
using Ats.Services.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Ats.Commons;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Ats.Domain.Member.Models;

namespace Ats.Services.Implementation
{
    public class VoucherService : BaseService, IVoucherService
    {
        private IConfiguration _config;
        private IHostingEnvironment _hostingEnvironment;
        private int pageSize;

        public VoucherService(IUnitOfWork unitOfWork, IConfiguration config, IHostingEnvironment hostingEnvironment, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<VoucherViewModel> Search(string searchText, int? voucherCateid, int? channelid, bool? voucherType, int? stockFrom, int? stockTo, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.VoucherRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                                   && (!String.IsNullOrEmpty(x.VoucherCode) && x.VoucherCode.ToLower().Contains(searchText.ToLower()))
                                   || (!String.IsNullOrEmpty(x.VoucherName) && x.VoucherName.ToLower().Contains(searchText.ToLower()))))
                                   && (voucherCateid == 0 || voucherCateid == null ? true : x.VoucherCategoryId == voucherCateid)
                                   && (channelid == 0 || channelid == null ? true : x.ChannelId == channelid)
                                   && (voucherType == null ? true : x.VoucherType == voucherType)
                                   && ((stockFrom == null && stockTo == null)||
                                   (stockFrom == null && stockTo != null && x.AvailableStock <= stockTo)||
                                   (stockTo == null && stockFrom != null && x.AvailableStock >= stockFrom)||
                                   (stockFrom != null && stockTo != null && x.AvailableStock >= stockFrom && x.AvailableStock <= stockTo)));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "type":
                        query = IsAscOrder ? query.OrderBy(x => x.VoucherType) : query.OrderByDescending(x => x.VoucherType);
                        break;
                    case "channel":
                        query = IsAscOrder ? query.OrderBy(x => x.Channel.ChannelName) : query.OrderByDescending(x => x.Channel.ChannelName);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.VoucherName) : query.OrderByDescending(x => x.VoucherName);
                        break;
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.VoucherCode) : query.OrderByDescending(x => x.VoucherCode);
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
            List<VoucherViewModel> data = new List<VoucherViewModel>();
            foreach (var item in datas)
            {
                VoucherViewModel voucher = new VoucherViewModel();
                voucher.Id = item.Id;
                voucher.VoucherCode = item.VoucherCode;
                voucher.VoucherName = item.VoucherName;
                voucher.VoucherType = item.VoucherType;
                if (item.VoucherCategory != null) voucher.VoucherCategoryName = item.VoucherCategory.VoucherName;
                voucher.Desc = item.Desc;
                voucher.Active = item.Active;
                voucher.AfterEffectiveDay = item.AfterEffectiveDay;
                voucher.AfterReceptionDay = item.AfterReceptionDay; 
                voucher.ValidityPeriodType = item.ValidityPeriodType; 
                voucher.EffectiveDateBegin = item.EffectiveDateBegin; 
                voucher.EffectiveDateEnd = item.EffectiveDateEnd; 

                voucher.AvailableStock = item.AvailableStock;
                //voucher.ChannelId = item.ChannelId;
                if (item.Channel != null) voucher.ChannelName = item.Channel.ChannelName;
                voucher.uniqueFileName = item.TemplateBanner;
                voucher.EffectualDateBegin = string.Format("{0:dd/MM/yyyy}", item.EffectiveDateBegin);
                voucher.EffectualDateEnd = string.Format("{0:dd/MM/yyyy}", item.EffectiveDateEnd);
                data.Add(voucher);
            }

            return data;
        }

        public List<VoucherViewModel> GetVouchers()
        {
            _logger.LogDebug($"Get all Voucher");
            var vouchers = UnitOfWork.VoucherRepo.GetAll().Where(x => x.Active).Select(x => new VoucherViewModel()
            {

                Id = x.Id,
                VoucherCode = x.VoucherCode,
                VoucherName = x.VoucherName,
                VoucherCategoryId = x.VoucherCategoryId,
                Desc = x.Desc,
                Active = x.Active,

                AvailableStock = x.AvailableStock,
                ChannelId = x.ChannelId,
                uniqueFileName = x.TemplateBanner

            }).OrderBy(x => x.VoucherName).ToList();

            return vouchers;
        }

        public VoucherViewModel GetVoucher(Guid id)
        {
            _logger.LogDebug($"Voucher Detail service (Id: {id})");
            var entity = UnitOfWork.VoucherRepo.GetById(id);
            if (entity != null)
            {
                var model = new VoucherViewModel()
                {

                    Id = entity.Id,
                    VoucherCode = entity.VoucherCode,
                    VoucherName = entity.VoucherName,
                    VoucherCategoryId = entity.VoucherCategoryId,

                    Templates = entity.Template,
                    ChannelId = entity.ChannelId,
                    FaceValue = entity.FaceValue,
                    Price = entity.Price,

                    VoucherType = entity.VoucherType,
                    ValidityPeriodType = entity.ValidityPeriodType,
                    MinimumPurchase = entity.MinimumPurchase,
                    TermAndConditions = entity.TermAndConditions,
                     
                    Desc = entity.Desc,
                    Active = entity.Active,
                    AvailableStock = entity.AvailableStock,
                    uniqueFileName = entity.TemplateBanner
                };
                if(entity.VoucherCategory != null)
                {
                    model.VoucherCategoryName = entity.VoucherCategory.VoucherName;
                }
                else
                {
                    model.VoucherCategoryName = " ";
                }
                if (entity.Channel != null)
                {
                    model.ChannelName = entity.Channel.ChannelName;
                }
                else
                {
                    model.ChannelName = " ";
                }
                if (entity.VoucherType == true)
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
        public void CreateVoucher(VoucherViewModel model)
        {
            string uniqueFileName = UploadedFile(model);

            _logger.LogDebug($"Create Voucher (Name: {model.VoucherName})");
            Voucher entity = new Voucher();
            entity.Id = Guid.NewGuid(); 
            if(model.VoucherCodeAutomatically == true)
            {
                entity.VoucherCode = this.GenerateMemberCode();
                _logger.LogDebug($"Creating Coupon (CodeAuto: {model.VoucherCode})");
            }
            else
            {
                entity.VoucherCode = model.VoucherCode;
            }
            entity.VoucherName = model.VoucherName;
            entity.VoucherCategoryId = model.VoucherCategoryId;
            entity.Template = model.Templates;
            entity.ChannelId = model.ChannelId;
            entity.FaceValue = model.FaceValue;
            entity.Price = model.Price;
            ////VoucherType
            entity.VoucherType = model.VoucherType;
            if (entity.VoucherType == true)
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

            entity.MinimumPurchase = model.MinimumPurchase;
            ////image
            entity.TemplateBanner = uniqueFileName;
            entity.TermAndConditions = model.TermAndConditions;
            entity.Desc = model.Desc;
            entity.Active = true;
            entity.AvailableStock = model.AvailableStock;

            UnitOfWork.VoucherRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateVoucher(VoucherViewModel model)
        {
            _logger.LogDebug($"Edit Voucher (Id: {model.Id})");
            var entity = UnitOfWork.VoucherRepo.GetById(model.Id);
            if (entity != null)
            {
                model.uniqueFileName = UploadedFile(model);
                entity.VoucherCode = model.VoucherCode;
                entity.VoucherName = model.VoucherName;
                entity.VoucherCategoryId = model.VoucherCategoryId;
                entity.Template = model.Templates;
                entity.ChannelId = model.ChannelId;
                entity.FaceValue = model.FaceValue;
                entity.Price = model.Price;
                ////VoucherType
                entity.VoucherType = model.VoucherType;
                if (entity.VoucherType == true)
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

                entity.MinimumPurchase = model.MinimumPurchase;
                entity.TemplateBanner = model.uniqueFileName;
                entity.TermAndConditions = model.TermAndConditions;
                entity.Desc = model.Desc;
                entity.Active = true;
                entity.AvailableStock = model.AvailableStock;

                UnitOfWork.VoucherRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteVoucher(Guid id)
        {
            _logger.LogDebug($"Delete Voucher (Id: {id})");
            var entity = UnitOfWork.VoucherRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.VoucherRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<VoucherCategory> GetVoucherCategories()
        {
            _logger.LogDebug($"Get Voucher Categories Enter.");
            var voucherCategories = UnitOfWork.VoucherCategoryRepo.GetAll().Where(x => x.Active).Select(x => new VoucherCategory()
            {
                Id = x.Id,
                VoucherName = x.VoucherName
            }).OrderBy(x => x.VoucherName).ToList();

            return voucherCategories;
        }
        private string UploadedFile(VoucherViewModel model)
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

        private string GenerateMemberCode()
        {
            var generator = new RandomGenerator();
            return generator.RandomCode();
        }

        public List<VoucherViewModel> Reset(string searchText, string orderField, bool IsAscOrder, bool? couponType, int? channelId, string fromEffectiveDateBegin, string toEffectiveDateBegin, string fromEffectiveDateEnd, string toEffectiveDateEnd, EXPIRY_COUPON? expiry_coupon, int page, int size, out int total)
        {
            throw new NotImplementedException();
        }



        public void Send(VoucherViewModel model )
        {
            _logger.LogDebug($"Voucher Detail service (Id: {model.Id})");
            var voucher = UnitOfWork.VoucherRepo.GetById(model.Id);
            var channel = UnitOfWork.MemberChannelRepo.GetAll().Where(m => m.Id == voucher.ChannelId).FirstOrDefault();
            

            List<Member> member = UnitOfWork.MemberRepo.GetAll().Where(m => m.ChannelId == channel.Id).ToList();
            foreach(var item in member)
            {
                var membervouchercheck = UnitOfWork.MemberVoucherRepo.GetAll().Where(m => m.MemberId == item.Id && m.VoucherId == voucher.Id).FirstOrDefault();
                if (membervouchercheck == null)
                {
                    MemberVoucher mbVoucher = new MemberVoucher();
                    mbVoucher.Id = new Guid();
                    mbVoucher.MemberId = item.Id;
                    mbVoucher.VoucherId = voucher.Id;
                    mbVoucher.EffectiveFrom = voucher.EffectiveDateBegin;
                    mbVoucher.EffectiveTo = voucher.EffectiveDateEnd;
                    UnitOfWork.MemberVoucherRepo.Insert(mbVoucher);
                    UnitOfWork.SaveChanges();
                } 
            } 
        }











    }
}
