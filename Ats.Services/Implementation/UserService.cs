using Ats.Commons;
using Ats.Domain;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Address.Models;
using Ats.Domain.Identity.Models;
using Ats.Models;
using Ats.Models.User;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class UserService : BaseService, IUserService
    {
        private IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        private int pageSize;

        public UserService(IUnitOfWork unitOfWork, IConfiguration config, UserManager<User> userManager,
            IHostingEnvironment environment, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _userManager = userManager;
            _config = config;
            _hostingEnvironment = environment;
            pageSize = _config.GetValue<int>("PageSize");
        }

        #region User
        public List<UserrViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Users service Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.UserRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText)
                                || (x.UserName != null && x.UserName.ToLower().Contains(searchText.ToLower()))
                                || (x.FirstName != null && x.FirstName.ToLower().Contains(searchText.ToLower()))
                                || (x.LastName != null && x.LastName.ToLower().Contains(searchText.ToLower()))
                                || (x.Email != null && x.Email.ToLower().Contains(searchText.ToLower()))
                                || (x.PhoneNumber != null && x.PhoneNumber.Contains(searchText))
                            );
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
                    case "username":
                        query = IsAscOrder ? query.OrderBy(x => x.UserName) : query.OrderByDescending(x => x.UserName);
                        break;
                    case "fullname":
                        query = IsAscOrder ? query.OrderBy(x => x.FirstName.ToLower()).ThenBy( y => y.LastName) : query.OrderByDescending(x => x.FirstName.ToLower()).ThenBy(y => y.LastName);
                        break;
                    case "email":
                        query = IsAscOrder ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email);
                        break;
                    case "phone":
                        query = IsAscOrder ? query.OrderBy(x => x.PhoneNumber) : query.OrderByDescending(x => x.PhoneNumber);
                        break;
                }
            }

            var data = query.Select(x => new UserrViewModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.FirstName + " " + x.LastName,
                Email = x.Email,
                Phone = x.PhoneNumber,
                IsActive = x.IsActive
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public UserrProfileViewModel GetUser(Guid id)
        {
            _logger.LogDebug($"User Detail service (Id: {id})");
            var entity = UnitOfWork.UserRepo.GetById(id);
            if (entity != null)
            {
                var roleGroup = this.UnitOfWork.UserRepo.GetRoleGroup(id);
                UserrProfileViewModel model = new UserrProfileViewModel();
                model.Id = entity.Id;
                model.UserName = entity.UserName;
                model.FirstName = entity.FirstName;
                model.LastName = entity.LastName;
                model.Email = entity.Email;
                model.Phone = entity.PhoneNumber;
                model.DOB = entity.DOB;
                model.Address = entity.Address;
                model.IsActive = entity.IsActive;
                if (roleGroup != null)
                {
                    model.GroupId = roleGroup.Id;
                    model.GroupName = roleGroup.Name;
                }
                return model;
            }
            return null;
        }
        public bool CheckExistUserNameOrEmail(UserrViewModel model)
        {
            var userList = UnitOfWork.UserRepo.GetAll();
            if (userList.Any(x => x.UserName.ToLower().Equals(model.UserName.ToLower()))) return true;
            if (model.Email != null) if (userList.Any(x => x.Email.ToLower().Equals(model.Email.ToLower()))) return true;
            return false;
        }
        public void CreateUser(UserrViewModel model)
        {
            _logger.LogDebug($"Create User service (Username: {model.UserName})");
            try
            {
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    //Email = model.Email,
                    Email = model.UserName,
                    PhoneNumber = model.Phone,
                    IsActive = model.IsActive,
                    LockoutEnabled = model.LockoutEnabled == Active.Yes,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true
                };
                var password = Ultility.CreatePassword(10);
                var identityResult = _userManager.CreateAsync(user, password).Result;
                if (identityResult.Succeeded)
                {
                    var userProfile = UnitOfWork.UserRepo.GetById(user.Id);
                    userProfile.UserGroups = userProfile.UserGroups ?? new List<UserGroup>();
                    userProfile.UserGroups.Add(new UserGroup()
                    {
                        GroupId = model.GroupId.Value,
                        UserId = user.Id,
                    });
                    userProfile.UserRoles = new List<UserRole>();
                    var roles = UnitOfWork.RoleRepo.GetAll().ToList().Where(x => x.GroupRoles.ToList().Any(y => y.GroupId == model.GroupId));
                    foreach (var i in roles)
                    {
                        userProfile.UserRoles.Add(new UserRole()
                        {
                            RoleId = i.Id,
                            UserId = user.Id
                        });
                    }
                    UnitOfWork.UserRepo.Update(userProfile);

                    #region Profile 
                    Address address = UnitOfWork.AddressRepo.CreateAddress("", null, null);
                    Profile profile = new Profile()
                    {
                        Id = Guid.NewGuid(),
                        Email = userProfile.Email,
                        Phone = userProfile.PhoneNumber,
                        FirstName = userProfile.FirstName,
                        LastName = userProfile.LastName,
                        AddressId = address.Id,
                        Address = address
                    };
                    #endregion

                    #region Wallet
                    Wallet wallet = new Wallet()
                    {
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.UtcNow,
                        LastUpdate = DateTime.UtcNow,
                        UserId = user.Id,
                    };
                    #endregion

                    UnitOfWork.BeginTransaction();       
                    UnitOfWork.ProfileRepo.Insert(profile);
                    UnitOfWork.WalletRepo.Insert(wallet);          
                    UnitOfWork.SaveChanges();
                    UnitOfWork.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }     
        }
        public void UpdateUser(UserrProfileViewModel model)
        {
            _logger.LogDebug($"Edit User service (Id: {model.Id})");
            var user = UnitOfWork.UserRepo.GetById(model.Id);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                //user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.DOB = model.DOB;

                user.Address = model.Address;
                user.IsActive = model.IsActive;
                UnitOfWork.UserRepo.Update(user);

                #region User group
                if ((user.UserGroups == null || user.UserGroups.Count == 0 || (!user.UserGroups.ToList().Any(x => x.GroupId.Equals(model.GroupId)))) && model.GroupId != Guid.Empty)
                {
                    user.UserGroups = new List<UserGroup>();
                    user.UserGroups.Add(new UserGroup()
                    {
                        GroupId = model.GroupId.Value,
                        UserId = user.Id
                    });
                    var roles = UnitOfWork.RoleRepo.GetAll().ToList().Where(x => x.GroupRoles.ToList().Any(y => y.GroupId.Equals(model.GroupId)));
                    var userRoleRepo = UnitOfWork.UserRepo.GetUserRoleRepo();
                    userRoleRepo.RemoveRange(userRoleRepo.ToList().Where(x => x.UserId == model.Id));
                    var newUserRole = new List<UserRole>();
                    foreach (var r in roles)
                    {
                        newUserRole.Add(new UserRole()
                        {
                            RoleId = r.Id,
                            UserId = user.Id
                        });
                    }
                    userRoleRepo.AddRange(newUserRole);
                }
                #endregion                

                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteUser(Guid id)
        {
            _logger.LogDebug($"Delete User service (Id: {id})");
            var entity = UnitOfWork.UserRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.UserRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Role group
        public List<RoleGrouppViewModel> SearchRoleGroup(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Get all Role groups service Search={searchText}, Page={page}");
            var query = UnitOfWork.GroupRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                }
            }

            var data = query.Select(x => new RoleGrouppViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public RoleGrouppViewModel GetRoleGroup(Guid id)
        {
            _logger.LogDebug($"Role group Detail service (Id: {id})");
            var entity = UnitOfWork.GroupRepo.GetById(id);
            if (entity != null)
            {
                var model = new RoleGrouppViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                };

                return model;
            }
            return null;
        }

        public void CreateRoleGroup(RoleGrouppViewModel model)
        {
            _logger.LogDebug($"Create Role group service (Name: {model.Name})");
            var entity = new Group
            {
                Id = model.Id,
                Name = model.Name,
            };

            UnitOfWork.GroupRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteRoleGroup(Guid id)
        {
            _logger.LogDebug($"Delete Role group service (Id: {id})");
            var entity = UnitOfWork.GroupRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.GroupRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion
    }
}
