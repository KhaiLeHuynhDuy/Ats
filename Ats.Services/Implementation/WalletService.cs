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

namespace Ats.Services.Implementation
{
    public class WalletService : BaseService, IWalletService
    {
        private IConfiguration _config;
        private int pageSize;

        public WalletService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
        
        public Guid CreateWallet(string username)
        {
            _logger.LogDebug($"CreateWallet={username}");

            Guid userId = this.UnitOfWork.UserRepo.GetUserId(username);

            Wallet wallet = new Wallet()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
                UserId = userId
            };

            this.UnitOfWork.WalletRepo.Insert(wallet);
            this.UnitOfWork.SaveChanges();

            _logger.LogDebug($"CreateWallet done {wallet.Id}");
            return wallet.Id;
        }

        public List<WalletViewModel> Search(string keyword, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"WalletService.Search={keyword}, Page={page}");

            if (!String.IsNullOrEmpty(keyword)) keyword = keyword.Trim();

            var query = UnitOfWork.WalletRepo.GetAll().Where(x => (String.IsNullOrEmpty(keyword) || (!String.IsNullOrEmpty(keyword)
                                    && ((!String.IsNullOrEmpty(x.User.UserName) && x.User.UserName.ToLower().Contains(keyword.ToLower()))
                                    || (!String.IsNullOrEmpty(x.User.Email) && x.User.Email.ToLower().Contains(keyword.ToLower()))))));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "username":
                        query = IsAscOrder ? query.OrderBy(x => x.User.UserName) : query.OrderByDescending(x => x.User.UserName);
                        break;
                    case "email":
                        query = IsAscOrder ? query.OrderBy(x => x.User.Email) : query.OrderByDescending(x => x.User.Email);
                        break;
                }
            }

            var data = query.Select(x => new WalletViewModel()
            {
                Id = x.Id,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Email = x.User.Email,
                WalletName = x.WalletName,
                Balance = x.Balance,
                CreatedDate = x.CreatedDate,
                LastUpdate = x.LastUpdate,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public WalletViewModel GetWallet(Guid id)
        {
            Wallet wallet = this.UnitOfWork.WalletRepo.GetById(id);
            if (wallet == null) throw new NullReferenceException($"Wallet ({id}) Not Found.");

            WalletViewModel model = new WalletViewModel()
            {
                Id = wallet.Id,
                WalletName = wallet.WalletName,
                Balance = wallet.Balance,
                LastUpdate = wallet.LastUpdate,
                CreatedDate = wallet.CreatedDate
            };

            return model;
        }

        public List<TransactionViewModel> GetTransactions(Guid walletId, int page, int size, out int total)
        {
            Wallet wallet = this.UnitOfWork.WalletRepo.GetById(walletId);
            if (wallet == null) throw new NullReferenceException($"Wallet ({walletId}) Not Found.");

            total = this.UnitOfWork.TransactionRepo.GetAll().Count(x => x.WalletId == walletId);

            List <TransactionViewModel> transactions = this.UnitOfWork.TransactionRepo.GetAll().Where(x => x.WalletId == walletId).OrderBy(x => x.Id).Select(x => new TransactionViewModel() { 
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

            Wallet walletFrom = this.UnitOfWork.WalletRepo.GetWallet(emailFrom);
            if (walletFrom == null) throw new NullReferenceException($"Wallet ({emailFrom}) Not Found.");

            Wallet walletTo = this.UnitOfWork.WalletRepo.GetWallet(emailTo);
            if (walletTo == null) throw new NullReferenceException($"Wallet ({emailTo}) Not Found.");

            Transaction transaction1 = new Transaction()
            {                
                WalletId = walletFrom.Id,
                RefId = walletTo.Id,
                Amount = -amount,
                TransType = TRANSACTION_TYPE.TRANSFERED,
                Action = false,
                Date = DateTime.UtcNow
            };

            if (walletFrom.Balance < amount) throw new InvalidOperationException($"Invalid Balance!");

            walletFrom.Balance -= amount;
            walletFrom.LastUpdate = DateTime.UtcNow;
            this.UnitOfWork.WalletRepo.Update(walletFrom, x => x.Balance, x => x.LastUpdate);

            Transaction transaction2 = new Transaction()
            {
                WalletId = walletTo.Id,
                RefId = walletFrom.Id,
                Amount = amount,
                TransType = TRANSACTION_TYPE.TRANSFERED,
                Action = true,
                Date = DateTime.UtcNow
            };

            walletTo.Balance += amount;
            walletTo.LastUpdate = DateTime.UtcNow;
            this.UnitOfWork.WalletRepo.Update(walletTo, x => x.Balance, x => x.LastUpdate);

            this.UnitOfWork.TransactionRepo.Insert(transaction1);
            this.UnitOfWork.TransactionRepo.Insert(transaction2);

            this.UnitOfWork.SaveChanges();

            //_logger.LogDebug($"CreateWallet done {wallet.Id}");
        }
        public void DeleteWallet(Guid id)
        {
            _logger.LogDebug($"Delete Wallet service (Id: {id})");
            var entity = UnitOfWork.WalletRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.WalletRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
