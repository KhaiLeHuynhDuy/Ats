using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Lead.Models;
using Ats.Models.LoanBook;
using Ats.Models.User;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class LoanBookService : BaseService, ILoanBookService
    {
        private IConfiguration _config;
        private int pageSize;

        public LoanBookService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<LoanBookViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all LoanBooks service Search={searchText}, Page={page}");
            var query = UnitOfWork.LoanBookRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) 
                /*|| x.Name != null && x.Name.ToLower().Contains(searchText.ToLower())*/);
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    //case "isactive":
                    //    query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                    //    break;
                    //case "name":
                    //    query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                    //    break;
                }
            }

            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<LoanBookViewModel> data = new List<LoanBookViewModel>();
            foreach (var item in datas)
            {
                LoanBookViewModel loanBook = new LoanBookViewModel();
                loanBook.Id = item.Id;
                loanBook.Price = item.Price;
                loanBook.BookType = item.BookType;
                loanBook.BookDate = item.BookDate;
                if(item.User != null)
                {
                    loanBook.User = new UserrProfileViewModel() { Id = item.User.Id, FirstName = item.User.FirstName, LastName = item.User.LastName, Email = item.User.Email, Phone = item.User.PhoneNumber, UserName = item.User.UserName };
                }
                data.Add(loanBook);
            }
            return data;
        }

        public List<LoanBookViewModel> GetLoanBooks()
        {
            _logger.LogDebug($"GetLoanBooks");
            var LoanBooks = UnitOfWork.LoanBookRepo.GetAll().Select(x => new LoanBookViewModel()
            {
                Id = x.Id,
                Price = x.Price,
                BookType = x.BookType,
            }).ToList();

            return LoanBooks;
        }

        public LoanBookViewModel GetLoanBook(Guid id)
        {
            _logger.LogDebug($"LoanBook Detail service (Id: {id})");
            var entity = UnitOfWork.LoanBookRepo.GetById(id);
            if (entity != null)
            {
                var model = new LoanBookViewModel()
                {
                    Id = entity.Id,
                    Price = entity.Price,
                    BookType = entity.BookType,
                    BookDate = entity.BookDate,
                    
                };

                return model;
            }
            return null;
        }
        public void CreateLoanBook(LoanBookViewModel model)
        {
            _logger.LogDebug($"Create LoanBook service");
            var entity = new LoanBook
            {
                Id = model.Id,
                Price = model.Price,
                BookType = model.BookType,
                BookDate = DateTime.UtcNow,
                LoanProductId = model.LoanProductId,
                LeadId = model.LeadId,
                BorrowerId = model.BorrowerId,
                UserId = model.UserId,

            };

            UnitOfWork.LoanBookRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateLoanBook(LoanBookViewModel model)
        {
            _logger.LogDebug($"Edit LoanBook service (Id: {model.Id})");
            var entity = UnitOfWork.LoanBookRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Price = model.Price;
                entity.BookType = model.BookType;
                UnitOfWork.LoanBookRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteLoanBook(Guid id)
        {
            _logger.LogDebug($"Delete LoanBook service (Id: {id})");
            var entity = UnitOfWork.LoanBookRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.LoanBookRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
