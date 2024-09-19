using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Lead.Models;
using Ats.Models.Comment;
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
    public class CommentService : BaseService, ICommentService
    {
        private IConfiguration _config;
        private int pageSize;

        public CommentService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
        public List<CommentViewModel> GetListComments(Guid leadId)
        {
            _logger.LogDebug($"Get all Comments service");
            var query = UnitOfWork.CommentRepo.GetAll().Where(x => x.LeadId == leadId);
            var datas = query.ToList();
            List<CommentViewModel> data = new List<CommentViewModel>();
            foreach (var item in datas)
            {
                CommentViewModel comment = new CommentViewModel();
                comment.Id = item.Id;
                comment.CommentText = item.CommentText;
                comment.CommentType = item.CommentType;
                comment.CommentDate = item.CommentDate;
                if (item.User != null)
                {
                    comment.User = new UserrProfileViewModel() { Id = item.User.Id, FirstName = item.User.FirstName, LastName = item.User.LastName, Email = item.User.Email, Phone = item.User.PhoneNumber, UserName = item.User.UserName };
                }
                data.Add(comment);
            }
            return data;
        }
        public List<CommentViewModel> GetListComments(Guid leadId, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Comments service, Page={page}");
            var query = UnitOfWork.CommentRepo.GetAll().Where(x => x.LeadId == leadId);
            total = query.Count();

            var data = query.Select(x => new CommentViewModel()
            {
                Id = x.Id,
                CommentText = x.CommentText,
                CommentType = x.CommentType,
                CommentDate = x.CommentDate,
                LeadId = x.LeadId,
                UserId = x.UserId,
                BorrowerId = x.BorrowerId,
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public CommentViewModel GetComment(Guid id)
        {
            _logger.LogDebug($"Comment Detail service (Id: {id})");
            var entity = UnitOfWork.CommentRepo.GetById(id);
            if (entity != null)
            {
                var model = new CommentViewModel()
                {
                    Id = entity.Id,
                    CommentText = entity.CommentText,
                    CommentType = entity.CommentType,
                    LeadId = entity.LeadId,
                    UserId = entity.UserId,
                    BorrowerId = entity.BorrowerId,
                };

                return model;
            }
            return null;
        }
        public void CreateComment(CommentViewModel model)
        {
            _logger.LogDebug($"Create Comment service");
            var entity = new Comment
            {
                Id = model.Id,
                CommentText = model.CommentText,
                CommentType = model.CommentType,
                LeadId = model.LeadId,
                UserId = model.UserId,
                BorrowerId = model.BorrowerId,
                CommentDate = DateTime.UtcNow
            };

            UnitOfWork.CommentRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateComment(CommentViewModel model)
        {
            _logger.LogDebug($"Edit Comment service (Id: {model.Id})");
            var entity = UnitOfWork.CommentRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.CommentText = model.CommentText;
                entity.CommentType = model.CommentType;
                UnitOfWork.CommentRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteComment(Guid id)
        {
            _logger.LogDebug($"Delete Comment service (Id: {id})");
            var entity = UnitOfWork.CommentRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.CommentRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #region
        public List<CommentViewModel> GetListBorrowerComments(Guid borrowerId)
        {
            _logger.LogDebug($"Get all Comments service");
            var query = UnitOfWork.CommentRepo.GetAll().Where(x => x.BorrowerId == borrowerId);
            var datas = query.ToList();
            List<CommentViewModel> data = new List<CommentViewModel>();
            foreach (var item in datas)
            {
                CommentViewModel comment = new CommentViewModel();
                comment.Id = item.Id;
                comment.CommentText = item.CommentText;
                comment.CommentType = item.CommentType;
                comment.CommentDate = item.CommentDate;
                if (item.User != null)
                {
                    comment.User = new UserrProfileViewModel() { Id = item.User.Id, FirstName = item.User.FirstName, LastName = item.User.LastName, Email = item.User.Email, Phone = item.User.PhoneNumber, UserName = item.User.UserName };
                }
                data.Add(comment);
            }
            return data;
        }
        #endregion
    }
}
