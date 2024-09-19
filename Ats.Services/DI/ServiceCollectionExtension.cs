using Microsoft.Extensions.DependencyInjection;
using Ats.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Ats.Service.EmailEngine;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;
using Ats.Services.Implementation;
using Ats.Domain.Member;

namespace Ats.Services.DI

{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();

            //---------------------------------Email Service-------------------------------------
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IViewRenderService, ViewRenderService>();
            //---------------------------------Image Helper---------------------------------------

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IAdminManagementService, AdminManagementService>();
            services.AddScoped<ICommonService, CommonService>();

            //---------------------------------Api---------------------------------------            
            services.AddScoped<IAddressService, AddressService>();

            //---------------------------------Configuration---------------------------------------            
            services.AddScoped<IOccupationService, OccupationService>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IJobTitleService, JobTitleService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IOriginalFileService, OriginalFileService>();

            //---------------------------------Lead---------------------------------------            
            services.AddScoped<ILeadService, LeadService>();
            services.AddScoped<IBorrowerService, BorrowerService>();
            services.AddScoped<ILoanProductService, LoanProductService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ILoanBookService, LoanBookService>();

            //---------------------------------Account Management---------------------------------            
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IUserService, UserService>();

            //---------------------------------Loan Management---------------------------------            
            services.AddScoped<IAssetService, AssetService>();

            //---------------------------------Membership---------------------------------            
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IMemberTagService, MemberTagService>();
            services.AddScoped<IMemberLifecycleService, MemberLifecycleService>();
            services.AddScoped<IMemberTagCategoryService, MemberTagCategoryService>();
            services.AddScoped<IMemberGroupService, MemberGroupService>();
            services.AddScoped<IMemberGroupTagService, MemberGroupTagService>();
            services.AddScoped<IMemberSegmentService, MemberSegmentService>();
            
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductOrderService, ProductOrderService>();
            //services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IMemberChannelService, MemberChannelService>();
            services.AddScoped<ILoyaltyTierService, LoyaltyTierService>();
            services.AddScoped<ILoyaltyPointTypeService, LoyaltyPointTypeService>();
            services.AddScoped<IPurchasedTransactionService, PurchasedTransactionService>();
            services.AddScoped<ILoyaltyPointSettingService, LoyaltyPointSettingService>();
            //add new loyalty
            services.AddScoped<IPointRuleStoresService, PointRuleStoresService>();
            services.AddScoped<IPointRuleProductService, PointRuleProductService>();
            services.AddScoped<IPointRuleChannelService, PointRuleChannelService>();
            services.AddScoped<IPointRuleCategoryService, PointRuleCategoryService>();
            services.AddScoped<IPointRuleBrandService, PointRuleBrandService>();
            //add new ProductCollectionItem, ProductCollection
            services.AddScoped<IProductCollectionService, ProductCollectionService>();
            services.AddScoped<IProductCollectionItemService, ProductCollectionItemService>();

            services.AddScoped<IVoucherCategoryService, VoucherCategoryService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IGiftCategoryService, GiftCategoryService>();
            services.AddScoped<IGiftService, GiftService>();
            services.AddScoped<ICouponCategoryService, CouponCategoryService>();
            services.AddScoped<ICouponService, CouponService>();

            services.AddScoped<ILoyaltyPointRulesService, LoyaltyPointRulesService>();
            services.AddScoped<IRewardsService, RewardsService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IFileImportService, FileImportService>();
            services.AddScoped<IReportsService, ReportsService>();

            services.AddScoped<IMemberWalletService, MemberWalletService>();

            services.AddScoped<IMemberProductQrCodeService, MemberProductQrCodeService>();

            services.AddScoped<IClaimsService, ClaimsService>();

            //services.AddTransient<IMembershipService, MembershipService>();
            services.AddTransient<ClaimsService>();


            return services;
        }
    }
}
