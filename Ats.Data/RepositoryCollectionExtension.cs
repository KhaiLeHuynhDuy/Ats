using Ats.Data.Accounts.Repositories;
using Ats.Data.Departments.Repositories;
using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Data.Repositories.Account;
using Ats.Data.Repositories.Address;
using Ats.Data.Repositories.Channel;
using Ats.Data.Repositories.Coupon;
using Ats.Data.Repositories.Gift;
using Ats.Data.Repositories.Lead;
using Ats.Data.Repositories.Loan;
using Ats.Data.Repositories.Loyalty;
using Ats.Data.Repositories.Member;
using Ats.Data.Repositories.Organization;
using Ats.Data.Repositories.Store;
using Ats.Data.Repositories.TagMember;
using Ats.Data.Repositories.Voucher;
using Ats.Domain;
using Ats.Domain.Account;
using Ats.Domain.Accounts.Repositories;
using Ats.Domain.Address;
using Ats.Domain.Channel;
using Ats.Domain.Coupon;
using Ats.Domain.Departments.Repositories;
using Ats.Domain.Gift;
using Ats.Domain.Lead;
using Ats.Domain.Loan;
using Ats.Domain.Loan.Models;
using Ats.Domain.Loyalty;
using Ats.Domain.Member;
using Ats.Domain.Member.Repositories;
using Ats.Domain.Organization;
using Ats.Domain.Store;
using Ats.Domain.Voucher;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Claims;
using Ats.Data.Repositories.Claims;

namespace Ats.Data
{
    public static class RepositoryCollectionExtension
    {
        public static IServiceCollection AddRepositoryCollection(this IServiceCollection services)
        {            
            // services.AddTransient<IMCDataContext>(s => new MCDataContext(options));
            //services.AddScoped(typeof(IMCDataContext), typeof(MCDataContext));

            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IJobTitleRepository, JobTitleRepository>();
            services.AddScoped<IOccupationRepository, OccupationRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamUserRepository, TeamUserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentUserRepository, DepartmentUserRepository>();

            services.AddScoped<IDebtRepository, DebtRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IIncomeRepository, IncomeRepository>();
            services.AddScoped<ILeadAccessLevelHistoryRepository, LeadAccessLevelHistoryRepository>();
            services.AddScoped<ILeadExchangeRequestRepository, LeadExchangeRequestRepository>();
            services.AddScoped<ILeadLevelHistoryRepository, LeadLevelHistoryRepository>();
            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddScoped<ILeadStatusHistoryRepository, LeadStatusHistoryRepository>();
            services.AddScoped<ILeadTeamRepository, LeadTeamRepository>();
            services.AddScoped<ILeadUserRepository, LeadUserRepository>();
            services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
            services.AddScoped<ILoanNoteRepository, LoanNoteRepository>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();

            services.AddScoped<IOriginalFileRepository, OriginalFileRepository>();
            services.AddScoped<IOriginalFileAdditionRepository, OriginalFileAdditionRepository>();
            services.AddScoped<IIncomeStreamRepository, IncomeStreamRepository>();
            services.AddScoped<IIncomeAmountRepository, IncomeAmountRepository>();

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILoanBookRepository, LoanBookRepository>();
            services.AddScoped<IBorrowerRepository, BorrowerRepository>();

            services.AddScoped<IBorrowerLevelHistoryRepository, BorrowerLevelHistoryRepository>();
            services.AddScoped<IBorrowerAccessLevelHistoryRepository, BorrowerAccessLevelHistoryRepository>();
            services.AddScoped<IBorrowerStatusHistoryRepository, BorrowerStatusHistoryRepository>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IAssetAttributeRepository, AssetAttributeRepository>();
            services.AddScoped<IAssetLoanRepository, AssetLoanRepository>();
            services.AddScoped<IAssetPropertyRepository, AssetPropertyRepository>();
            services.AddScoped<IAssetTypeRepository, AssetTypeRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<Domain.Loan.ILoanProductRepository, Repositories.Loan.LoanProductRepository>();
            services.AddScoped<ILoanProductAttributeRepository, LoanProductAttributeRepository>();
            services.AddScoped<ILoanProductCategoryRepository, LoanProductCategoryRepository>();
            services.AddScoped<ILoanProductPropertyRepository, LoanProductPropertyRepository>();


            // Member
            services.AddScoped<ILoyaltyPointRepository, LoyaltyPointRepository>();
            services.AddScoped<ILoyaltyPointTypeRepository, LoyaltyPointTypeRepository>();
            services.AddScoped<IMemberLoyaltyTransactionRepository, MemberLoyaltyTransactionRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMemberWalletRepository, MemberWalletRepository>();
            services.AddScoped<IPreferredBrandRepository, PreferredBrandRepository>();
            services.AddScoped<IPreferredProductRepository, PreferredProductRepository>();
            services.AddScoped<IProductOrderRepository, ProductOrderRepository>();

            services.AddScoped<ILoyaltyPointRuleRepository, LoyaltyPointRuleRepository>();
            services.AddScoped<ILoyaltyPointSettingRepository, LoyaltyPointSettingRepository>();
            services.AddScoped<ILoyaltyTierRepository, LoyaltyTierRepository>();
            services.AddScoped<IMemberLoyaltyTierRepository, MemberLoyaltyTierRepository>();
            //add new loyalty
            services.AddScoped<IPointRuleStoresRepository, PointRuleStoresRepository>();
            services.AddScoped<IPointRuleProductRepository, PointRuleProductRepository>();
            services.AddScoped<IPointRuleChannelRepository, PointRuleChannelRepository>();
            services.AddScoped<IPointRuleCategoryRepository, PointRuleCategoryRepository>();
            //services.AddScoped<IPointRuleBrandRepository, PointRuleBrandRepository>();
            //add new ProductCollectionItem, ProductCollection
            services.AddScoped<IProductCollectionRepository, ProductCollectionRepository>();
            services.AddScoped<IProductCollectionItemRepository, ProductCollectionItemRepository>();

            //services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IMemberChannelRepository, MemberChannelRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IMemberGroupRepository, MemberGroupRepository>();
            services.AddScoped<IMemberGroupTagRepository, MemberGroupTagRepository>();
            services.AddScoped<IMemberLifeCycleRepository, MemberLifeCycleRepository>();
            services.AddScoped<IMemberSegmentRepository, MemberSegmentRepository>();

            services.AddScoped<IMemberTagCategoryRepository, MemberTagCategoryRepository>();
            services.AddScoped<IMemberTagRepository, MemberTagRepository>();
            services.AddScoped<IMemberTagValueRepository, MemberTagValueRepository>();
            services.AddScoped<ITagKeyRepository, TagKeyRepository>();

            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IMemberCouponRepository, MemberCouponRepository>();
            services.AddScoped<IMemberVoucherRepository, MemberVoucherRepository>();
            services.AddScoped<IMemberGiftRepository, MemberGiftRepository>();
            services.AddScoped<ICouponRedemptionRepository, CouponRedemptionRepository>();
            services.AddScoped<IGiftRepository, GiftRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();

            services.AddScoped<ICouponCategoryRepository, CouponCategoryRepository>();
            services.AddScoped<IGiftCategoryRepository, GiftCategoryRepository>();
            services.AddScoped<IVoucherCategoryRepository, VoucherCategoryRepository>();
            services.AddScoped<IClaimsReprository, ClaimsRepository>();

            //services.AddScoped<IMemberTaggingRepository, MemberTaggingRepository>();

            return services;
        }
    }
}
