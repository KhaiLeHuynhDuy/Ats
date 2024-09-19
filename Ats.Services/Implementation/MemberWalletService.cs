using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Models.Account;
using Ats.Models.Team;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;
using Ats.Models.MemberWallets;
using Ats.Domain.Member.Models;
using Ats.Domain.Coupon.Models;
using Ats.Models.MemberWallet;

namespace Ats.Services.Implementation
{
    public class MemberWalletService : BaseService, IMemberWalletService
    {
        private IConfiguration _config;
        private int pageSize;

        public MemberWalletService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
        
        public List<MemberWalletHistoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"MemberWalletService.Search={searchText}, Page={page}");

            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();

            var query = UnitOfWork.MemberWalletRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText) || (!String.IsNullOrEmpty(searchText)
                                    && ((!String.IsNullOrEmpty(x.Remark) && x.Remark.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.Email) && x.Member.Email.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.MemberCode) && x.Member.MemberCode.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.FirstName) && x.Member.FirstName.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.PhoneNumber) && x.Member.PhoneNumber.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.LastName) && x.Member.LastName.ToLower().Contains(searchText.ToLower()))))));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {                   
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.Member.MemberCode) : query.OrderByDescending(x => x.Member.MemberCode);
                        break;
                    case "lastname":
                        query = IsAscOrder ? query.OrderBy(x => x.Member.LastName) : query.OrderByDescending(x => x.Member.LastName);
                        break;
                    case "firstname":
                        query = IsAscOrder ? query.OrderBy(x => x.Member.FirstName) : query.OrderByDescending(x => x.Member.FirstName);
                        break;
                    case "email":
                        query = IsAscOrder ? query.OrderBy(x => x.Member.Email) : query.OrderByDescending(x => x.Member.Email);
                        break;
                    case "phone":
                        query = IsAscOrder ? query.OrderBy(x => x.Member.PhoneNumber) : query.OrderByDescending(x => x.Member.PhoneNumber);
                        break;
                    case "point":
                        query = IsAscOrder ? query.OrderBy(x => x.Point) : query.OrderByDescending(x => x.Point);
                        break;
                    case "activeend":
                        query = IsAscOrder ? query.OrderBy(x => x.ActiveEnd) : query.OrderByDescending(x => x.ActiveEnd);
                        break;
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberWalletHistoryViewModel> data = new List<MemberWalletHistoryViewModel>();            
            foreach (var item in datas)
            {
                var memberwallet = UnitOfWork.MemberLoyaltyTransactionRepo.GetAll().Where(x => x.WalletId == item.Id).FirstOrDefault();
                MemberWalletHistoryViewModel itemMembeWallet = new MemberWalletHistoryViewModel();
                itemMembeWallet.Id = item.Id;           
                itemMembeWallet.MemberId = item.Member.Id;
                itemMembeWallet.MemberCode = item.Member.MemberCode;
                itemMembeWallet.FistName = item.Member.FirstName;
                itemMembeWallet.LastName = item.Member.LastName;
                itemMembeWallet.Email = item.Member.Email;
                itemMembeWallet.Phone = item.Member.PhoneNumber;
                itemMembeWallet.Point = item.Point;
             
                itemMembeWallet.ActiveEnd = memberwallet == null ? null : memberwallet.TransactionDate;

                itemMembeWallet.ActiveFrom = item.ActiveFrom;

                data.Add(itemMembeWallet);
            }
            return data;        
        }

        public List<MemberWalletsViewModel> GetMemberWallets()
        {
            _logger.LogDebug($"Get MemberMemberWallet");
            var memberWallet = UnitOfWork.MemberWalletRepo.GetAll().Where(x => x.Active).Select(x => new MemberWalletsViewModel()
            {
                Id = x.Id,
                MemberId = x.Member.Id,// bổ sung sau
                //Point = x.Point,
                Active = x.Active,
                Remark = x.Remark,
                ActiveFrom = x.ActiveFrom,
                ActiveEnd = x.ActiveEnd,
            }).OrderBy(x => x.Id).ToList();

            return memberWallet;
        }
        public int GetMemberWalletsInactiveOfYear(string year)
        {
            _logger.LogDebug($"Get MemberMemberWallet");
            var memberWallet = UnitOfWork.MemberWalletRepo.GetAll().Where(x => x.Active == false).Select(x => new MemberWalletsViewModel()
            {
                Id = x.Id,
                MemberId = x.Member.Id,// bổ sung sau
                //Point = x.Point,
                Active = x.Active,
                Remark = x.Remark,
                ActiveFrom = x.ActiveFrom,
                ActiveEnd = x.ActiveEnd,
            }).OrderBy(x => x.Id).Count();

            return memberWallet;
        }

        public List<MemberWalletHistoryViewModel> GetMemberWalletHistory(Guid Id ,string keyword, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            var history = UnitOfWork.MemberWalletRepo.GetById(Id);

            _logger.LogDebug($"MemberWalletService.Search={keyword}, Page={page}");

            if (!String.IsNullOrEmpty(keyword)) keyword = keyword.Trim();

            var query = history.MemberLoyaltyTransactions.ToList().OrderByDescending(x=> x.TransactionDate);
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    //Don't know what to search for?
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberWalletHistoryViewModel> data = new List<MemberWalletHistoryViewModel>();
          

            foreach (var item in datas)
            {
                MemberWalletHistoryViewModel itemMembeWallet = new MemberWalletHistoryViewModel();
                itemMembeWallet.Id = item.Id;
                itemMembeWallet.MemberCode = item.MemberWallet.Member.MemberCode;
                itemMembeWallet.FistName = item.MemberWallet.Member.FirstName;
                itemMembeWallet.LastName = item.MemberWallet.Member.LastName;
                itemMembeWallet.Email = item.MemberWallet.Member.Email;// bổ sung sau
                itemMembeWallet.Phone = item.MemberWallet.Member.PhoneNumber;// bổ sung sau
                itemMembeWallet.Point = item.Point;
                itemMembeWallet.TransactionDate = item.TransactionDate;
                itemMembeWallet.PointTypeId =item.PointTypeId;
             
           
                //itemMembeWallet.ActiveEnd = memberwallet == null ? null : memberwallet.TransactionDate;
         
                data.Add(itemMembeWallet);
            }
            return data;          
        }

        public List<TransactionViewModel> GetTransactions(Guid walletId, int page, int size, out int total)
        {
            MemberWallet memberwallet = this.UnitOfWork.MemberWalletRepo.GetById(walletId);
            if (memberwallet == null) throw new NullReferenceException($"MemberWallet ({walletId}) Not Found.");

            total = this.UnitOfWork.TransactionRepo.GetAll().Count(x => x.WalletId == walletId);

            List<TransactionViewModel> transactions = this.UnitOfWork.TransactionRepo.GetAll().Where(x => x.WalletId == walletId).OrderBy(x => x.Id).Select(x => new TransactionViewModel()
            {
                Id = x.Id,
                Date = x.Date,
                Action = x.Action,
                Amount = x.Amount,
                RefId = x.RefId,
                TransType = x.TransType,
                Comment = x.Comment,
                WalletId = x.WalletId
            }).Skip((page - 1) * size).Take(size).ToList();

            return transactions;
        }

        public void Transfer(string emailFrom, string emailTo, double amount)
        {
            _logger.LogDebug($"Transfer From: {emailFrom}, To: {emailTo}, Amount: {amount}");

            MemberWallet memberwalletFrom = this.UnitOfWork.MemberWalletRepo.GetWallet(emailFrom);
            if (memberwalletFrom == null) throw new NullReferenceException($"Wallet ({emailFrom}) Not Found.");

            MemberWallet memberwalletTo = this.UnitOfWork.MemberWalletRepo.GetWallet(emailTo);
            if (memberwalletTo == null) throw new NullReferenceException($"Wallet ({emailTo}) Not Found.");

            Transaction transaction1 = new Transaction()
            {
                WalletId = memberwalletFrom.Id,
                RefId = memberwalletTo.Id,
                Amount = -amount,
                TransType = TRANSACTION_TYPE.TRANSFERED,
                Action = false,
                Date = DateTime.UtcNow
            };

            if (memberwalletFrom.Point < amount) throw new InvalidOperationException($"Invalid Point!");  //I don't understand ! 

            memberwalletFrom.Point -= amount;
            memberwalletFrom.ActiveEnd = DateTime.UtcNow;
            this.UnitOfWork.MemberWalletRepo.Update(memberwalletFrom, x => x.Point, x => x.ActiveEnd);

            Transaction transaction2 = new Transaction()
            {
                WalletId = memberwalletTo.Id,
                RefId = memberwalletFrom.Id,
                Amount = amount,
                TransType = TRANSACTION_TYPE.TRANSFERED,
                Action = true,
                Date = DateTime.UtcNow
            };

            memberwalletTo.Point += amount;
            memberwalletTo.ActiveEnd = DateTime.UtcNow;
            this.UnitOfWork.MemberWalletRepo.Update(memberwalletTo, x => x.Point, x => x.ActiveEnd);

            this.UnitOfWork.TransactionRepo.Insert(transaction1);
            this.UnitOfWork.TransactionRepo.Insert(transaction2);

            this.UnitOfWork.SaveChanges();

            //_logger.LogDebug($"CreateWallet done {wallet.Id}");
        }

        public Guid CreateMemberWallet(MemberWalletsViewModel model)
        {
            _logger.LogDebug($"Create MemberWallet (Name: {model.Id})");
            var entity = UnitOfWork.MemberWalletRepo.GetById(model.Id);

            //Id = model.Id,
            entity.Point = model.Point;                                
            UnitOfWork.MemberWalletRepo.Update(entity);
            UnitOfWork.SaveChanges();

            return entity.Id;
        }

        public int UpdateMemberWallet(MemberWalletsViewModel model)
        {
            _logger.LogDebug($"Edit MemberWallet (Id: {model.Id})");
            
            string phone = model.Phone == null ? "" : model.Phone;
            string email = model.Email == null ? "" : model.Email;
            double TotalPoint = 0;
            //var member = UnitOfWork.MemberRepo.GetAll().Where(x => x.PhoneNumber.Contains(phone) || x.Email.Contains(email)).FirstOrDefault();       
            List<Member> member = UnitOfWork.MemberRepo.GetAll().Where(x => x.PhoneNumber==phone || x.Email==email).ToList();       
            
            
            if(member.Count > 1)
            {
                return 0;
            }
            else
            {
                try
                {
                    var memberwallet = UnitOfWork.MemberWalletRepo.GetAll().Where(x => x.MemberId == member[0].Id).FirstOrDefault();
                    var entity = UnitOfWork.MemberWalletRepo.GetById(memberwallet.Id);
                    if (entity != null)
                    {

                        TotalPoint = model.Point + entity.Point;
                        entity.Point = TotalPoint;
                        UnitOfWork.MemberWalletRepo.Update(entity);
                        UnitOfWork.SaveChanges();
                    }

                    #region Check Work, update later 😁
                    // them data bi trong cho lich su MemberLoyaltyTransaction
                    //Random rn = new Random();
                    //Coupon coupon = new Coupon();
                    //coupon.Id = new Guid();
                    //coupon.CouponCode = rn.ToString();
                    //coupon.StoreId = member.FirstOrDefault().StoreId;
                    //coupon.ChannelId = member[0].ChannelId;
                    //coupon.CouponCategoryId = model.CouponCategoryId.HasValue ? model.CouponCategoryId : (int?)null;
                    //UnitOfWork.CouponRepo.Insert(coupon);

                    //Campaign campaign = new Campaign();
                    //campaign.Id = new Guid();
                    //UnitOfWork.CampaignRepo.Insert(campaign);

                    //Tao moi mot cai lich su MemberLoyaltyTransaction
                    MemberLoyaltyTransaction mbLoyalty = new MemberLoyaltyTransaction();
                    mbLoyalty.Id = new Guid();
                    mbLoyalty.WalletId = memberwallet.Id;
                    mbLoyalty.Point = model.Point;
                    mbLoyalty.TransactionDate = memberwallet.ChangedTimestamp;
                    mbLoyalty.StoreId = member.FirstOrDefault().StoreId;
                    mbLoyalty.ChannelId = member.FirstOrDefault().ChannelId;
                    mbLoyalty.PointTypeId = 1;
                    mbLoyalty.CouponId = null;
                    UnitOfWork.MemberLoyaltyTransactionRepo.Insert(mbLoyalty);
                    #endregion

                    UnitOfWork.SaveChanges();
                    return 1;
                }
                catch (Exception ex)
                {

                    return 0;
                }
            }
            // Add Point
           
           
        }


        public void DeleteMemberWallet(Guid id)
        {
            _logger.LogDebug($"Delete MemberWallet service (Id: {id})");
            var entity = UnitOfWork.MemberWalletRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.MemberWalletRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }


        //public List<MemberWallet> mbWallet(Guid WalletId)check data walletMember
        //{
        //    var entity = UnitOfWork.MemberWalletRepo.mbWallet(WalletId);

        //    return entity;
        //}

        public MemberWalletsViewModel GetMemberWallet(Guid IdMember)
        {
            var entity = UnitOfWork.MemberWalletRepo.GetAll().FirstOrDefault(m => m.MemberId == IdMember);
            var model = new MemberWalletsViewModel()
            {
                MemberId = entity.MemberId,
                Point = entity.Point
            }; 
            return model;
        }

        public List<MemberWalletDisplayItem> SearchMemberSend(string searchText, int page, int size)
        {
            _logger.LogDebug($"MemberWalletService.Search={searchText}");

            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.MemberWalletRepo.GetAll().Where(x => (String.IsNullOrEmpty(searchText)||(!String.IsNullOrEmpty(searchText)
                                    && ((!String.IsNullOrEmpty(x.Member.Email) && x.Member.Email.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.MemberCode) && x.Member.MemberCode.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.FirstName) && x.Member.FirstName.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.PhoneNumber) && x.Member.PhoneNumber.ToLower().Contains(searchText.ToLower()))
                                    || (!String.IsNullOrEmpty(x.Member.LastName) && x.Member.LastName.ToLower().Contains(searchText.ToLower()))))));


            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<MemberWalletDisplayItem> data = new List<MemberWalletDisplayItem>();
            foreach (var item in datas)
            {
                MemberWalletDisplayItem itemMembeWallet = new MemberWalletDisplayItem();
                itemMembeWallet.Selected = false;
                itemMembeWallet.Value = item.Id.ToString();
                itemMembeWallet.MemberCode = item.Member.MemberCode;
                itemMembeWallet.FistName = item.Member.FirstName;
                itemMembeWallet.LastName = item.Member.LastName;
                itemMembeWallet.Email = item.Member.Email;
                itemMembeWallet.Phone = item.Member.PhoneNumber;
                data.Add(itemMembeWallet);
            }
            return data;
        }
    }
}
