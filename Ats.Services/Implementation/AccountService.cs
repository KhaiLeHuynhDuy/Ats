using System.Collections.Generic;
using Ats.Domain;
using Ats.Commons;
using System;
using System.Linq;
using System.IO;
using Ats.Models;
using Ats.Models.User;
using Ats.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ats.Domain.Identity.Models;
using Ats.Domain.Accounts.Models;
using Ats.Commons.Models;
using System.Threading.Tasks;
using Ats.Commons.Resource;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hostingEnvironment;
        public AccountService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager,
                             IHostingEnvironment environment, IConfiguration config) : base(unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = environment;
            this._config = config;
        }

        public IEnumerable<DisplayItem> GetActiveUsers()
        {
            var items = this.UnitOfWork.UserRepo.GetAll().Where(x => x.IsActive).OrderBy(x => x.FirstName).AsEnumerable().Select(x => new DisplayItem { Text = String.Format("{1} {2} | {0}", x.UserName, x.FirstName, x.LastName), Value = x.Id.ToString() });
            return items;
        }

        public IEnumerable<UserProfileModel> Search()
        {
            var users = this.UnitOfWork.UserRepo.Search();

            List<UserProfileModel> userModels = new List<UserProfileModel>();

            foreach (var user in users)
            {
                userModels.Add(new UserProfileModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });
            }

            return userModels;
        }
        public UserSearchViewModel SearchWithPaging(string searchText, int pageIndex)
        {
            int pageSize = _config.GetValue<int>("PageSize");

            var users = this.UnitOfWork.UserRepo.GetAll().AsEnumerable().Where(x =>
                            (String.IsNullOrEmpty(searchText) || (x.FirstName != null && x.FirstName.ToLower().Contains(searchText.ToLower())
                                || x.LastName != null && x.LastName.ToLower().Contains(searchText.ToLower())
                                || x.Email != null && x.Email.ToLower().Contains(searchText.ToLower()))
                                || x.UserName != null && x.UserName.ToLower().Contains(searchText.ToLower())
                                || x.PhoneNumber != null && x.PhoneNumber.Contains(searchText)
                                || x.Title != null && x.Title.ToLower().Contains(searchText.ToLower())
                                || !string.IsNullOrEmpty(x.FirstName) && !string.IsNullOrEmpty(x.FirstName) && ($"{x.FirstName} {x.LastName}").Contains(searchText.ToLower())
                            ))
                            .OrderBy(x => x.FirstName).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            int totalItem = this.UnitOfWork.UserRepo.GetAll().Where(x =>
                            (String.IsNullOrEmpty(searchText) || (x.FirstName != null && x.FirstName.ToLower().Contains(searchText.ToLower())
                                || x.LastName != null && x.LastName.ToLower().Contains(searchText.ToLower())
                                || x.Email != null && x.Email.ToLower().Contains(searchText.ToLower()))
                            )).Count();

            List<UserProfileModel> userModels = new List<UserProfileModel>();

            foreach (var user in users.ToList())
            {
                userModels.Add(new UserProfileModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Title = user.Title,
                    PhotoUrl = user.PhotoUrl,
                });
            }
            UserSearchViewModel model = new UserSearchViewModel()
            {
                UsersProfile = userModels,
                TotalItem = totalItem
            };
            return model;
        }
        
        public IQueryable<User> GetAllUsers()
        {
            var items = this.UnitOfWork.UserRepo.GetAll();
            return items;
        }

        public UserProfileModel GetUser(Guid id)
        {
            var user = this.UnitOfWork.UserRepo.GetById(id);

            if (user != null)
            {
                var roleGroup = this.UnitOfWork.UserRepo.GetRoleGroup(id);

                var model = new UserProfileModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    LockoutEnabled = user.LockoutEnabled == true ? Active.Yes : Active.No,
                    IsActive = user.IsActive,
                    PhotoUrl = user.PhotoUrl,
                    Title = user.Title,
                };

                if (roleGroup != null)
                {
                    model.GroupId = roleGroup.Id;
                    model.GroupName = roleGroup.Name;
                }

                return model;
            }

            return null;
        }
        public string EditUserProfile(UserProfileModel model)
        {
            var user = this.UnitOfWork.UserRepo.GetById(model.Id);

            var filePath = BuildAttachmentLink(model);
            if (!string.IsNullOrEmpty(filePath))
            {
                model.PhotoUrl = filePath;
            }

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Title = model.Title;
                user.Email = model.Email;
                if (model.File != null)
                {
                    user.PhotoUrl = filePath;
                }
                else
                {
                    if (model.PhotoUrl != null)
                    {
                        user.PhotoUrl = model.PhotoUrl;
                    }
                    else
                    {
                        user.PhotoUrl = null;
                    }
                }

                this.UnitOfWork.UserRepo.Update(user, x => x.FirstName, x => x.LastName, x => x.PhoneNumber, x => x.PhotoUrl, x => x.Title, x => x.Email);
                this.UnitOfWork.SaveChanges();
            }
            return user.Id.ToString();

        }

        public string BuildAttachmentLink(UserProfileModel model)
        {
            string folderUpload = "uploads";
            var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, folderUpload);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            return Ultility.BuildAttachmentLink(model.File, uploadPath, folderUpload);
        }
        public async Task<BaseResponse<UserProfileModel>> CreateUser(UserProfileModel model)
        {
            var response = new BaseResponse<UserProfileModel>();

            var userList = GetAllUsers();

            var filePath = BuildAttachmentLink(model);
            if (!string.IsNullOrEmpty(filePath))
            {
                model.PhotoUrl = filePath;
            }

            if (userList.Any(x => x.UserName.ToLower().Equals(model.UserName.ToLower())))
            {
                response.Errors = new List<string> { Resource.UserNameUsed };
                return response;
            }
           
            var userExist = UnitOfWork.UserRepo.GetAll().ToList().FirstOrDefault(x => x.Email.ToLower().Equals(model.Email.ToLower()));
            if (userExist != null)
            {
                response.Errors = new List<string> { Resource.EmailUsed };
                return response;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                LockoutEnabled = model.LockoutEnabled == Active.Yes,
                IsActive = model.IsActive,
                PhotoUrl = model.PhotoUrl,
                Title = model.Title,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true
            };
            //var passwordHash = new PasswordHasher<User>();
            //newUser.PasswordHash = passwordHash.HashPassword(newUser, Ultility.CreatePassword(8));
            var password = Ultility.CreatePassword(10);

            var identityResult = _userManager.CreateAsync(user, password).Result;
            if (identityResult.Succeeded)
            {
                var userProfile = this.UnitOfWork.UserRepo.GetById(user.Id);
                userProfile.UserGroups = userProfile.UserGroups ?? new List<UserGroup>();
                userProfile.UserGroups.Add(new UserGroup()
                {
                    GroupId = model.GroupId,
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
                await UnitOfWork.SaveChangesAsync();

                response.Success = true;
                model.Id = user.Id;
                response.Data = model;
                return response;
            }

            response.Errors = identityResult.Errors.Select(x => x.Description).ToList();
            return response;
        }

        public async Task<BaseResponse<UserProfileModel>> UpdateUserProfile(UserProfileModel model)
        {
            var response = new BaseResponse<UserProfileModel>();
            var user = this.UnitOfWork.UserRepo.GetById(model.Id);
            if (user == null)
            {
                response.Errors = new List<string> { Resource.UserNotFound };
                return response;
            }
            var userList = this.GetAllUsers();
            if (userList.Any(x => x.UserName.ToLower().Equals(model.UserName.ToLower()) && !model.Id.Equals(x.Id)))
            {
                response.Errors = new List<string> { Resource.UserNameUsed };
                return response;
            }
            if (userList.Any(x => x.Email.ToLower().Equals(model.Email.ToLower()) && !model.Id.Equals(x.Id)))
            {
                response.Errors = new List<string> { Resource.EmailUsed };
                return response;
            }            

            var filePath = BuildAttachmentLink(model);
            if (!string.IsNullOrEmpty(filePath))
            {
                model.PhotoUrl = filePath;
                user.PhotoUrl = filePath;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.LockoutEnabled = model.LockoutEnabled == Active.Yes ? true : false;
            user.IsActive = model.IsActive;
            user.Title = model.Title;
            user.Email = model.Email;
            user.UserName = model.UserName;

            this.UnitOfWork.UserRepo.Update(user);

            if ((user.UserGroups == null || user.UserGroups.Count == 0
                || (!user.UserGroups.ToList().Any(x => x.GroupId.Equals(model.GroupId)))) && model.GroupId != Guid.Empty)
            {
                user.UserGroups = new List<UserGroup>();
                user.UserGroups.Add(new UserGroup()
                {
                    GroupId = model.GroupId,
                    UserId = user.Id
                });
                var roles = UnitOfWork.RoleRepo.GetAll().ToList().Where(x => x.GroupRoles.ToList().Any(y => y.GroupId.Equals(model.GroupId)));
                var userRoleRepo = UnitOfWork.UserRepo.GetUserRoleRepo();
                userRoleRepo.RemoveRange(userRoleRepo.ToList().Where(x=>x.UserId == model.Id));
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

            this.UnitOfWork.SaveChanges();

            response.Success = true;
            response.Data = model;

            return response;
        }

        public IEnumerable<GroupViewModel> GetAllRoleGroups()
        {
            var roles = this.UnitOfWork.GroupRepo.GetAll();

            List<GroupViewModel> roleGroups = new List<GroupViewModel>();

            foreach (Group role in roles)
            {
                roleGroups.Add(new GroupViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                });
            }

            return roleGroups;
        }

        public GroupViewModel GetRoleGroup(Guid groupId)
        {
            var enity = this.UnitOfWork.GroupRepo.GetById(groupId);
            var roleList = this.UnitOfWork.GroupRepo.GetRolesByGroup(groupId);

            GroupViewModel model = new GroupViewModel()
            {
                Id = enity.Id,
                Name = enity.Name,
                Description = enity.Description,
                Roles = roleList.Select(x => new RoleViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                ListRoles = GetRoles(roleList.Select(x => x.Id).ToList())

            };

            return model;
        }

        public ICollection<DisplayItem> GetRoles(List<Guid> selectedIds)
        {
            var roles = this.UnitOfWork.RoleRepo.GetAll();
            var listRoles = new List<DisplayItem>();
            foreach (Role role in roles)
            {
                DisplayItem item = new DisplayItem()
                {
                    Text = role.Name,
                    Value = role.Name,
                    Selected = false
                };

                if (selectedIds.Any(x => x == role.Id))
                {
                    item.Selected = true;
                }

                listRoles.Add(item);
            }

            return listRoles;
        }
        public RoleGroupSearchViewModel GetRoleGroupWithPaging(string searchText, int pageIndex)
        {
            int pageSize = _config.GetValue<int>("PageSize");

            var roles = this.UnitOfWork.GroupRepo.GetAll().Where(x =>
                            (String.IsNullOrEmpty(searchText) || (x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()))
                            || (x.Description != null && x.Description.ToLower().Contains(searchText.ToLower()))
                            )).OrderBy(x => x.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var totalItem = this.UnitOfWork.GroupRepo.GetAll().Where(x =>
                            (String.IsNullOrEmpty(searchText) || (x.Name != null && x.Name.ToLower().Contains(searchText.ToLower()))
                            )).Count();

            List<GroupViewModel> roleGroups = new List<GroupViewModel>();
            foreach (Group role in roles)
            {
                roleGroups.Add(new GroupViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                });
            }
            var model = new RoleGroupSearchViewModel()
            {
                RoleGroups = roleGroups,
                TotalItem = totalItem
            };
            return model;
        }
        public async Task<BaseResponse<GroupViewModel>> UpdateRoleGroup(GroupViewModel model)
        {
            var response = new BaseResponse<GroupViewModel>();
            var roleGroups = this.GetAllRoleGroups();
            //if(roleGroups.Any(x=> x.Id != model.Id && x.Name.ToLower().Contains(model.Name.ToLower())))
            //{
            //    response.Errors = new List<string> { Resource.UserNameUsed };
            //    return response;
            //}
            this.UnitOfWork.GroupRepo.UpdateGroup(model.Id, model.Name, model.Description, model.SelectedRoles);
            this.UnitOfWork.UserRepo.UpdateRolesVer2(model.Id, model.SelectedRoles);
            //this.UnitOfWork.UserRepo.UpdateRoles(model.Id);

            this.UnitOfWork.SaveChanges();
            response.Success = true;
            response.Data = model;

            return response;
        }

        public List<UserRolesNameViewModel> GetUserRoles(Guid userId)
        {
            return this.UnitOfWork.UserRepo.GetUserRoles(userId).ToList();
        }

        public float GetTargetHours(Guid userId, DateTime month)
        {
            var xxx = this.UnitOfWork.DepartmentRepo.GetById(Guid.NewGuid());

            float targetHours = 0;            

            return targetHours;
        }
        public void DeleteUser(Guid id)
        {
            this.UnitOfWork.UserRepo.Delete(id);
            this.UnitOfWork.SaveChanges();
        }
        public void DeleteGroup(Guid id)
        {
            this.UnitOfWork.GroupRepo.Delete(id);
            this.UnitOfWork.SaveChanges();
        }
        public async Task<BaseResponse<GroupViewModel>> CreateRoleGroup(GroupViewModel model)
        {
            var response = new BaseResponse<GroupViewModel>();
            var roleGroups = this.GetAllRoleGroups();
            if (roleGroups.Any(x => x.Name.ToLower().Contains(model.Name.ToLower())))
            {
                response.Errors = new List<string> { Resource.UserNameUsed };
                return response;
            }

            Group rg = new Group()
            {
                Id = Guid.NewGuid(),
                Description = model.Description,
                Name = model.Name
            };
            this.UnitOfWork.GroupRepo.Insert(rg);
            this.UnitOfWork.SaveChanges();

            model.Id = rg.Id;
            response.Success = true;
            response.Data = model;

            return response;
        }        
    }
}
