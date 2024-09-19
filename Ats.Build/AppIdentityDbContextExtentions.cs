using Ats.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ats.Commons;
using System.Linq;
using Ats.Domain.Identity.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Address;
using Ats.Domain.Lead.Models;
using Ats.Domain.Account.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Ats.Domain.Member.Models;
using Ats.Domain.Store.Models;
using Ats.Domain.Loyalty.Models;
using Ats.Domain;
using Ats.Domain.Channel.Models;
using Ats.Domain.Claims;
using Ats.Domain.Claims.Models;

namespace Ats.Build
{
    public static class AppIdentityDbContextExtentions
    {
        public static void EnsureSeedDataForContext(this AppIdentityDbContext context)
        {
            Seedup1(context);
            Seedup2(context);
            Seedup3(context);
            Seedup4(context);
            Seedup5(context);
            Seedup6(context);
            Seedup7(context);
            Seedup8(context);
            Seedup9(context);
            Seedup10(context);
            Seedup11(context);
            Seedup12(context);
            Seedup13(context);
            Seedup14(context);

        }

        private static void Seedup1(AppIdentityDbContext context)
        {

            #region System Roles
            List<Group> roleGroups = new List<Group>()
            {
                new Group {Id = Guid.NewGuid(), Name = Constants.SYS_USER_GROUP_MEMBER},
                new Group {Id = Guid.NewGuid(), Name = Constants.SYS_USER_GROUP_SYSTEM_ADMINISTRATORS}, 
                new Group {Id = Guid.NewGuid(), Name = Constants.SYS_USER_GROUP_MANAGERS}, 
                new Group {Id = Guid.NewGuid(), Name = Constants.SYS_USER_GROUP_MARKETING},
                new Group {Id = Guid.NewGuid(), Name = Constants.SYS_USER_GROUP_EMPLOYEE},

            };

            List<Role> appRoles = new List<Role>()
            {
                new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.SYS_ROLE_SYSTEM_ADMINISTRATION,
                    NormalizedName = Constants.SYS_ROLE_SYSTEM_ADMINISTRATION.ToUpper()
                },
                new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.SYS_ROLE_USER_MANAGEMENT,
                    NormalizedName = Constants.SYS_ROLE_USER_MANAGEMENT.ToUpper()
                },

              
           

            };
            if (context.Role.Any())
            {
                List<Role> lsRoles = new List<Role>()
                {              


                #region Member 2
                 new Role() //member
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_VIEW.ToUpper()
                },
                  new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBER_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                },


                  
                //new Role() //member tag
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBERTAG_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBERTAG_MANAGEMENT_VIEW.ToUpper()
                //} ,
                // new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBERTAG_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBERTAG_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //},

                  
                // new Role() //member group
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBERGROUP_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBERGROUP_MANAGEMENT_VIEW.ToUpper()
                //},
                //    new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBERGROUP_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBERGROUP_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //},

                     
                //new Role() //member segment
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_VIEW.ToUpper()
                //} ,
                // new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBERSEGMENT_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //},

                   
                //new Role() //member lifecycle
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_VIEW.ToUpper()
                //} ,
                // new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_MEMBERLIFECYCEL_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //},
                #endregion

                #region Loyalty 2

                   new Role() //tier rule
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_VIEW.ToUpper()
                },
                    new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_ROLE_LOYALTY_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                },

                // new Role() //tier rule
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_TIERRULE_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_TIERRULE_MANAGEMENT_VIEW.ToUpper()
                //}, 
                //    new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_TIERRULE_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_TIERRULE_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //},

                     
                //new Role() //referral
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_REFERRAL_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_REFERRAL_MANAGEMENT_VIEW.ToUpper()
                //} ,
                // new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_REFERRAL_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_REFERRAL_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //},

                  
                // new Role() //point rule
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_POINTRULE_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_POINTRULE_MANAGEMENT_VIEW.ToUpper()
                //},
                //   new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_POINTRULE_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_POINTRULE_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //},

                    
                //new Role() //point rule standard
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_VIEW.ToUpper()
                //} ,
                // new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_POINTRULESTANDARD_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //},
                 #endregion Loyalty

                #region Campaign 7
                    
                new Role() //spot campaign
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_VIEW.ToUpper()
                }, 
                new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                },
                new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_ANALYSH,
                    NormalizedName = Constants.RoleName.SYS_ROLE_SPOTCAMPAIGN_MANAGEMENT_ANALYSH.ToUpper()
                },
                
                  new Role() //coupon
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_COUPON_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_ROLE_COUPON_MANAGEMENT_VIEW.ToUpper()
                },
                    new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_COUPON_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_ROLE_COUPON_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                },
                     
                //new Role() //collection
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_COLLECTION_MANAGEMENT_VIEW,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_COLLECTION_MANAGEMENT_VIEW.ToUpper()
                //} ,
                // new Role()
                //{
                //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_COLLECTION_MANAGEMENT_CREATE_EDIT_DELETE,
                //    NormalizedName = Constants.RoleName.SYS_ROLE_COLLECTION_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                //}, 
                               
                 new Role() //order
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_ORDER_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_ROLE_ORDER_MANAGEMENT_VIEW.ToUpper()
                },
                             new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_ROLE_ORDER_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                },
                 #endregion Campaign

                #region Reports 1
                    
                 new Role() // member report
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_REPORT_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_ROLE_REPORT_MANAGEMENT_VIEW.ToUpper()
                }, 
 
                  
                 #endregion Reports

                #region Tools 1
                new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_IMPORT_MANAGEMENT_INDEX,
                    NormalizedName = Constants.RoleName.SYS_ROLE_IMPORT_MANAGEMENT_INDEX.ToUpper()
                } ,

           

                 #endregion Tools

                #region Wallets 1
                 new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_WALLETS_MANAGEMENT_INDEX,
                    NormalizedName = Constants.RoleName.SYS_ROLE_WALLETS_MANAGEMENT_INDEX.ToUpper()
                } ,
                 //new Role()
                 //{ 
                 //    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_WALLETS_MANAGEMENT_DELETE, 
                 //    NormalizedName = Constants.RoleName.SYS_ROLE_WALLETS_MANAGEMENT_DELETE.ToUpper() 
                 //},
             

                 #endregion Wallets

                #region Store 2
                new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_STORE_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_ROLE_STORE_MANAGEMENT_VIEW.ToUpper()
                } ,
                 new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_STORE_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_ROLE_STORE_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                },
                 #endregion Store

                #region User 0
              
                #endregion User

                #region category 2
                   
                new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_CATEGORY_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_CATEGORY_MANAGEMENT_VIEW.ToUpper()
                } ,
                 new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_CATEGORY_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                },
                 #endregion category
                  
                #region Product 2
                new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_PRODUCT_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_PRODUCT_MANAGEMENT_VIEW.ToUpper()
                } ,
                 new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_PRODUCT_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                },


             
                 #endregion Product
                  
                #region Organization 2
                  
                 new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_VIEW,
                    NormalizedName = Constants.RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_VIEW.ToUpper()
                }, 
                 new Role()
                {
                    Id = Guid.NewGuid(), Name = Constants.RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_CREATE_EDIT_DELETE,
                    NormalizedName = Constants.RoleName.SYS_ROLE_ORGANIZATION_MANAGEMENT_CREATE_EDIT_DELETE.ToUpper()
                }
                #endregion Organization
                };


                var grAdmin = context.Group.Where(x => x.Name.Equals(Constants.SYS_USER_GROUP_SYSTEM_ADMINISTRATORS)).FirstOrDefault().Id;

                if (grAdmin != null)
                {
                    foreach (Role ar in lsRoles)
                    {
                        if (context.Roles.Any(x => x.Name == ar.Name)) continue;
                        GroupRole gr = new GroupRole() { GroupId = grAdmin, RoleId = ar.Id };

                        context.Role.Add(ar);
                        context.GroupRole.Add(gr);

                    }
                    context.SaveChanges();
                } 
                return;
            }
            var grAdmins = roleGroups.FirstOrDefault(x => x.Name.Equals(Constants.SYS_USER_GROUP_SYSTEM_ADMINISTRATORS));

            if (grAdmins != null)
            {
                foreach (Role ar in appRoles)
                {
                    GroupRole gr = new GroupRole() { GroupId = grAdmins.Id, RoleId = ar.Id };
                    grAdmins.GroupRoles.Add(gr);
                }
                context.Role.AddRange(appRoles);
                context.Group.AddRange(roleGroups);
            }

            #endregion

            #region Root Admin
            string password = "Agn@1234579";
            User admin = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Admin",
                LastName = "System",
                UserName = "admin@aegona.com",
                Email = "admin@aegona.com",
                NormalizedEmail = "admin@aegona.com".ToUpper(),
                IsActive = true,
                EmailConfirmed = true,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                NormalizedUserName = "admin@aegona.com".ToUpper(),

            };
            var passwordHash = new PasswordHasher<User>();
            admin.PasswordHash = passwordHash.HashPassword(admin, password);

            context.User.Add(admin);

            List<UserRole> adminRoles = new List<UserRole>();
            foreach (Role ar in appRoles)
            {
                UserRole ur = new UserRole() { UserId = admin.Id, RoleId = ar.Id };
                adminRoles.Add(ur);
            }

            context.UserRole.AddRange(adminRoles);

            #endregion         

            #region Address

            var provinces = new string[]
            {
                "01|Hà Nội",
                "02|Hà Giang",
                "04|Cao Bằng",
                "06|Bắc Kạn",
                "08|Tuyên Quang",
                "10|Lào Cai",
                "11|Điện Biên",
                "12|Lai Châu",
                "14|Sơn La",
                "15|Yên Bái",
                "17|Hòa Bình",
                "19|Thái Nguyên",
                "20|Lạng Sơn",
                "22|Quảng Ninh",
                "24|Bắc Giang",
                "25|Phú Thọ",
                "26|Vĩnh Phúc",
                "27|Bắc Ninh",
                "30|Hải Dương",
                "31|Hải Phòng",
                "33|Hưng Yên",
                "34|Thái Bình",
                "35|Hà Nam",
                "36|Nam Định",
                "37|Ninh Bình",
                "38|Thanh Hóa",
                "40|Nghệ An",
                "42|Hà Tĩnh",
                "44|Quảng Bình",
                "45|Quảng Trị",
                "46|Thừa Thiên Huế",
                "48|Đà Nẵng",
                "49|Quảng Nam",
                "51|Quảng Ngãi",
                "52|Bình Định",
                "54|Phú Yên",
                "56|Khánh Hòa",
                "58|Ninh Thuận",
                "60|Bình Thuận",
                "62|Kon Tum",
                "64|Gia Lai",
                "66|Đắk Lắk",
                "67|Đắk Nông",
                "68|Lâm Đồng",
                "70|Bình Phước",
                "72|Tây Ninh",
                "74|Bình Dương",
                "75|Đồng Nai",
                "77|Bà Rịa - Vũng Tàu",
                "79|Hồ Chí Minh",
                "80|Long An",
                "82|Tiền Giang",
                "83|Bến Tre",
                "84|Trà Vinh",
                "86|Vĩnh Long",
                "87|Đồng Tháp",
                "89|An Giang",
                "91|Kiên Giang",
                "92|Cần Thơ",
                "93|Hậu Giang",
                "94|Sóc Trăng",
                "95|Bạc Liêu",
                "96|Cà Mau"
            };

            foreach (var province in provinces)
            {
                var prov = province.Split('|');

                var entity = new Province()
                {
                    Id = Int32.Parse(prov[0]),
                    ProvinceCode = prov[0],
                    Name = prov[1],
                };

                context.Province.Add(entity);
            }

            string[] districts = new string[]
            {
                "001|Quận Ba Đình|01",
                "002|Quận Hoàn Kiếm|01",
                "003|Quận Tây Hồ|01",
                "004|Quận Long Biên|01",
                "005|Quận Cầu Giấy|01",
                "006|Quận Đống Đa|01",
                "007|Quận Hai Bà Trưng|01",
                "008|Quận Hoàng Mai|01",
                "009|Quận Thanh Xuân|01",
                "016|Huyện Sóc Sơn|01",
                "017|Huyện Đông Anh|01",
                "018|Huyện Gia Lâm|01",
                "019|Quận Nam Từ Liêm|01",
                "020|Huyện Thanh Trì|01",
                "021|Quận Bắc Từ Liêm|01",
                "024|Thành phố Hà Giang|02",
                "026|Huyện Đồng Văn|02",
                "027|Huyện Mèo Vạc|02",
                "028|Huyện Yên Minh|02",
                "029|Huyện Quản Bạ|02",
                "030|Huyện Vị Xuyên|02",
                "031|Huyện Bắc Mê|02",
                "032|Huyện Hoàng Su Phì|02",
                "033|Huyện Xín Mần|02",
                "034|Huyện Bắc Quang|02",
                "035|Huyện Quang Bình|02",
                "040|Thành phố Cao Bằng|04",
                "042|Huyện Bảo Lâm|04",
                "043|Huyện Bảo Lạc|04",
                "044|Huyện Thông Nông|04",
                "045|Huyện Hà Quảng|04",
                "046|Huyện Trà Lĩnh|04",
                "047|Huyện Trùng Khánh|04",
                "048|Huyện Hạ Lang|04",
                "049|Huyện Quảng Uyên|04",
                "050|Huyện Phục Hòa|04",
                "051|Huyện Hòa An|04",
                "052|Huyện Nguyên Bình|04",
                "053|Huyện Thạch An|04",
                "058|Thành Phố Bắc Kạn|06",
                "060|Huyện Pác Nặm|06",
                "061|Huyện Ba Bể|06",
                "062|Huyện Ngân Sơn|06",
                "063|Huyện Bạch Thông|06",
                "064|Huyện Chợ Đồn|06",
                "065|Huyện Chợ Mới|06",
                "066|Huyện Na Rì|06",
                "070|Thành phố Tuyên Quang|08",
                "071|Huyện Lâm Bình|08",
                "072|Huyện Na Hang|08",
                "073|Huyện Chiêm Hóa|08",
                "074|Huyện Hàm Yên|08",
                "075|Huyện Yên Sơn|08",
                "076|Huyện Sơn Dương|08",
                "080|Thành phố Lào Cai|10",
                "082|Huyện Bát Xát|10",
                "083|Huyện Mường Khương|10",
                "084|Huyện Si Ma Cai|10",
                "085|Huyện Bắc Hà|10",
                "086|Huyện Bảo Thắng|10",
                "087|Huyện Bảo Yên|10",
                "088|Huyện Sa Pa|10",
                "089|Huyện Văn Bàn|10",
                "094|Thành phố Điện Biên Phủ|11",
                "095|Thị Xã Mường Lay|11",
                "096|Huyện Mường Nhé|11",
                "097|Huyện Mường Chà|11",
                "098|Huyện Tủa Chùa|11",
                "099|Huyện Tuần Giáo|11",
                "100|Huyện Điện Biên|11",
                "101|Huyện Điện Biên Đông|11",
                "102|Huyện Mường Ảng|11",
                "103|Huyện Nậm Pồ|11",
                "105|Thành phố Lai Châu|12",
                "106|Huyện Tam Đường|12",
                "107|Huyện Mường Tè|12",
                "108|Huyện Sìn Hồ|12",
                "109|Huyện Phong Thổ|12",
                "110|Huyện Than Uyên|12",
                "111|Huyện Tân Uyên|12",
                "112|Huyện Nậm Nhùn|12",
                "116|Thành phố Sơn La|14",
                "118|Huyện Quỳnh Nhai|14",
                "119|Huyện Thuận Châu|14",
                "120|Huyện Mường La|14",
                "121|Huyện Bắc Yên|14",
                "122|Huyện Phù Yên|14",
                "123|Huyện Mộc Châu|14",
                "124|Huyện Yên Châu|14",
                "125|Huyện Mai Sơn|14",
                "126|Huyện Sông Mã|14",
                "127|Huyện Sốp Cộp|14",
                "128|Huyện Vân Hồ|14",
                "132|Thành phố Yên Bái|15",
                "133|Thị xã Nghĩa Lộ|15",
                "135|Huyện Lục Yên|15",
                "136|Huyện Văn Yên|15",
                "137|Huyện Mù Căng Chải|15",
                "138|Huyện Trấn Yên|15",
                "139|Huyện Trạm Tấu|15",
                "140|Huyện Văn Chấn|15",
                "141|Huyện Yên Bình|15",
                "148|Thành phố Hòa Bình|17",
                "150|Huyện Đà Bắc|17",
                "151|Huyện Kỳ Sơn|17",
                "152|Huyện Lương Sơn|17",
                "153|Huyện Kim Bôi|17",
                "154|Huyện Cao Phong|17",
                "155|Huyện Tân Lạc|17",
                "156|Huyện Mai Châu|17",
                "157|Huyện Lạc Sơn|17",
                "158|Huyện Yên Thủy|17",
                "159|Huyện Lạc Thủy|17",
                "164|Thành phố Thái Nguyên|19",
                "165|Thành phố Sông Công|19",
                "167|Huyện Định Hóa|19",
                "168|Huyện Phú Lương|19",
                "169|Huyện Đồng Hỷ|19",
                "170|Huyện Võ Nhai|19",
                "171|Huyện Đại Từ|19",
                "172|Thị xã Phổ Yên|19",
                "173|Huyện Phú Bình|19",
                "178|Thành phố Lạng Sơn|20",
                "180|Huyện Tràng Định|20",
                "181|Huyện Bình Gia|20",
                "182|Huyện Văn Lãng|20",
                "183|Huyện Cao Lộc|20",
                "184|Huyện Văn Quan|20",
                "185|Huyện Bắc Sơn|20",
                "186|Huyện Hữu Lũng|20",
                "187|Huyện Chi Lăng|20",
                "188|Huyện Lộc Bình|20",
                "189|Huyện Đình Lập|20",
                "193|Thành phố Hạ Long|22",
                "194|Thành phố Móng Cái|22",
                "195|Thành phố Cẩm Phả|22",
                "196|Thành phố Uông Bí|22",
                "198|Huyện Bình Liêu|22",
                "199|Huyện Tiên Yên|22",
                "200|Huyện Đầm Hà|22",
                "201|Huyện Hải Hà|22",
                "202|Huyện Ba Chẽ|22",
                "203|Huyện Vân Đồn|22",
                "204|Huyện Hoành Bồ|22",
                "205|Thị xã Đông Triều|22",
                "206|Thị xã Quảng Yên|22",
                "207|Huyện Cô Tô|22",
                "213|Thành phố Bắc Giang|24",
                "215|Huyện Yên Thế|24",
                "216|Huyện Tân Yên|24",
                "217|Huyện Lạng Giang|24",
                "218|Huyện Lục Nam|24",
                "219|Huyện Lục Ngạn|24",
                "220|Huyện Sơn Động|24",
                "221|Huyện Yên Dũng|24",
                "222|Huyện Việt Yên|24",
                "223|Huyện Hiệp Hòa|24",
                "227|Thành phố Việt Trì|25",
                "228|Thị xã Phú Thọ|25",
                "230|Huyện Đoan Hùng|25",
                "231|Huyện Hạ Hòa|25",
                "232|Huyện Thanh Ba|25",
                "233|Huyện Phù Ninh|25",
                "234|Huyện Yên Lập|25",
                "235|Huyện Cẩm Khê|25",
                "236|Huyện Tam Nông|25",
                "237|Huyện Lâm Thao|25",
                "238|Huyện Thanh Sơn|25",
                "239|Huyện Thanh Thuỷ|25",
                "240|Huyện Tân Sơn|25",
                "243|Thành phố Vĩnh Yên|26",
                "244|Thị xã Phúc Yên|26",
                "246|Huyện Lập Thạch|26",
                "247|Huyện Tam Dương|26",
                "248|Huyện Tam Đảo|26",
                "249|Huyện Bình Xuyên|26",
                "250|Huyện Mê Linh|01",
                "251|Huyện Yên Lạc|26",
                "252|Huyện Vĩnh Tường|26",
                "253|Huyện Sông Lô|26",
                "256|Thành phố Bắc Ninh|27",
                "258|Huyện Yên Phong|27",
                "259|Huyện Quế Võ|27",
                "260|Huyện Tiên Du|27",
                "261|Thị xã Từ Sơn|27",
                "262|Huyện Thuận Thành|27",
                "263|Huyện Gia Bình|27",
                "264|Huyện Lương Tài|27",
                "268|Quận Hà Đông|01",
                "269|Thị xã Sơn Tây|01",
                "271|Huyện Ba Vì|01",
                "272|Huyện Phúc Thọ|01",
                "273|Huyện Đan Phượng|01",
                "274|Huyện Hoài Đức|01",
                "275|Huyện Quốc Oai|01",
                "276|Huyện Thạch Thất|01",
                "277|Huyện Chương Mỹ|01",
                "278|Huyện Thanh Oai|01",
                "279|Huyện Thường Tín|01",
                "280|Huyện Phú Xuyên|01",
                "281|Huyện Ứng Hòa|01",
                "282|Huyện Mỹ Đức|01",
                "288|Thành phố Hải Dương|30",
                "290|Thị xã Chí Linh|30",
                "291|Huyện Nam Sách|30",
                "292|Huyện Kinh Môn|30",
                "293|Huyện Kim Thành|30",
                "294|Huyện Thanh Hà|30",
                "295|Huyện Cẩm Giàng|30",
                "296|Huyện Bình Giang|30",
                "297|Huyện Gia Lộc|30",
                "298|Huyện Tứ Kỳ|30",
                "299|Huyện Ninh Giang|30",
                "300|Huyện Thanh Miện|30",
                "303|Quận Hồng Bàng|31",
                "304|Quận Ngô Quyền|31",
                "305|Quận Lê Chân|31",
                "306|Quận Hải An|31",
                "307|Quận Kiến An|31",
                "308|Quận Đồ Sơn|31",
                "309|Quận Dương Kinh|31",
                "311|Huyện Thủy Nguyên|31",
                "312|Huyện An Dương|31",
                "313|Huyện An Lão|31",
                "314|Huyện Kiến Thuỵ|31",
                "315|Huyện Tiên Lãng|31",
                "316|Huyện Vĩnh Bảo|31",
                "317|Huyện Cát Hải|31",
                "318|Huyện Bạch Long Vĩ|31",
                "323|Thành phố Hưng Yên|33",
                "325|Huyện Văn Lâm|33",
                "326|Huyện Văn Giang|33",
                "327|Huyện Yên Mỹ|33",
                "328|Huyện Mỹ Hào|33",
                "329|Huyện Ân Thi|33",
                "330|Huyện Khoái Châu|33",
                "331|Huyện Kim Động|33",
                "332|Huyện Tiên Lữ|33",
                "333|Huyện Phù Cừ|33",
                "336|Thành phố Thái Bình|34",
                "338|Huyện Quỳnh Phụ|34",
                "339|Huyện Hưng Hà|34",
                "340|Huyện Đông Hưng|34",
                "341|Huyện Thái Thụy|34",
                "342|Huyện Tiền Hải|34",
                "343|Huyện Kiến Xương|34",
                "344|Huyện Vũ Thư|34",
                "347|Thành phố Phủ Lý|35",
                "349|Huyện Duy Tiên|35",
                "350|Huyện Kim Bảng|35",
                "351|Huyện Thanh Liêm|35",
                "352|Huyện Bình Lục|35",
                "353|Huyện Lý Nhân|35",
                "356|Thành phố Nam Định|36",
                "358|Huyện Mỹ Lộc|36",
                "359|Huyện Vụ Bản|36",
                "360|Huyện Ý Yên|36",
                "361|Huyện Nghĩa Hưng|36",
                "362|Huyện Nam Trực|36",
                "363|Huyện Trực Ninh|36",
                "364|Huyện Xuân Trường|36",
                "365|Huyện Giao Thủy|36",
                "366|Huyện Hải Hậu|36",
                "369|Thành phố Ninh Bình|37",
                "370|Thành phố Tam Điệp|37",
                "372|Huyện Nho Quan|37",
                "373|Huyện Gia Viễn|37",
                "374|Huyện Hoa Lư|37",
                "375|Huyện Yên Khánh|37",
                "376|Huyện Kim Sơn|37",
                "377|Huyện Yên Mô|37",
                "380|Thành phố Thanh Hóa|38",
                "381|Thị xã Bỉm Sơn|38",
                "382|Thành phố Sầm Sơn|38",
                "384|Huyện Mường Lát|38",
                "385|Huyện Quan Hóa|38",
                "386|Huyện Bá Thước|38",
                "387|Huyện Quan Sơn|38",
                "388|Huyện Lang Chánh|38",
                "389|Huyện Ngọc Lặc|38",
                "390|Huyện Cẩm Thủy|38",
                "391|Huyện Thạch Thành|38",
                "392|Huyện Hà Trung|38",
                "393|Huyện Vĩnh Lộc|38",
                "394|Huyện Yên Định|38",
                "395|Huyện Thọ Xuân|38",
                "396|Huyện Thường Xuân|38",
                "397|Huyện Triệu Sơn|38",
                "398|Huyện Thiệu Hóa|38",
                "399|Huyện Hoằng Hóa|38",
                "400|Huyện Hậu Lộc|38",
                "401|Huyện Nga Sơn|38",
                "402|Huyện Như Xuân|38",
                "403|Huyện Như Thanh|38",
                "404|Huyện Nông Cống|38",
                "405|Huyện Đông Sơn|38",
                "406|Huyện Quảng Xương|38",
                "407|Huyện Tĩnh Gia|38",
                "412|Thành phố Vinh|40",
                "413|Thị xã Cửa Lò|40",
                "414|Thị xã Thái Hòa|40",
                "415|Huyện Quế Phong|40",
                "416|Huyện Quỳ Châu|40",
                "417|Huyện Kỳ Sơn|40",
                "418|Huyện Tương Dương|40",
                "419|Huyện Nghĩa Đàn|40",
                "420|Huyện Quỳ Hợp|40",
                "421|Huyện Quỳnh Lưu|40",
                "422|Huyện Con Cuông|40",
                "423|Huyện Tân Kỳ|40",
                "424|Huyện Anh Sơn|40",
                "425|Huyện Diễn Châu|40",
                "426|Huyện Yên Thành|40",
                "427|Huyện Đô Lương|40",
                "428|Huyện Thanh Chương|40",
                "429|Huyện Nghi Lộc|40",
                "430|Huyện Nam Đàn|40",
                "431|Huyện Hưng Nguyên|40",
                "432|Thị xã Hoàng Mai|40",
                "436|Thành phố Hà Tĩnh|42",
                "437|Thị xã Hồng Lĩnh|42",
                "439|Huyện Hương Sơn|42",
                "440|Huyện Đức Thọ|42",
                "441|Huyện Vũ Quang|42",
                "442|Huyện Nghi Xuân|42",
                "443|Huyện Can Lộc|42",
                "444|Huyện Hương Khê|42",
                "445|Huyện Thạch Hà|42",
                "446|Huyện Cẩm Xuyên|42",
                "447|Huyện Kỳ Anh|42",
                "448|Huyện Lộc Hà|42",
                "449|Thị xã Kỳ Anh|42",
                "450|Thành Phố Đồng Hới|44",
                "452|Huyện Minh Hóa|44",
                "453|Huyện Tuyên Hóa|44",
                "454|Huyện Quảng Trạch|44",
                "455|Huyện Bố Trạch|44",
                "456|Huyện Quảng Ninh|44",
                "457|Huyện Lệ Thủy|44",
                "458|Thị xã Ba Đồn|44",
                "461|Thành phố Đông Hà|45",
                "462|Thị xã Quảng Trị|45",
                "464|Huyện Vĩnh Linh|45",
                "465|Huyện Hướng Hóa|45",
                "466|Huyện Gio Linh|45",
                "467|Huyện Đakrông|45",
                "468|Huyện Cam Lộ|45",
                "469|Huyện Triệu Phong|45",
                "470|Huyện Hải Lăng|45",
                "474|Thành phố Huế|46",
                "476|Huyện Phong Điền|46",
                "477|Huyện Quảng Điền|46",
                "478|Huyện Phú Vang|46",
                "479|Thị xã Hương Thủy|46",
                "480|Thị xã Hương Trà|46",
                "481|Huyện A Lưới|46",
                "482|Huyện Phú Lộc|46",
                "483|Huyện Nam Đông|46",
                "490|Quận Liên Chiểu|48",
                "491|Quận Thanh Khê|48",
                "492|Quận Hải Châu|48",
                "493|Quận Sơn Trà|48",
                "494|Quận Ngũ Hành Sơn|48",
                "495|Quận Cẩm Lệ|48",
                "497|Huyện Hòa Vang|48",
                "502|Thành phố Tam Kỳ|49",
                "503|Thành phố Hội An|49",
                "504|Huyện Tây Giang|49",
                "505|Huyện Đông Giang|49",
                "506|Huyện Đại Lộc|49",
                "507|Thị xã Điện Bàn|49",
                "508|Huyện Duy Xuyên|49",
                "509|Huyện Quế Sơn|49",
                "510|Huyện Nam Giang|49",
                "511|Huyện Phước Sơn|49",
                "512|Huyện Hiệp Đức|49",
                "513|Huyện Thăng Bình|49",
                "514|Huyện Tiên Phước|49",
                "515|Huyện Bắc Trà My|49",
                "516|Huyện Nam Trà My|49",
                "517|Huyện Núi Thành|49",
                "518|Huyện Phú Ninh|49",
                "519|Huyện Nông Sơn|49",
                "522|Thành phố Quảng Ngãi|51",
                "524|Huyện Bình Sơn|51",
                "525|Huyện Trà Bồng|51",
                "526|Huyện Tây Trà|51",
                "527|Huyện Sơn Tịnh|51",
                "528|Huyện Tư Nghĩa|51",
                "529|Huyện Sơn Hà|51",
                "530|Huyện Sơn Tây|51",
                "531|Huyện Minh Long|51",
                "532|Huyện Nghĩa Hành|51",
                "533|Huyện Mộ Đức|51",
                "534|Huyện Đức Phổ|51",
                "535|Huyện Ba Tơ|51",
                "536|Huyện Lý Sơn|51",
                "540|Thành phố Quy Nhơn|52",
                "542|Huyện An Lão|52",
                "543|Huyện Hoài Nhơn|52",
                "544|Huyện Hoài Ân|52",
                "545|Huyện Phù Mỹ|52",
                "546|Huyện Vĩnh Thạnh|52",
                "547|Huyện Tây Sơn|52",
                "548|Huyện Phù Cát|52",
                "549|Thị xã An Nhơn|52",
                "550|Huyện Tuy Phước|52",
                "551|Huyện Vân Canh|52",
                "555|Thành phố Tuy Hòa|54",
                "557|Thị xã Sông Cầu|54",
                "558|Huyện Đồng Xuân|54",
                "559|Huyện Tuy An|54",
                "560|Huyện Sơn Hòa|54",
                "561|Huyện Sông Hinh|54",
                "562|Huyện Tây Hòa|54",
                "563|Huyện Phú Hòa|54",
                "564|Huyện Đông Hòa|54",
                "568|Thành phố Nha Trang|56",
                "569|Thành phố Cam Ranh|56",
                "570|Huyện Cam Lâm|56",
                "571|Huyện Vạn Ninh|56",
                "572|Thị xã Ninh Hòa|56",
                "573|Huyện Khánh Vĩnh|56",
                "574|Huyện Diên Khánh|56",
                "575|Huyện Khánh Sơn|56",
                "576|Huyện Trường Sa|56",
                "582|Thành phố Phan Rang-Tháp Chàm|58",
                "584|Huyện Bác Ái|58",
                "585|Huyện Ninh Sơn|58",
                "586|Huyện Ninh Hải|58",
                "587|Huyện Ninh Phước|58",
                "588|Huyện Thuận Bắc|58",
                "589|Huyện Thuận Nam|58",
                "593|Thành phố Phan Thiết|60",
                "594|Thị xã La Gi|60",
                "595|Huyện Tuy Phong|60",
                "596|Huyện Bắc Bình|60",
                "597|Huyện Hàm Thuận Bắc|60",
                "598|Huyện Hàm Thuận Nam|60",
                "599|Huyện Tánh Linh|60",
                "600|Huyện Đức Linh|60",
                "601|Huyện Hàm Tân|60",
                "602|Huyện Phú Quí|60",
                "608|Thành phố Kon Tum|62",
                "610|Huyện Đắk Glei|62",
                "611|Huyện Ngọc Hồi|62",
                "612|Huyện Đắk Tô|62",
                "613|Huyện Kon Plông|62",
                "614|Huyện Kon Rẫy|62",
                "615|Huyện Đắk Hà|62",
                "616|Huyện Sa Thầy|62",
                "617|Huyện Tu Mơ Rông|62",
                "618|Huyện Ia H\' Drai|62",
                "622|Thành phố Pleiku|64",
                "623|Thị xã An Khê|64",
                "624|Thị xã Ayun Pa|64",
                "625|Huyện KBang|64",
                "626|Huyện Đăk Đoa|64",
                "627|Huyện Chư Păh|64",
                "628|Huyện Ia Grai|64",
                "629|Huyện Mang Yang|64",
                "630|Huyện Kông Chro|64",
                "631|Huyện Đức Cơ|64",
                "632|Huyện Chư Prông|64",
                "633|Huyện Chư Sê|64",
                "634|Huyện Đăk Pơ|64",
                "635|Huyện Ia Pa|64",
                "637|Huyện Krông Pa|64",
                "638|Huyện Phú Thiện|64",
                "639|Huyện Chư Pưh|64",
                "643|Thành phố Buôn Ma Thuột|66",
                "644|Thị Xã Buôn Hồ|66",
                "645|Huyện Ea H\'leo|66",
                "646|Huyện Ea Súp|66",
                "647|Huyện Buôn Đôn|66",
                "648|Huyện Cư M\'gar|66",
                "649|Huyện Krông Búk|66",
                "650|Huyện Krông Năng|66",
                "651|Huyện Ea Kar|66",
                "652|Huyện M\'Đrắk|66",
                "653|Huyện Krông Bông|66",
                "654|Huyện Krông Pắc|66",
                "655|Huyện Krông A Na|66",
                "656|Huyện Lắk|66",
                "657|Huyện Cư Kuin|66",
                "660|Thị xã Gia Nghĩa|67",
                "661|Huyện Đăk Glong|67",
                "662|Huyện Cư Jút|67",
                "663|Huyện Đắk Mil|67",
                "664|Huyện Krông Nô|67",
                "665|Huyện Đắk Song|67",
                "666|Huyện Đắk R\'Lấp|67",
                "667|Huyện Tuy Đức|67",
                "672|Thành phố Đà Lạt|68",
                "673|Thành phố Bảo Lộc|68",
                "674|Huyện Đam Rông|68",
                "675|Huyện Lạc Dương|68",
                "676|Huyện Lâm Hà|68",
                "677|Huyện Đơn Dương|68",
                "678|Huyện Đức Trọng|68",
                "679|Huyện Di Linh|68",
                "680|Huyện Bảo Lâm|68",
                "681|Huyện Đạ Huoai|68",
                "682|Huyện Đạ Tẻh|68",
                "683|Huyện Cát Tiên|68",
                "688|Thị xã Phước Long|70",
                "689|Thị xã Đồng Xoài|70",
                "690|Thị xã Bình Long|70",
                "691|Huyện Bù Gia Mập|70",
                "692|Huyện Lộc Ninh|70",
                "693|Huyện Bù Đốp|70",
                "694|Huyện Hớn Quản|70",
                "695|Huyện Đồng Phú|70",
                "696|Huyện Bù Đăng|70",
                "697|Huyện Chơn Thành|70",
                "698|Huyện Phú Riềng|70",
                "703|Thành phố Tây Ninh|72",
                "705|Huyện Tân Biên|72",
                "706|Huyện Tân Châu|72",
                "707|Huyện Dương Minh Châu|72",
                "708|Huyện Châu Thành|72",
                "709|Huyện Hòa Thành|72",
                "710|Huyện Gò Dầu|72",
                "711|Huyện Bến Cầu|72",
                "712|Huyện Trảng Bàng|72",
                "718|Thành phố Thủ Dầu Một|74",
                "719|Huyện Bàu Bàng|74",
                "720|Huyện Dầu Tiếng|74",
                "721|Thị xã Bến Cát|74",
                "722|Huyện Phú Giáo|74",
                "723|Thị xã Tân Uyên|74",
                "724|Thị xã Dĩ An|74",
                "725|Thị xã Thuận An|74",
                "726|Huyện Bắc Tân Uyên|74",
                "731|Thành phố Biên Hòa|75",
                "732|Thị xã Long Khánh|75",
                "734|Huyện Tân Phú|75",
                "735|Huyện Vĩnh Cửu|75",
                "736|Huyện Định Quán|75",
                "737|Huyện Trảng Bom|75",
                "738|Huyện Thống Nhất|75",
                "739|Huyện Cẩm Mỹ|75",
                "740|Huyện Long Thành|75",
                "741|Huyện Xuân Lộc|75",
                "742|Huyện Nhơn Trạch|75",
                "747|Thành phố Vũng Tàu|77",
                "748|Thành phố Bà Rịa|77",
                "750|Huyện Châu Đức|77",
                "751|Huyện Xuyên Mộc|77",
                "752|Huyện Long Điền|77",
                "753|Huyện Đất Đỏ|77",
                "754|Huyện Tân Thành|77",
                "755|Huyện Côn Đảo|77",
                "760|Quận 1|79",
                "761|Quận 12|79",
                "762|Quận Thủ Đức|79",
                "763|Quận 9|79",
                "764|Quận Gò Vấp|79",
                "765|Quận Bình Thạnh|79",
                "766|Quận Tân Bình|79",
                "767|Quận Tân Phú|79",
                "768|Quận Phú Nhuận|79",
                "769|Quận 2|79",
                "770|Quận 3|79",
                "771|Quận 10|79",
                "772|Quận 11|79",
                "773|Quận 4|79",
                "774|Quận 5|79",
                "775|Quận 6|79",
                "776|Quận 8|79",
                "777|Quận Bình Tân|79",
                "778|Quận 7|79",
                "783|Huyện Củ Chi|79",
                "784|Huyện Hóc Môn|79",
                "785|Huyện Bình Chánh|79",
                "786|Huyện Nhà Bè|79",
                "787|Huyện Cần Giờ|79",
                "794|Thành phố Tân An|80",
                "795|Thị xã Kiến Tường|80",
                "796|Huyện Tân Hưng|80",
                "797|Huyện Vĩnh Hưng|80",
                "798|Huyện Mộc Hóa|80",
                "799|Huyện Tân Thạnh|80",
                "800|Huyện Thạnh Hóa|80",
                "801|Huyện Đức Huệ|80",
                "802|Huyện Đức Hòa|80",
                "803|Huyện Bến Lức|80",
                "804|Huyện Thủ Thừa|80",
                "805|Huyện Tân Trụ|80",
                "806|Huyện Cần Đước|80",
                "807|Huyện Cần Giuộc|80",
                "808|Huyện Châu Thành|80",
                "815|Thành phố Mỹ Tho|82",
                "816|Thị xã Gò Công|82",
                "817|Thị xã Cai Lậy|82",
                "818|Huyện Tân Phước|82",
                "819|Huyện Cái Bè|82",
                "820|Huyện Cai Lậy|82",
                "821|Huyện Châu Thành|82",
                "822|Huyện Chợ Gạo|82",
                "823|Huyện Gò Công Tây|82",
                "824|Huyện Gò Công Đông|82",
                "825|Huyện Tân Phú Đông|82",
                "829|Thành phố Bến Tre|83",
                "831|Huyện Châu Thành|83",
                "832|Huyện Chợ Lách|83",
                "833|Huyện Mỏ Cày Nam|83",
                "834|Huyện Giồng Trôm|83",
                "835|Huyện Bình Đại|83",
                "836|Huyện Ba Tri|83",
                "837|Huyện Thạnh Phú|83",
                "838|Huyện Mỏ Cày Bắc|83",
                "842|Thành phố Trà Vinh|84",
                "844|Huyện Càng Long|84",
                "845|Huyện Cầu Kè|84",
                "846|Huyện Tiểu Cần|84",
                "847|Huyện Châu Thành|84",
                "848|Huyện Cầu Ngang|84",
                "849|Huyện Trà Cú|84",
                "850|Huyện Duyên Hải|84",
                "851|Thị xã Duyên Hải|84",
                "855|Thành phố Vĩnh Long|86",
                "857|Huyện Long Hồ|86",
                "858|Huyện Mang Thít|86",
                "859|Huyện Vũng Liêm|86",
                "860|Huyện Tam Bình|86",
                "861|Thị xã Bình Minh|86",
                "862|Huyện Trà Ôn|86",
                "863|Huyện Bình Tân|86",
                "866|Thành phố Cao Lãnh|87",
                "867|Thành phố Sa Đéc|87",
                "868|Thị xã Hồng Ngự|87",
                "869|Huyện Tân Hồng|87",
                "870|Huyện Hồng Ngự|87",
                "871|Huyện Tam Nông|87",
                "872|Huyện Tháp Mười|87",
                "873|Huyện Cao Lãnh|87",
                "874|Huyện Thanh Bình|87",
                "875|Huyện Lấp Vò|87",
                "876|Huyện Lai Vung|87",
                "877|Huyện Châu Thành|87",
                "883|Thành phố Long Xuyên|89",
                "884|Thành phố Châu Đốc|89",
                "886|Huyện An Phú|89",
                "887|Thị xã Tân Châu|89",
                "888|Huyện Phú Tân|89",
                "889|Huyện Châu Phú|89",
                "890|Huyện Tịnh Biên|89",
                "891|Huyện Tri Tôn|89",
                "892|Huyện Châu Thành|89",
                "893|Huyện Chợ Mới|89",
                "894|Huyện Thoại Sơn|89",
                "899|Thành phố Rạch Giá|91",
                "900|Thị xã Hà Tiên|91",
                "902|Huyện Kiên Lương|91",
                "903|Huyện Hòn Đất|91",
                "904|Huyện Tân Hiệp|91",
                "905|Huyện Châu Thành|91",
                "906|Huyện Giồng Riềng|91",
                "907|Huyện Gò Quao|91",
                "908|Huyện An Biên|91",
                "909|Huyện An Minh|91",
                "910|Huyện Vĩnh Thuận|91",
                "911|Huyện Phú Quốc|91",
                "912|Huyện Kiên Hải|91",
                "913|Huyện U Minh Thượng|91",
                "914|Huyện Giang Thành|91",
                "916|Quận Ninh Kiều|92",
                "917|Quận Ô Môn|92",
                "918|Quận Bình Thuỷ|92",
                "919|Quận Cái Răng|92",
                "923|Quận Thốt Nốt|92",
                "924|Huyện Vĩnh Thạnh|92",
                "925|Huyện Cờ Đỏ|92",
                "926|Huyện Phong Điền|92",
                "927|Huyện Thới Lai|92",
                "930|Thành phố Vị Thanh|93",
                "931|Thị xã Ngã Bảy|93",
                "932|Huyện Châu Thành A|93",
                "933|Huyện Châu Thành|93",
                "934|Huyện Phụng Hiệp|93",
                "935|Huyện Vị Thủy|93",
                "936|Huyện Long Mỹ|93",
                "937|Thị xã Long Mỹ|93",
                "941|Thành phố Sóc Trăng|94",
                "942|Huyện Châu Thành|94",
                "943|Huyện Kế Sách|94",
                "944|Huyện Mỹ Tú|94",
                "945|Huyện Cù Lao Dung|94",
                "946|Huyện Long Phú|94",
                "947|Huyện Mỹ Xuyên|94",
                "948|Thị xã Ngã Năm|94",
                "949|Huyện Thạnh Trị|94",
                "950|Thị xã Vĩnh Châu|94",
                "951|Huyện Trần Đề|94",
                "954|Thành phố Bạc Liêu|95",
                "956|Huyện Hồng Dân|95",
                "957|Huyện Phước Long|95",
                "958|Huyện Vĩnh Lợi|95",
                "959|Thị xã Giá Rai|95",
                "960|Huyện Đông Hải|95",
                "961|Huyện Hòa Bình|95",
                "964|Thành phố Cà Mau|96",
                "966|Huyện U Minh|96",
                "967|Huyện Thới Bình|96",
                "968|Huyện Trần Văn Thời|96",
                "969|Huyện Cái Nước|96",
                "970|Huyện Đầm Dơi|96",
                "971|Huyện Năm Căn|96",
                "972|Huyện Phú Tân|96",
                "973|Huyện Ngọc Hiển|96",
            };

            foreach (var district in districts)
            {
                var dis = district.Split('|');

                var entity = new District()
                {
                    Id = Int32.Parse(dis[0]),
                    Name = dis[1],
                    ProvinceCode = dis[2],
                    ProvinceId = Int32.Parse(dis[2]),
                };

                context.District.Add(entity);
            }

            string[] countries = new string[]
            {
                "1|AF|Afghanistan|AFG|4|93",
                "2|AL|Albania|ALB|8|355",
                "3|DZ|Algeria|DZA|12|213",
                "4|AS|American Samoa|ASM|16|1684",
                "5|AD|Andorra|AND|20|376",
                "6|AO|Angola|AGO|24|244",
                "7|AI|Anguilla|AIA|660|1264",
                "8|AQ|Antarctica|NULL|NULL|0",
                "9|AG|Antigua and Barbuda|ATG|28|1268",
                "10|AR|Argentina|ARG|32|54",
                "11|AM|Armenia|ARM|51|374",
                "12|AW|Aruba|ABW|533|297",
                "13|AU|Australia|AUS|36|61",
                "14|AT|Austria|AUT|40|43",
                "15|AZ|Azerbaijan|AZE|31|994",
                "16|BS|Bahamas|BHS|44|1242",
                "17|BH|Bahrain|BHR|48|973",
                "18|BD|Bangladesh|BGD|50|880",
                "19|BB|Barbados|BRB|52|1246",
                "20|BY|Belarus|BLR|112|375",
                "21|BE|Belgium|BEL|56|32",
                "22|BZ|Belize|BLZ|84|501",
                "23|BJ|Benin|BEN|204|229",
                "24|BM|Bermuda|BMU|60|1441",
                "25|BT|Bhutan|BTN|64|975",
                "26|BO|Bolivia|BOL|68|591",
                "27|BA|Bosnia and Herzegovina|BIH|70|387",
                "28|BW|Botswana|BWA|72|267",
                "29|BV|Bouvet Island|NULL|NULL|0",
                "30|BR|Brazil|BRA|76|55",
                "31|IO|British Indian Ocean Territory|NULL|NULL|246",
                "32|BN|Brunei Darussalam|BRN|96|673",
                "33|BG|Bulgaria|BGR|100|359",
                "34|BF|Burkina Faso|BFA|854|226",
                "35|BI|Burundi|BDI|108|257",
                "36|KH|Cambodia|KHM|116|855",
                "37|CM|Cameroon|CMR|120|237",
                "38|CA|Canada|CAN|124|1",
                "39|CV|Cape Verde|CPV|132|238",
                "40|KY|Cayman Islands|CYM|136|1345",
                "41|CF|Central African Republic|CAF|140|236",
                "42|TD|Chad|TCD|148|235",
                "43|CL|Chile|CHL|152|56",
                "44|CN|China|CHN|156|86",
                "45|CX|Christmas Island|NULL|NULL|61",
                "46|CC|Cocos Keeling Islands | NULL | NULL | 672",
                "47|CO|Colombia|COL|170|57",
                "48|KM|Comoros|COM|174|269",
                "49|CG|Congo|COG|178|242",
                "50|CD|Congo|the Democratic Republic of the|COD|180|242",
                "51|CK|Cook Islands|COK|184|682",
                "52|CR|Costa Rica|CRI|188|506",
                "53|CI|Cote D''Ivoire|CIV|384|225",
                "54|HR|Croatia|HRV|191|385",
                "55|CU|Cuba|CUB|192|53",
                "56|CY|Cyprus|CYP|196|357",
                "57|CZ|Czech Republic|CZE|203|420",
                "58|DK|Denmark|DNK|208|45",
                "59|DJ|Djibouti|DJI|262|253",
                "60|DM|Dominica|DMA|212|1767",
                "61|DO|Dominican Republic|DOM|214|1809",
                "62|EC|Ecuador|ECU|218|593",
                "63|EG|Egypt|EGY|818|20",
                "64|SV|El Salvador|SLV|222|503",
                "65|GQ|Equatorial Guinea|GNQ|226|240",
                "66|ER|Eritrea|ERI|232|291",
                "67|EE|Estonia|EST|233|372",
                "68|ET|Ethiopia|ETH|231|251",
                "69|FK|Falkland Islands Malvinas| FLK | 238 | 500",
                "70|FO|Faroe Islands|FRO|234|298",
                "71|FJ|Fiji|FJI|242|679",
                "72|FI|Finland|FIN|246|358",
                "73|FR|France|FRA|250|33",
                "74|GF|French Guiana|GUF|254|594",
                "75|PF|French Polynesia|PYF|258|689",
                "76|TF|French Southern Territories|NULL|NULL|0",
                "77|GA|Gabon|GAB|266|241",
                "78|GM|Gambia|GMB|270|220",
                "79|GE|Georgia|GEO|268|995",
                "80|DE|Germany|DEU|276|49",
                "81|GH|Ghana|GHA|288|233",
                "82|GI|Gibraltar|GIB|292|350",
                "83|GR|Greece|GRC|300|30",
                "84|GL|Greenland|GRL|304|299",
                "85|GD|Grenada|GRD|308|1473",
                "86|GP|Guadeloupe|GLP|312|590",
                "87|GU|Guam|GUM|316|1671",
                "88|GT|Guatemala|GTM|320|502",
                "89|GN|Guinea|GIN|324|224",
                "90|GW|Guinea-Bissau|GNB|624|245",
                "91|GY|Guyana|GUY|328|592",
                "92|HT|Haiti|HTI|332|509",
                "93|HM|Heard Island and Mcdonald Islands|NULL|NULL|0",
                "94|VA|Holy See Vatican City State| VAT | 336 | 39",
                "95|HN|Honduras|HND|340|504",
                "96|HK|Hong Kong|HKG|344|852",
                "97|HU|Hungary|HUN|348|36",
                "98|IS|Iceland|ISL|352|354",
                "99|IN|India|IND|356|91",
                "100|ID|Indonesia|IDN|360|62",
                "101|IR|Iran|Islamic Republic of|IRN|364|98",
                "102|IQ|Iraq|IRQ|368|964",
                "103|IE|Ireland|IRL|372|353",
                "104|IL|Israel|ISR|376|972",
                "105|IT|Italy|ITA|380|39",
                "106|JM|Jamaica|JAM|388|1876",
                "107|JP|Japan|JPN|392|81",
                "108|JO|Jordan|JOR|400|962",
                "109|KZ|Kazakhstan|KAZ|398|7",
                "110|KE|Kenya|KEN|404|254",
                "111|KI|Kiribati|KIR|296|686",
                "112|KP|Korea|Democratic People''s Republic of|PRK|408|850",
                "113|KR|Korea|Republic of|KOR|410|82",
                "114|KW|Kuwait|KWT|414|965",
                "115|KG|Kyrgyzstan|KGZ|417|996",
                "116|LA|Lao People''s Democratic Republic|LAO|418|856",
                "117|LV|Latvia|LVA|428|371",
                "118|LB|Lebanon|LBN|422|961",
                "119|LS|Lesotho|LSO|426|266",
                "120|LR|Liberia|LBR|430|231",
                "121|LY|Libyan Arab Jamahiriya|LBY|434|218",
                "122|LI|Liechtenstein|LIE|438|423",
                "123|LT|Lithuania|LTU|440|370",
                "124|LU|Luxembourg|LUX|442|352",
                "125|MO|Macao|MAC|446|853",
                "126|MK|Macedonia|the Former Yugoslav Republic of|MKD|807|389",
                "127|MG|Madagascar|MDG|450|261",
                "128|MW|Malawi|MWI|454|265",
                "129|MY|Malaysia|MYS|458|60",
                "130|MV|Maldives|MDV|462|960",
                "131|ML|Mali|MLI|466|223",
                "132|MT|Malta|MLT|470|356",
                "133|MH|Marshall Islands|MHL|584|692",
                "134|MQ|Martinique|MTQ|474|596",
                "135|MR|Mauritania|MRT|478|222",
                "136|MU|Mauritius|MUS|480|230",
                "137|YT|Mayotte|NULL|NULL|269",
                "138|MX|Mexico|MEX|484|52",
                "139|FM|Micronesia|Federated States of|FSM|583|691",
                "140|MD|Moldova|Republic of|MDA|498|373",
                "141|MC|Monaco|MCO|492|377",
                "142|MN|Mongolia|MNG|496|976",
                "143|MS|Montserrat|MSR|500|1664",
                "144|MA|Morocco|MAR|504|212",
                "145|MZ|Mozambique|MOZ|508|258",
                "146|MM|Myanmar|MMR|104|95",
                "147|NA|Namibia|NAM|516|264",
                "148|NR|Nauru|NRU|520|674",
                "149|NP|Nepal|NPL|524|977",
                "150|NL|Netherlands|NLD|528|31",
                "151|AN|Netherlands Antilles|ANT|530|599",
                "152|NC|New Caledonia|NCL|540|687",
                "153|NZ|New Zealand|NZL|554|64",
                "154|NI|Nicaragua|NIC|558|505",
                "155|NE|Niger|NER|562|227",
                "156|NG|Nigeria|NGA|566|234",
                "157|NU|Niue|NIU|570|683",
                "158|NF|Norfolk Island|NFK|574|672",
                "159|MP|Northern Mariana Islands|MNP|580|1670",
                "160|NO|Norway|NOR|578|47",
                "161|OM|Oman|OMN|512|968",
                "162|PK|Pakistan|PAK|586|92",
                "163|PW|Palau|PLW|585|680",
                "164|PS|Palestinian Territory|Occupied|NULL|NULL|970",
                "165|PA|Panama|PAN|591|507",
                "166|PG|Papua New Guinea|PNG|598|675",
                "167|PY|Paraguay|PRY|600|595",
                "168|PE|Peru|PER|604|51",
                "169|PH|Philippines|PHL|608|63",
                "170|PN|Pitcairn|PCN|612|0",
                "171|PL|Poland|POL|616|48",
                "172|PT|Portugal|PRT|620|351",
                "173|PR|Puerto Rico|PRI|630|1787",
                "174|QA|Qatar|QAT|634|974",
                "175|RE|Reunion|REU|638|262",
                "176|RO|Romania|ROM|642|40",
                "177|RU|Russian Federation|RUS|643|70",
                "178|RW|Rwanda|RWA|646|250",
                "179|SH|Saint Helena|SHN|654|290",
                "180|KN|Saint Kitts and Nevis|KNA|659|1869",
                "181|LC|Saint Lucia|LCA|662|1758",
                "182|PM|Saint Pierre and Miquelon|SPM|666|508",
                "183|VC|Saint Vincent and the Grenadines|VCT|670|1784",
                "184|WS|Samoa|WSM|882|684",
                "185|SM|San Marino|SMR|674|378",
                "186|ST|Sao Tome and Principe|STP|678|239",
                "187|SA|Saudi Arabia|SAU|682|966",
                "188|SN|Senegal|SEN|686|221",
                "189|CS|Serbia and Montenegro|NULL|NULL|381",
                "190|SC|Seychelles|SYC|690|248",
                "191|SL|Sierra Leone|SLE|694|232",
                "192|SG|Singapore|SGP|702|65",
                "193|SK|Slovakia|SVK|703|421",
                "194|SI|Slovenia|SVN|705|386",
                "195|SB|Solomon Islands|SLB|90|677",
                "196|SO|Somalia|SOM|706|252",
                "197|ZA|South Africa|ZAF|710|27",
                "198|GS|South Georgia and the South Sandwich Islands|NULL|NULL|0",
                "199|ES|Spain|ESP|724|34",
                "200|LK|Sri Lanka|LKA|144|94",
                "201|SD|Sudan|SDN|736|249",
                "202|SR|Suriname|SUR|740|597",
                "203|SJ|Svalbard and Jan Mayen|SJM|744|47",
                "204|SZ|Swaziland|SWZ|748|268",
                "205|SE|Sweden|SWE|752|46",
                "206|CH|Switzerland|CHE|756|41",
                "207|SY|Syrian Arab Republic|SYR|760|963",
                "208|TW|Taiwan|Province of China|TWN|158|886",
                "209|TJ|Tajikistan|TJK|762|992",
                "210|TZ|Tanzania|United Republic of|TZA|834|255",
                "211|TH|Thailand|THA|764|66",
                "212|TL|Timor-Leste|NULL|NULL|670",
                "213|TG|Togo|TGO|768|228",
                "214|TK|Tokelau|TKL|772|690",
                "215|TO|Tonga|TON|776|676",
                "216|TT|Trinidad and Tobago|TTO|780|1868",
                "217|TN|Tunisia|TUN|788|216",
                "218|TR|Turkey|TUR|792|90",
                "219|TM|Turkmenistan|TKM|795|7370",
                "220|TC|Turks and Caicos Islands|TCA|796|1649",
                "221|TV|Tuvalu|TUV|798|688",
                "222|UG|Uganda|UGA|800|256",
                "223|UA|Ukraine|UKR|804|380",
                "224|AE|United Arab Emirates|ARE|784|971",
                "225|GB|United Kingdom|GBR|826|44",
                "226|US|United States|USA|840|1",
                "227|UM|United States Minor Outlying Islands|NULL|NULL|1",
                "228|UY|Uruguay|URY|858|598",
                "229|UZ|Uzbekistan|UZB|860|998",
                "230|VU|Vanuatu|VUT|548|678",
                "231|VE|Venezuela|VEN|862|58",
                "232|VN|Viet Nam|VNM|704|84",
                "233|VG|Virgin Islands|British|VGB|92|1284",
                "234|VI|Virgin Islands|U.s.|VIR|850|1340",
                "235|WF|Wallis and Futuna|WLF|876|681",
                "236|EH|Western Sahara|ESH|732|212",
                "237|YE|Yemen|YEM|887|967",
                "238|ZM|Zambia|ZMB|894|260",
                "239|ZW|Zimbabwe|ZWE|716|263"
            };

            foreach (var country in countries)
            {
                var cou = country.Split('|');

                var entity = new Country()
                {
                    Id = Int32.Parse(cou[0]),
                    Iso = cou[1],
                    Name = cou[2],
                    Iso3 = cou[3],
                    NumCode = cou[4],
                    PhoneCode = cou[5],
                };

                context.Country.Add(entity);
            }

            #endregion            

            context.SaveChanges();
        }

        private static void Seedup2(AppIdentityDbContext context)
        {
            if (context.LoanPackage.Any())
            {
                return;
            }

            #region Loan packate
            List<LoanPackage> loanProducts = new List<LoanPackage>()
            {
                new LoanPackage {Id = 1, Name = "Motor"},
                new LoanPackage {Id = 2, Name = "Salary"},
                new LoanPackage {Id = 3, Name = "Household"},
                new LoanPackage {Id = 4, Name = "Car"},
                new LoanPackage {Id = 5, Name = "Credit"}
            };

            context.LoanPackage.AddRange(loanProducts);
            #endregion

            context.SaveChanges();
        }

        private static void Seedup3(AppIdentityDbContext context)
        {
            #region Income Amount
            if (context.IncomeAmount.Any()) return;
            List<IncomeAmount> incomeAmounts = new List<IncomeAmount>()
            {
                new IncomeAmount {Id = 1, Name = "2 Triệu"},
                new IncomeAmount {Id = 2, Name = "3 Triệu"},
                new IncomeAmount {Id = 3, Name = "3,5 Triệu"},
                new IncomeAmount {Id = 4, Name = "4 Triệu"},
                new IncomeAmount {Id = 5, Name = "5 Triệu"},
                new IncomeAmount {Id = 6, Name = "6 Triệu"},
                new IncomeAmount {Id = 7, Name = "7 Triệu"},
                new IncomeAmount {Id = 8, Name = "7,5 Triệu"},
                new IncomeAmount {Id = 9, Name = "8 Triệu"},
                new IncomeAmount {Id = 10, Name = "9 Triệu"},
                new IncomeAmount {Id = 11, Name = "10 Triệu"},
                new IncomeAmount {Id = 12, Name = "11 Triệu"},
                new IncomeAmount {Id = 13, Name = "12 Triệu"},
                new IncomeAmount {Id = 14, Name = "13 Triệu"},
                new IncomeAmount {Id = 15, Name = "14 Triệu"},
                new IncomeAmount {Id = 16, Name = "15 Triệu"},
                new IncomeAmount {Id = 17, Name = "16 Triệu"},
                new IncomeAmount {Id = 18, Name = "17 Triệu"},
                new IncomeAmount {Id = 19, Name = "18 Triệu"},
                new IncomeAmount {Id = 20, Name = "19 Triệu"},
                new IncomeAmount {Id = 21, Name = "20 Triệu"},
                new IncomeAmount {Id = 22, Name = "21 Triệu"},
                new IncomeAmount {Id = 23, Name = "22 Triệu"},
                new IncomeAmount {Id = 24, Name = "23 Triệu"},
                new IncomeAmount {Id = 25, Name = "24 Triệu"},
                new IncomeAmount {Id = 26, Name = "25 Triệu Trở Lên"},
                new IncomeAmount {Id = 27, Name = "Không Có Thu Nhập"},
            };
            context.IncomeAmount.AddRange(incomeAmounts);
            #endregion

            #region Income Stream
            if (context.IncomeStream.Any()) return;
            List<IncomeStream> incomeStreams = new List<IncomeStream>()
            {
                new IncomeStream {Id = 1, Name = "Làm Cho Công Ty", IsActive=true},
                new IncomeStream {Id = 2, Name = "Làm Cho Tư Nhân", IsActive=true},
                new IncomeStream {Id = 3, Name = "Làm Cho Nhà Nước", IsActive=true},
                new IncomeStream {Id = 4, Name = "Nội Trợ Tại Nhà", IsActive=true},
                new IncomeStream {Id = 5, Name = "Trồng Trọt", IsActive=true},
                new IncomeStream {Id = 6, Name = "Chăn Nuôi", IsActive=true},
                new IncomeStream {Id = 7, Name = "Buôn Bán Online", IsActive=true},
                new IncomeStream {Id = 8, Name = "Buôn Bán Hàng Rong", IsActive=true},
                new IncomeStream {Id = 9, Name = "Buôn Bán Có Nơi Cố Định", IsActive=true},
                new IncomeStream {Id = 10, Name = "Buôn Bán Có Cửa Hàng", IsActive=true},
                new IncomeStream {Id = 11, Name = "Có Giấy Xác Nhận Buôn Bán", IsActive=true},
                new IncomeStream {Id = 12, Name = "Có Hợp Đồng Thuê Sạp Chợ", IsActive=true},
                new IncomeStream {Id = 13, Name = "Có Giấy Phép Hộ Kinh Doanh", IsActive=true},
                new IncomeStream {Id = 14, Name = "Có Giấy Phép Kinh Doanh Công Ty", IsActive=true},
            };
            context.IncomeStream.AddRange(incomeStreams);
            #endregion

            #region Original File
            if (context.OriginalFile.Any()) return;
            List<OriginalFile> originalFiles = new List<OriginalFile>()
            {
                new OriginalFile {Id = 1, Name = "CMND và Hộ Khẩu"},
                new OriginalFile {Id = 2, Name = "CMND và Hộ Chiếu"},
                new OriginalFile {Id = 3, Name = "CMND và Bằng Lái Xe"},
                new OriginalFile {Id = 4, Name = "Thẻ Căn Cước và Hộ Khẩu"},
                new OriginalFile {Id = 5, Name = "Thẻ Căn Cước và Hộ Chiếu"},
                new OriginalFile {Id = 6, Name = "Thẻ Căn Cước và Bằng Lái Xe"},
                new OriginalFile {Id = 7, Name = "Không Cung Cấp Được"},
            };
            context.OriginalFile.AddRange(originalFiles);

            if (context.OriginalFileAddition.Any()) return;
            List<OriginalFileAddition> originalFileAdditions = new List<OriginalFileAddition>()
            {
                new OriginalFileAddition {Id = 1, Name = "Bảo Hiểm Y Tế"},
                new OriginalFileAddition {Id = 2, Name = "Hợp Đồng Lao Động"},
                new OriginalFileAddition {Id = 3, Name = "Phụ lục Hợp Đồng Lao Động"},
                new OriginalFileAddition {Id = 4, Name = "Sao Kê Lương"},
                new OriginalFileAddition {Id = 5, Name = "Bảo Hiểm Y Tế + Sao Kê Lương"},
                new OriginalFileAddition {Id = 6, Name = "Quyết Định Nhà Nước"},
                new OriginalFileAddition {Id = 7, Name = "Sổ Lãnh Hưu Trí"},
                new OriginalFileAddition {Id = 8, Name = "Giấy Xác Nhận Lương"},
                new OriginalFileAddition {Id = 9, Name = "Bảng Lương 3 Tháng"},
                new OriginalFileAddition {Id = 10, Name = "Giấy Phép Kinh Doanh"},
                new OriginalFileAddition {Id = 11, Name = "Hóa Đơn Đóng Thuế"},
                new OriginalFileAddition {Id = 12, Name = "Hợp Đồng Thuê Sạp Chợ"},
                new OriginalFileAddition {Id = 13, Name = "Bảo Hiểm Nhân Thọ"},
                new OriginalFileAddition {Id = 14, Name = "Hợp Đồng Vay Nơi Khác"},
                new OriginalFileAddition {Id = 15, Name = "2 Hóa Đơn Điện 250.000đ"},
                new OriginalFileAddition {Id = 16, Name = "2 Hóa Đơn Điện 600.000đ"},
                new OriginalFileAddition {Id = 17, Name = "Không Cung Cấp Được"},
            };
            context.OriginalFileAddition.AddRange(originalFileAdditions);
            #endregion

            context.SaveChanges();
        }

        private static void Seedup4(AppIdentityDbContext context)
        {
            #region Job Titles
            if (context.JobTitle.Any()) return;

            List<JobTitle> jobtitles = new List<JobTitle>()
            {
                new JobTitle {Id = 1, Name = "", IsActive=true},
                new JobTitle {Id = 2, Name = "Học Sinh", IsActive=true},
                new JobTitle {Id = 3, Name = "Sinh Viên", IsActive=true},
                new JobTitle {Id = 4, Name = "Công Nhân", IsActive=true},
                new JobTitle {Id = 5, Name = "Nhân Viên", IsActive=true},
                new JobTitle {Id = 6, Name = "Quản Lý", IsActive=true},
                new JobTitle {Id = 7, Name = "Giám Đốc", IsActive=true},
                new JobTitle {Id = 8, Name = "Công Chức", IsActive=true},
                new JobTitle {Id = 9, Name = "Cử Nhân", IsActive=true},
                new JobTitle {Id = 10, Name = "Kỹ Sư", IsActive=true},
                new JobTitle {Id = 11, Name = "Thạc Sỹ", IsActive=true},
                new JobTitle {Id = 12, Name = "Tiến Sỹ", IsActive=true},
                new JobTitle {Id = 13, Name = "Giáo Sư", IsActive=true},
                new JobTitle {Id = 14, Name = "Khác", IsActive=true},
            };
            context.JobTitle.AddRange(jobtitles);
            #endregion

            #region Occupations
            if (context.Occupation.Any()) return;

            List<Occupation> occupations = new List<Occupation>()
            {
                new Occupation {Id = 1, Name = "Nhân Viên Văn Phòng", IsActive=true},
                new Occupation {Id = 2, Name = "Nhân Viên Kinh Doanh", IsActive=true},
                new Occupation {Id = 3, Name = "Công Nhân", IsActive=true},
                new Occupation {Id = 4, Name = "Sinh Viên", IsActive=true},
                new Occupation {Id = 5, Name = "Nội Trợ", IsActive=true},
                new Occupation {Id = 6, Name = "Kinh Doanh Online", IsActive=true},
                new Occupation {Id = 7, Name = "Chủ Doanh Nghiệp", IsActive=true},
                new Occupation {Id = 8, Name = "Nhạc Sĩ/Ca Sĩ", IsActive=true},
                new Occupation {Id = 9, Name = "Bác Sĩ", IsActive=true},
                new Occupation {Id = 10, Name = "Quân Đội/Công An", IsActive=true},
                new Occupation {Id = 11, Name = "Viên Chức Nhà Nước", IsActive=true},
                new Occupation {Id = 12, Name = "Giáo Viên", IsActive=true},
                new Occupation {Id = 13, Name = "Nhân Viên Ngân Hàng", IsActive=true},
                new Occupation {Id = 14, Name = "Luật Sư", IsActive=true},
                new Occupation {Id = 15, Name = "Khác", IsActive=true},
            };
            context.Occupation.AddRange(occupations);
            #endregion

            context.SaveChanges();
        }

        private static void Seedup5(AppIdentityDbContext context)
        {
            var admin = context.User.FirstOrDefault(x => x.UserName == "admin@aegona.com");
            if (admin != null)
            {
                Wallet wallet = new Wallet()
                {
                    Id = Guid.NewGuid(),
                    UserId = admin.Id,
                    Balance = 9999999999999999999,
                };
                context.Wallet.Add(wallet);
                context.SaveChanges();
            }
        }

        private static void Seedup6(AppIdentityDbContext context)
        {
            var existing = context.LoyaltyPointType.Any();
            if (!existing)
            {
                Dictionary<string, string> types = new Dictionary<string, string>() {
                    { "Transaction Points", "Points earned when Members make a regular transaction"},
                    { "Tier Bonus Points", "Bonus points in addition to the Standard points, earned when doing a regular purchase by belonging to a specific Tier"},
                    { "Expired Points", "Points that haven’t been redeemed and overpassed their validity date"},
                    { "Campaign Points", "Points offered to Members on a Multiple points or Bonus points Campaign"},
                    { "Redemption Points", "Points that have been redeemed for a gift or coupon"},
                    { "Redemption Rollback Points", "Points that are refunded to Members after canceling a point redemption"},
                    { "Welcome Points", "Points offered to Members when they register"},
                    { "Pay-with Points", ""},
                    { "Transferred Points", "Points transferred to/received from a Member"},
                    { "Sign-in Points", ""},
                    { "Referral Points", ""},
                    { "Adjusted Points", "Points adjusted manually by admin"},
                    { "Custom Points", ""},
                    { "Redemption Points Product", "Points that have been redeemed for a product"},
                    { "Redemption Rollback Product", "Points that are refunded to Members after canceling a product redemption"}
                };

                int idn = 1;
                foreach (string key in types.Keys)
                {
                    LoyaltyPointType pointType = new LoyaltyPointType()
                    {
                        Id = idn++,
                        Name = key,
                        Decs = types[key],
                        StandardType = true,
                        Active = true
                    };

                    context.LoyaltyPointType.Add(pointType);
                }

                context.SaveChanges();
            }
        }

        private static void Seedup7(AppIdentityDbContext context)
        {
            var existing = context.LoyaltyTier.Any();
            if (!existing)
            {
                LoyaltyTier tier1 = new LoyaltyTier()
                {
                    Id = 1,
                    TierName = "Silver",
                    TierLevel = 1,
                    PointMin = 0,
                    PointMax = 1000,
                    UpgradePoint = 1000,
                    Active = true
                };

                LoyaltyTier tier2 = new LoyaltyTier()
                {
                    Id = 2,
                    TierName = "Gold",
                    TierLevel = 2,
                    PointMin = 1000,
                    PointMax = 5000,
                    UpgradePoint = 5000,
                    Active = true
                };

                LoyaltyTier tier3 = new LoyaltyTier()
                {
                    Id = 3,
                    TierName = "Diamond",
                    TierLevel = 3,
                    PointMin = 5000,
                    PointMax = 10000,
                    UpgradePoint = 10000,
                    Active = true
                };

                context.LoyaltyTier.Add(tier1);
                context.LoyaltyTier.Add(tier2);
                context.LoyaltyTier.Add(tier3);
                context.SaveChanges();
            }
        }

        private static void Seedup8(AppIdentityDbContext context)
        {
            var existing = context.Unit.Any();
            if (!existing)
            {
                Unit unit1 = new Unit()
                {
                    Id = 1,
                    Name = "",
                    IsActive = true
                };

                Unit unit2 = new Unit()
                {
                    Id = 2,
                    Name = "Cái",
                    IsActive = true
                };

                Unit unit3 = new Unit()
                {
                    Id = 3,
                    Name = "Ly",
                    IsActive = true
                };

                context.Unit.Add(unit1);
                context.Unit.Add(unit2);
                context.Unit.Add(unit3);
                context.SaveChanges();
            }
        }

        private static void Seedup9(AppIdentityDbContext context)
        {
            var existing = context.TagKey.Any();
            if (!existing)
            {
                Dictionary<string, TagGroup> keys = new Dictionary<string, TagGroup>() {
                    { "Gender", TagGroup.PERSONAL},
                    { "Age", TagGroup.PERSONAL},
                    { "Birthday", TagGroup.PERSONAL},
                    { "Birthday Month", TagGroup.PERSONAL},
                    { "Country", TagGroup.PERSONAL},
                    { "State", TagGroup.PERSONAL},
                    { "City", TagGroup.PERSONAL},
                    { "Wedding Aniversaty", TagGroup.PERSONAL},
                    { "Merried Years", TagGroup.PERSONAL},
                    { "Wedding Month", TagGroup.PERSONAL},
                    { "Merried Status", TagGroup.PERSONAL},
                    { "Preferred Brand", TagGroup.BRAND},
                    { "Preferred Store", TagGroup.BRAND},
                    { "Preferrer Product", TagGroup.BRAND},
                    { "Tier", TagGroup.LOYALTY},
                    { "Register Date", TagGroup.LOYALTY},
                    { "Available Point", TagGroup.LOYALTY},
                    { "Register Store", TagGroup.LOYALTY},
                    { "Register Channel", TagGroup.LOYALTY},
                    { "Referrer", TagGroup.REFERRAL},
                    { "Referee", TagGroup.REFERRAL},
                    { "Referrers", TagGroup.REFERRAL},
                    { "First Transaction Date", TagGroup.TRANSACTION},
                    { "Last Transaction Date", TagGroup.TRANSACTION},
                    { "Number Of Transaction", TagGroup.TRANSACTION},
                    { "Amount Of Transaction", TagGroup.TRANSACTION},
                    { "Average Basket", TagGroup.TRANSACTION},
                    { "Average Items", TagGroup.TRANSACTION},
                    { "Purchased Brand", TagGroup.TRANSACTION},
                    { "Purchased Category", TagGroup.TRANSACTION},
                    { "Purchased Store", TagGroup.TRANSACTION},
                    { "Purchased Product", TagGroup.TRANSACTION},
                    { "Member LifeCycle", TagGroup.LIFE_CYCLE},
                    { "Communication preferences", TagGroup.COMMUNICATION}};

                int idn = 1;
                foreach (string key in keys.Keys)
                {
                    TagKey tagKey = new TagKey()
                    {
                        Id = idn++,
                        Name = key,
                        TagGroup = keys[key],
                        DataFormat = DataFormat.CHECK,
                    };

                    context.TagKey.Add(tagKey);
                }

                context.SaveChanges();
            }
        }

        private static void Seedup10(AppIdentityDbContext context)
        {
            var existing = context.MemberTagCategory.Any();
            if (existing) return;

            string[] categories = new string[] {
                "Compaign",
                "Loyalty",
                "LifeCycle",
                "Privacy",
                "Socio-demographic",
                "Transaction",
                "Purchase",
            };

            int catId = 1;
            foreach (string category in categories)
            {
                MemberTagCategory memberCategory = new MemberTagCategory()
                {
                    Id = catId++,
                    TagCategoryName = category,
                    Active = true
                };

                context.MemberTagCategory.Add(memberCategory);
            }

            context.SaveChanges();
        }

        private static void Seedup11(AppIdentityDbContext context)
        {
            var existing = context.MemberLifeCycle.Any();
            if (!existing)
            {
                Dictionary<string, MEMBER_LIFECYCLE> keys = new Dictionary<string, MEMBER_LIFECYCLE>() {
                    //{ "Unspecified", MEMBER_LIFECYCLE.UNSPECIFIED},
                    { "New Prospects", MEMBER_LIFECYCLE.NEW_PROSPECTS},
                    { "Sleeping Prospects", MEMBER_LIFECYCLE.SLEEPING_PROSPECTS},
                    { "First Purchasers", MEMBER_LIFECYCLE.FIRST_PURCHASERS},
                    { "Repeat Purchasers", MEMBER_LIFECYCLE.REPEAT_PURCHASERS},
                    { "Re-purchasers", MEMBER_LIFECYCLE.REPURCHASERS},
                    { "Loyal Purchasers", MEMBER_LIFECYCLE.LOYAL_PURCHASERS},
                    { "Sleeping Purchasers", MEMBER_LIFECYCLE.SLEEPING_PURCHASERS},
                    { "Lapsed Purchasers", MEMBER_LIFECYCLE.LAPSED_PURCHASERS}};

                foreach (string key in keys.Keys)
                {
                    MemberLifeCycle model = new MemberLifeCycle()
                    {
                        Id = (int)keys[key],
                        MemberLifeCycleName = key,
                        DisplayOrder = (int)keys[key],
                        Active = true,
                    };

                    context.MemberLifeCycle.Add(model);
                }

                context.SaveChanges();
            }
        }

        private static void Seedup12(AppIdentityDbContext context)
        {
            var existing = context.LoyaltyPointSetting.Any();
            if (!existing)
            {

                LoyaltyPointSetting model = new LoyaltyPointSetting()
                {
                    Id = 1,
                    Name = "Point",
                    EarningRate = 0.0001, // rate 1000 vnd = 1 point
                    RedeemedRate = 1000, // rate 1000 vnd = 1 point
                    PointExpiryRule = 1, // never expire
                    EffectiveFrom = DateTime.UtcNow,
                    Active = true,
                };

                context.LoyaltyPointSetting.Add(model);

                context.SaveChanges();
            }
        }

        private static void Seedup13(AppIdentityDbContext context)
        {
            var existing = context.MemberChannel.Any();
            if (!existing)
            {
                Dictionary<string, int> keys = new Dictionary<string, int>() {
                    { "Panda Loyalty", 1},
                    { "Store-POS", 2},
                    { "Corporate Web", 3},
                    { "Ecommerce Web", 4},
                    { "Zalo Channel", 5} };

                foreach (string key in keys.Keys)
                {
                    MemberChannel model = new MemberChannel()
                    {
                        Id = (int)keys[key],
                        ChannelName = key,
                        ActiveFrom = DateTime.UtcNow,
                        Active = true,
                    };

                    context.MemberChannel.Add(model);

                }


                context.SaveChanges();
            }
        }
        private static void Seedup14(AppIdentityDbContext context)
        {
            var existing = context.Claims.Any();
            if(!existing)
            {

                Claims claims1 = new Claims()
                {
                    Id = 1,
                    ClaimType = "MEMBERSHIP_REGISTRATION_LIMIT",
                    ClaimValue = 5000.ToString(),
                };
                context.Claims.Add(claims1);
                context.SaveChanges();

            }

        }
    }
}