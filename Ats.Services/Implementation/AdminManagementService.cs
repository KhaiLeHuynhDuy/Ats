using System.Collections.Generic;
using Ats.Domain;
using Ats.Commons;
using System;
using System.Linq;
using Ats.Models;
using Ats.Commons.Utilities;
using static Ats.Commons.Constants;
using static Ats.Commons.DateTimeExtensions;
using System.Configuration;
using Ats.Models.Department;
using Ats.Commons.Resource;
using Ats.Models.Commons;
using Ats.Models.Account;
using Ats.Domain.Departments.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using Ats.Models.Deparment;
using System.Globalization;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ats.Domain.Identity.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Security.Principal;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{

    public class AdminManagementService : BaseService, IAdminManagementService
    {
        private IConfiguration _config;
        private int pageSize;
              
        public AdminManagementService(IUnitOfWork unitOfWork, IConfiguration config) : base(unitOfWork)
        {
            this._config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
       
        #region Departments
        public IEnumerable<DisplayItem> GetUserManagementDepartment(Guid userId)
        {
            var items = this.UnitOfWork.DepartmentUserRepo.GetAll().Where(x => x.User.Id.Equals(userId) && (x.Role == DEPARTMENT_ROLE.MANAGER || x.Role == DEPARTMENT_ROLE.OWNER || x.Role == DEPARTMENT_ROLE.WATCHER)).AsEnumerable().Select(x => new DisplayItem { Text = String.Format("{0} | {1}", x.Department.DepartmentCode, x.Department.DepartmentName), Value = x.DepartmentId.ToString() });
            return items;
        }
        public IEnumerable<DisplayItem> GetUserManagementDepartmentFilter(Guid userId)
        {
            var now = DateTime.Now;
            DateTime nowFormat = new DateTime(now.Year, now.Month, now.Day);
            var items = this.UnitOfWork.DepartmentUserRepo
                .GetAll().Where(x => x.User.Id.Equals(userId)
                && (x.Role == DEPARTMENT_ROLE.MANAGER || x.Role == DEPARTMENT_ROLE.OWNER || x.Role == DEPARTMENT_ROLE.WATCHER)
                && ((x.StartDate <= nowFormat && x.EndDate >= nowFormat)
                    || (x.StartDate <= nowFormat && x.EndDate == null)
                    )
                ).AsEnumerable()
                .GroupBy(x => x.DepartmentId)
                .Select(y => y.First())
                .Select(y => y.Department)
                .AsEnumerable()
                .Select(x => new DisplayItem { Text = String.Format("{0} | {1}", x.DepartmentCode, x.DepartmentName), Value = x.Id.ToString() });
            return items;
        }

        public List<SelectListItem> GetUserManagementDepartmentListItem(Guid userId)
        {
            var response = new List<SelectListItem>();
            var query = this.UnitOfWork.DepartmentUserRepo.GetAll().Where(x => x.User.Id.Equals(userId) && (x.Role == DEPARTMENT_ROLE.MANAGER || x.Role == DEPARTMENT_ROLE.OWNER || x.Role == DEPARTMENT_ROLE.WATCHER));

            response = query.Select(x => new SelectListItem()
            {
                Text = String.Format("{0} | {1}", x.Department.DepartmentCode, x.Department.DepartmentName),
                Value = x.DepartmentId.ToString()
            }).ToList();

            return response;
        }
        public IEnumerable<DisplayItem> GetDepartmentUser(Guid userId)
        {
            var items = this.UnitOfWork.DepartmentUserRepo.GetAll()
                .Where(x => x.UserId.Equals(userId))
                .Select(x => new DisplayItem { Text = x.Department.DepartmentName, Value = x.DepartmentId.ToString() }).ToList();
            return items;
        }

        public bool isManagerDepartment(Guid userId, Guid departmentId)
        {
            var items = this.UnitOfWork.DepartmentUserRepo.GetAll().Where(x => x.UserId.Equals(userId) && x.DepartmentId == departmentId && x.Role == DEPARTMENT_ROLE.MANAGER);
            if (items.Count() > 0)
                return true;
            else
                return false;
        }
        public IEnumerable<DepartmentViewModel> SearchDepartment()
        {
            var entities = this.UnitOfWork.DepartmentRepo.Search().ToList();

            List<DepartmentViewModel> models = new List<DepartmentViewModel>();

            foreach (Department entity in entities)
            {
                var item = new DepartmentViewModel()
                {
                    Id = entity.Id,
                    DepartmentCode = entity.DepartmentCode,
                    DepartmentName = entity.DepartmentName,
                    IsActive = entity.IsActive,
                    ManagerId = entity.ManagerId,
                };

                if (entity.Manager != null)
                {
                    item.Manager = new UserProfileModel() { Id = entity.Manager.Id, FirstName = entity.Manager.FirstName, LastName = entity.Manager.LastName, Email = entity.Manager.Email, PhoneNumber = entity.Manager.PhoneNumber, UserName = entity.Manager.UserName };
                }

                models.Add(item);
            }

            return models;
        }
        public DepartmentSearchViewModel SearchDepartmentWithPaging(string searchText, int pageIndex)
        {
            var entities = this.UnitOfWork.DepartmentRepo.GetAll().ToList().Where(x =>
                            (String.IsNullOrEmpty(searchText) || (x.DepartmentCode != null && x.DepartmentCode.ToLower().Contains(searchText.ToLower())
                                || x.DepartmentName != null && x.DepartmentName.ToLower().Contains(searchText.ToLower())
                                || x.Manager != null && x.Manager.FirstName != null && x.Manager.FirstName.ToLower().Contains(searchText.ToLower())
                                || x.Manager != null && x.Manager.LastName != null && x.Manager.LastName.ToLower().Contains(searchText.ToLower()))
                                || x.Manager != null && !string.IsNullOrEmpty(x.Manager.FirstName) && !string.IsNullOrEmpty(x.Manager.LastName) && ($"{x.Manager.FirstName.ToLower()} {x.Manager.LastName.ToLower()}").Contains(searchText.ToLower())
                            )).OrderBy(x => x.DepartmentName).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            int totalItem = this.UnitOfWork.DepartmentRepo.GetAll().Where(x =>
                            (String.IsNullOrEmpty(searchText) || (x.DepartmentCode != null && x.DepartmentCode.ToLower().Contains(searchText.ToLower())
                                || x.DepartmentName != null && x.DepartmentName.ToLower().Contains(searchText.ToLower())
                                || x.Manager != null && x.Manager.FirstName != null && x.Manager.FirstName.ToLower().Contains(searchText.ToLower())
                                || x.Manager != null && x.Manager.LastName != null && x.Manager.LastName.ToLower().Contains(searchText.ToLower()))
                                )).Count();

            List<DepartmentViewModel> models = new List<DepartmentViewModel>();

            foreach (Department entity in entities)
            {
                var item = new DepartmentViewModel()
                {
                    Id = entity.Id,
                    DepartmentCode = entity.DepartmentCode,
                    DepartmentName = entity.DepartmentName,
                    IsActive = entity.IsActive,
                    ManagerId = entity.ManagerId,
                };

                if (entity.Manager != null)
                {
                    item.Manager = new UserProfileModel() { Id = entity.Manager.Id, FirstName = entity.Manager.FirstName, LastName = entity.Manager.LastName, Email = entity.Manager.Email, PhoneNumber = entity.Manager.PhoneNumber, UserName = entity.Manager.UserName };
                }

                models.Add(item);
            }
            DepartmentSearchViewModel model = new DepartmentSearchViewModel()
            {
                Departments = models,
                TotalItem = totalItem
            };
            return model;
        }
        public DepartmentViewModel GetDepartment(Guid id)
        {
            var entity = this.UnitOfWork.DepartmentRepo.GetById(id);

            if (entity != null)
            {
                var model = new DepartmentViewModel()
                {
                    Id = entity.Id,
                    DepartmentCode = entity.DepartmentCode,
                    DepartmentName = entity.DepartmentName,
                    ManagerId = entity.ManagerId,
                    Note = entity.Note,
                    IsActive = entity.IsActive
                };

                if (entity.Manager != null)
                {
                    model.Manager = new UserProfileModel() { Id = entity.Manager.Id, FirstName = entity.Manager.FirstName, LastName = entity.Manager.LastName, Email = entity.Manager.Email, PhoneNumber = entity.Manager.PhoneNumber, UserName = entity.Manager.UserName };
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
        public DepartmentViewModel GetDepartmentFilter(Guid id, DateTime StartDate, DateTime EndDate)
        {
            var entity = this.UnitOfWork.DepartmentRepo.GetById(id);

            if (entity != null)
            {
                var model = new DepartmentViewModel()
                {
                    Id = entity.Id,
                    DepartmentCode = entity.DepartmentCode,
                    DepartmentName = entity.DepartmentName,
                    ManagerId = entity.ManagerId,
                    Note = entity.Note,
                    IsActive = entity.IsActive
                };

                if (entity.Manager != null)
                {
                    model.Manager = new UserProfileModel() { Id = entity.Manager.Id, FirstName = entity.Manager.FirstName, LastName = entity.Manager.LastName, Email = entity.Manager.Email, PhoneNumber = entity.Manager.PhoneNumber, UserName = entity.Manager.UserName };
                };

                var entities = entity.Users
                    .Where(x => ((x.StartDate > StartDate && x.EndDate < EndDate)
                        || (x.StartDate > StartDate && x.StartDate < EndDate)
                        || (x.StartDate < StartDate && x.StartDate > EndDate)
                        || (x.StartDate < StartDate && x.EndDate > EndDate)
                        || (x.StartDate == StartDate)
                        || (x.StartDate == EndDate)
                        || (x.EndDate == StartDate)
                        || (x.EndDate == EndDate)
                        || (x.StartDate < EndDate && x.EndDate == null))
                    )
                    .GroupBy(x => x.UserId)
                    .Select(y => y.First());

                foreach (DepartmentUser item in entities.OrderBy(x => x.User.FirstName))
                {
                    var m = new UserProfileModel()
                    {
                        Id = item.UserId,
                        FirstName = item.User.FirstName,
                        LastName = item.User.LastName,
                        Email = item.User.Email,
                        Title = item.User.Title,
                        PhotoUrl = item.User.PhotoUrl,
                        PhoneNumber = item.User.PhoneNumber,
                        UserName = item.User.UserName,
                        Tag = item.Role.ToName(),
                    };

                    model.Users.Add(m);
                }

                return model;
            }

            return null;
        }
        public ResponseViewModel<List<UserProfileModel>> GetDepartmentWithPaging(Guid departmentId, string searchText, int pageIndex)
        {
            var depart = this.UnitOfWork.DepartmentRepo.GetAll().Where(x => x.Id == departmentId).Select(i => new { i.Id, i.DepartmentCode, i.DepartmentName }).First();
            var title = "";
            if (depart != null)
                title = String.Format("{0} | {1}", depart.DepartmentCode, depart.DepartmentName);

            var entity = this.UnitOfWork.DepartmentRepo.GetById(departmentId);
            //var entity = this.UnitOfWork.DepartmentUserRepo.GetById(departmentId);


            if (entity != null)
            {
                var entities = entity.Users.Where(x => (String.IsNullOrEmpty(searchText)
                            || (x.User.FirstName != null && x.User.FirstName.ToLower().Contains(searchText.ToLower())
                            || x.User.LastName != null && x.User.LastName.ToLower().Contains(searchText.ToLower()))
                        )).OrderBy(x => x.User.FirstName).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => x.User);

                int totalItem = entity.Users.Where(x => (String.IsNullOrEmpty(searchText)
                            || (x.User.FirstName != null && x.User.FirstName.ToLower().Contains(searchText.ToLower())
                            || x.User.LastName != null && x.User.LastName.ToLower().Contains(searchText.ToLower()))
                        )).Count();

                IEnumerable<UserProfileModel> users = entities.Where(x => x != null).Select(x => new UserProfileModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                });

                ResponseViewModel<List<UserProfileModel>> model = new ResponseViewModel<List<UserProfileModel>>()
                {
                    Data = users.ToList(),
                    Total = totalItem,
                    Title = title
                };
                return model;
            }
            return null;
        }

        public ResponseViewModel<List<UserProfileModel>> GetDepartmentWithPagingFilter(Guid departmentId, string searchText, int pageIndex, DateTime StartDate, DateTime EndDate)
        {
            var depart = this.UnitOfWork.DepartmentRepo.GetAll().AsEnumerable().Where(x => x.Id == departmentId).Select(i => new { i.Id, i.DepartmentCode, i.DepartmentName }).First();
            var title = "";
            if (depart != null)
                title = String.Format("{0} | {1}", depart.DepartmentCode, depart.DepartmentName);

            
            var entity = UnitOfWork.DepartmentUserRepo.GetAll().ToList().Where(x => x.DepartmentId.Equals(departmentId) &&
                    ((x.StartDate > StartDate && x.EndDate < EndDate)
                        || (x.StartDate > StartDate && x.StartDate < EndDate)
                        || (x.StartDate < StartDate && x.StartDate > EndDate)
                        || (x.StartDate < StartDate && x.EndDate > EndDate)
                        || (x.StartDate == StartDate)
                        || (x.StartDate == EndDate)
                        || (x.EndDate == StartDate)
                        || (x.EndDate == EndDate)
                        || (x.StartDate < EndDate && x.EndDate == null)
                    )).Select(x => x.User);
            if (entity != null)
            {
                var entities = entity.Where(x => (String.IsNullOrEmpty(searchText)
                            || (x.FirstName != null && x.FirstName.ToLower().Contains(searchText.ToLower())
                            || x.LastName != null && x.LastName.ToLower().Contains(searchText.ToLower()))
                        )).OrderBy(x => x.FirstName).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                int totalItem = entity.Where(x => (String.IsNullOrEmpty(searchText)
                            || (x.FirstName != null && x.FirstName.ToLower().Contains(searchText.ToLower())
                            || x.LastName != null && x.LastName.ToLower().Contains(searchText.ToLower()))
                        )).Count();

                IEnumerable<UserProfileModel> users = entities.Where(x => x != null).Select(x => new UserProfileModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                });

                ResponseViewModel<List<UserProfileModel>> model = new ResponseViewModel<List<UserProfileModel>>()
                {
                    Data = users.ToList(),
                    Total = totalItem,
                    Title = title
                };
                return model;
            }
            return null;
        }

        public void UpdateDepartment(DepartmentViewModel model)
        {
            if (model == null || model.Id == Guid.Empty)
                throw new NullReferenceException(nameof(model));

            var entity = this.UnitOfWork.DepartmentRepo.GetById(model.Id);
            if (!entity.DepartmentCode.Equals(model.DepartmentCode))
            {
                var item = this.UnitOfWork.DepartmentRepo.GetAll().Where(x => x.Id != entity.Id && x.DepartmentCode.Equals(model.DepartmentCode)).FirstOrDefault();
                if (item != null)
                {
                    throw new Exception(Resource.CodeHasBeenUsed);
                }
            }
            entity.DepartmentCode = model.DepartmentCode;
            entity.DepartmentName = model.DepartmentName;
            entity.ManagerId = model.ManagerId;
            entity.Note = model.Note;

            this.UnitOfWork.DepartmentRepo.Update(entity, x => x.DepartmentCode, x => x.DepartmentName, x => x.ManagerId, x => x.Note);
            this.UnitOfWork.SaveChanges();
        }

        public Guid CreateDepartment(DepartmentViewModel model)
        {
            var item = this.UnitOfWork.DepartmentRepo.GetAll().Where(x => x.DepartmentCode.Equals(model.DepartmentCode)).FirstOrDefault();
            if (item != null)
            {
                throw new Exception(Resource.CodeHasBeenUsed);
            }
            var entity = new Department
            {
                Id = Guid.NewGuid(),
                DepartmentCode = model.DepartmentCode,
                DepartmentName = model.DepartmentName,
                ManagerId = model.ManagerId,
                Note = model.Note,
                IsActive = true,
            };

            this.UnitOfWork.DepartmentRepo.Insert(entity);
            this.UnitOfWork.SaveChanges();

            return entity.Id;
        }

        public Guid AddDepartmentUser(Guid departmentId, Guid userId, DEPARTMENT_ROLE role, DateTime StartDate, DateTime? EndDate)
        {            
            var entity = new DepartmentUser
            {
                Id = Guid.NewGuid(),
                DepartmentId = departmentId,
                UserId = userId,
                Role = role,
                //StartDate = new DateTime(Now.Year, Now.Month, Now.Day),
                EndDate = EndDate,
                StartDate = StartDate
            };

            this.UnitOfWork.DepartmentUserRepo.Insert(entity);
            this.UnitOfWork.SaveChanges();

            return entity.Id;
        }

        public bool CheckExistAddDepartmentUser(Guid departmentId,Guid userId)
        {
            return this.UnitOfWork.DepartmentUserRepo.CheckExistAddDepartmentUser(departmentId, userId);
        }

        public DepartmentUserEditModel GetDepartmentUserDetail(Guid id)
        {
            var entity = this.UnitOfWork.DepartmentUserRepo.GetById(id);
            DepartmentUserEditModel model = new DepartmentUserEditModel()
            {
                Id = entity.Id,
                UserId = entity.UserId,
                DepartmentId = entity.DepartmentId,
                Role = entity.Role,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };
            return model;
        }

        public void UpdateDepartmentUser(DepartmentUserEditModel model)
        {
            var item = this.UnitOfWork.DepartmentUserRepo.GetAll()
                //.Where(x => x.DepartmentId == model.DepartmentId && x.UserId.Equals(model.UserId)).FirstOrDefault();
                .Where(x => x.Id == model.Id).FirstOrDefault();
            
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            item.Role = model.Role;

            this.UnitOfWork.DepartmentUserRepo.Update(item);
            this.UnitOfWork.SaveChanges();
        }
        public void DeleteDepartmentUser(Guid departmentId, Guid DepartmentUserId)
        {
            //var entities = this.UnitOfWork.DepartmentUserRepo.GetAll().Where(x => x.DepartmentId == departmentId && x.UserId.Equals(userId)).Select(x => x.Id).ToList();

            //this.UnitOfWork.DepartmentUserRepo.DeleteMulti(entities);
            this.UnitOfWork.DepartmentUserRepo.Delete(DepartmentUserId);
            this.UnitOfWork.SaveChanges();
        }
        public void DeleteDerpartment(Guid id)
        {
            this.UnitOfWork.DepartmentRepo.Delete(id);
            this.UnitOfWork.SaveChanges();
        }

        public IEnumerable<DisplayItem> GetNotSelectedDepartmentUser(Guid departId)
        {
            List<DisplayItem> items = new List<DisplayItem>();

            List<DepartmentUser> departUser = new List<DepartmentUser>();

            var depart = this.UnitOfWork.DepartmentRepo.GetById(departId);

            if (depart != null)
            {
                departUser = depart.Users.ToList();
            }
            if (departUser.Count > 0)
            {
                var model = this.UnitOfWork.UserRepo.GetAll().AsEnumerable().Where(x => x.IsActive && (!departUser.Select(y => y.UserId).Contains(x.Id))).OrderBy(x => x.FirstName);
                items = model.Select(x => new DisplayItem { Text = String.Format("{1} {2} | {0}", x.UserName, x.FirstName, x.LastName), Value = x.Id.ToString() }).ToList();
            }
            else
            {
                items = this.UnitOfWork.UserRepo.GetAll().Where(x => x.IsActive).OrderBy(x => x.FirstName).AsEnumerable().Select(x => new DisplayItem { Text = String.Format("{1} {2} | {0}", x.UserName, x.FirstName, x.LastName), Value = x.Id.ToString() }).ToList();
            }
            return items;
        }
        public List<DepartmentUser> GetAllDepartmentManagers(Guid userId)
        {
            List<DepartmentUser> departmentUsers = new List<DepartmentUser>();
            var departIds = this.UnitOfWork.DepartmentRepo.GetAll().Where(x => x.Users.Select(y => y.UserId).Contains(userId)).Select(x => x.Id).ToList();
            foreach (var departId in departIds)
            {
                var departments = this.UnitOfWork.DepartmentUserRepo.GetAll().Where(x => x.DepartmentId == departId && x.Role == DEPARTMENT_ROLE.MANAGER
                                && !departmentUsers.Select(y => y.UserId).Contains(x.UserId)).ToList();
                departmentUsers.AddRange(departments);
            }
            return departmentUsers;
        }

        public IEnumerable<DisplayItem> GetActiveDepartMents()
        {
            var items = this.UnitOfWork.DepartmentRepo.GetAll().Where(x => x.IsActive).AsEnumerable().Select(x => new DisplayItem { Text = String.Format("{0} | {1}", x.DepartmentCode, x.DepartmentName), Value = x.Id.ToString() });
            return items;
        }

        #endregion
    }
}