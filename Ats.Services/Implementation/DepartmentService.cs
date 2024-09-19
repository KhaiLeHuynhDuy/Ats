using Ats.Commons.Utilities;
using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Departments.Models;
using Ats.Models.Deparment;
using Ats.Models.Department;
using Ats.Models.User;
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
    public class DepartmentService : BaseService, IDepartmentService
    {
        private IConfiguration _config;
        private int pageSize;

        public DepartmentService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<DepartmenttViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Departments service Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.DepartmentRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText)
                                || (x.DepartmentCode != null && x.DepartmentCode.ToLower().Contains(searchText.ToLower()))
                                || (x.DepartmentName != null && x.DepartmentName.ToLower().Contains(searchText.ToLower()))
                                || (x.Manager != null && x.Manager.FirstName != null && x.Manager.FirstName.ToLower().Contains(searchText.ToLower()))
                                || (x.Manager != null && x.Manager.LastName != null && x.Manager.LastName.ToLower().Contains(searchText.ToLower()))
                                ).OrderBy(x => x.DepartmentName);
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "code":
                        query = IsAscOrder ? query.OrderBy(x => x.DepartmentCode) : query.OrderByDescending(x => x.DepartmentCode);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.DepartmentName) : query.OrderByDescending(x => x.DepartmentName);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.IsActive) : query.OrderByDescending(x => x.IsActive);
                        break;
                    case "manager":
                        query = IsAscOrder ? query.OrderBy(x => x.Manager) : query.OrderByDescending(x => x.Manager);
                        break;
                }
            }
            var datas = query.Skip((page - 1) * size).Take(size).ToList();
            List<DepartmenttViewModel> data = new List<DepartmenttViewModel>();
            foreach (var item in datas)
            {
                DepartmenttViewModel department = new DepartmenttViewModel();
                department.Id = item.Id;
                department.Code = item.DepartmentCode;
                department.Name = item.DepartmentName;
                if (item.Manager != null)
                {
                    department.Manager = new UserrProfileViewModel() { Id = item.Manager.Id, FirstName = item.Manager.FirstName, LastName = item.Manager.LastName, Email = item.Manager.Email, Phone = item.Manager.PhoneNumber, UserName = item.Manager.UserName };
                }
                department.IsActive = item.IsActive;
                data.Add(department);
            }
            return data;
        }

        public DepartmenttViewModel GetDepartment(Guid id)
        {
            _logger.LogDebug($"Department Detail service (Id: {id})");
            var entity = UnitOfWork.DepartmentRepo.GetById(id);
            if (entity != null)
            {
                var model = new DepartmenttViewModel()
                {
                    Id = entity.Id,
                    Code = entity.DepartmentCode,
                    ManagerId = entity.ManagerId,
                    Name = entity.DepartmentName,
                    Description = entity.Note,
                    IsActive = entity.IsActive
                };
                if (entity.Manager != null)
                {
                    model.Manager = new UserrProfileViewModel() { Id = entity.Manager.Id, FirstName = entity.Manager.FirstName, LastName = entity.Manager.LastName, Email = entity.Manager.Email, Phone = entity.Manager.PhoneNumber, UserName = entity.Manager.UserName };
                }

                foreach (DepartmentUser item in entity.Users.OrderBy(x => x.User.FirstName))
                {
                    var m = new DepartmentUserModel()
                    {
                        Id = item.Id,
                        FirstName = item.User.FirstName,
                        LastName = item.User.LastName,
                        Email = item.User.Email,
                        PhoneNumber = item.User.PhoneNumber,
                        UserName = item.User.UserName,
                        Tag = item.Role.ToName(),
                        StartDate = item.StartDate,
                        EndDate = item.EndDate
                    };

                    model.DepartmentUsers.Add(m);
                }
                return model;
            }
            return null;
        }
        public void CreateDepartment(DepartmenttViewModel model)
        {
            _logger.LogDebug($"Create Department service (Name: {model.Name})");
            var entity = new Department
            {
                Id = model.Id,
                DepartmentCode = model.Code,
                DepartmentName = model.Name,
                ManagerId = model.ManagerId,
                IsActive = model.IsActive
            };

            UnitOfWork.DepartmentRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateDepartment(DepartmenttViewModel model)
        {
            _logger.LogDebug($"Edit Department service (Id: {model.Id})");
            var entity = UnitOfWork.DepartmentRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.DepartmentCode = model.Code;
                entity.DepartmentName = model.Name;
                entity.ManagerId = model.ManagerId;
                entity.Note = model.Description;
                entity.IsActive = model.IsActive;
                UnitOfWork.DepartmentRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteDepartment(Guid id)
        {
            _logger.LogDebug($"Delete Department service (Id: {id})");
            var entity = UnitOfWork.DepartmentRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.DepartmentRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
    }
}
