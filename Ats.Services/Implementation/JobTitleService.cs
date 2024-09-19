using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Models.JobTitle;
using Ats.Services.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class JobTitleService : BaseService, IJobTitleService
    {
        private IConfiguration _config;
        private int pageSize;

        public JobTitleService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
        public List<JobTitleViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Job titles service Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.JobTitleRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                }
            }

            var data = query.Select(x => new JobTitleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }
        public JobTitleViewModel GetJobTitle(int id)
        {
            _logger.LogDebug($"Job title Detail service (Id: {id})");
            var entity = UnitOfWork.JobTitleRepo.GetById(id);
            if (entity != null)
            {
                var model = new JobTitleViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                };

                return model;
            }
            return null;
        }
        public void CreateJobTitle(JobTitleViewModel model)
        {
            _logger.LogDebug($"Create Job title service (Name: {model.Name})");
            var entity = new JobTitle
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };

            UnitOfWork.JobTitleRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateJobTitle(JobTitleViewModel model)
        {
            _logger.LogDebug($"Edit Job title service (Id: {model.Id})");
            var entity = UnitOfWork.JobTitleRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.IsActive = model.IsActive;
                UnitOfWork.JobTitleRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteJobTitle(int id)
        {
            _logger.LogDebug($"Delete Job title service (Id: {id})");
            var entity = UnitOfWork.JobTitleRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.JobTitleRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }

        public List<JobTitleViewModel> GetJobTitles()
        {
            _logger.LogDebug($"Get job titles");
            var jobTitles = UnitOfWork.JobTitleRepo.GetAll().Where(x => x.IsActive).Select(x=> new JobTitleViewModel() { 
            Id = x.Id,
            Name = x.Name}).OrderBy(x => x.Name).ToList();
            
            return jobTitles;
        }        
    }
}
