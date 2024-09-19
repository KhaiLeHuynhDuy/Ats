using System;
using System.Threading.Tasks;
using Ats.Domain.Accounts.Repositories;
using Ats.Domain.Departments.Repositories;
using Ats.Domain.Address;
using Ats.Domain.Account;
using Ats.Domain.Lead;
using Ats.Domain.Organization;
using Ats.Domain.Loan;
using Ats.Domain.Member;
using Ats.Domain.Loyalty;
using Ats.Domain.Member.Repositories;
using Ats.Domain.Store;
using Ats.Domain.Channel;
using Ats.Domain.Coupon;
using Ats.Domain.Gift;
using Ats.Domain.Voucher;
using Ats.Domain.Store.Models;
using Ats.Domain.Claims;

namespace Ats.Domain
{
    public interface IUnitOfWork
    {
        void SetUser(Guid userId);
        int SaveChanges();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task SaveChangesAsync();
        IGroupRepository GroupRepo { get; }
        IRoleRepository RoleRepo { get; }

        IProfileRepository ProfileRepo { get; }
        IUserRepository UserRepo { get; }       

        IDepartmentRepository DepartmentRepo { get; }
        IDepartmentUserRepository DepartmentUserRepo { get; }

        IWalletRepository WalletRepo { get; }
        ITransactionRepository TransactionRepo { get; }

        ICountryRepository CountryRepo { get; }
        IAddressRepository AddressRepo { get; }
        IProvinceRepository ProvinceRepo { get; }
        IDistrictRepository DistrictRepo { get; }

        IJobTitleRepository JobTitleRepo { get; }
        IOccupationRepository OccupationRepo { get; }

        ITeamRepository TeamRepo { get; }
        ITeamUserRepository TeamUserRepo { get; }

        ILeadRepository LeadRepo { get; }
        ILeadAccessLevelHistoryRepository LeadAccessLevelHistoryRepo { get; }
        ILeadExchangeRequestRepository LeadExchangeRequestRepo { get; }
        ILeadStatusHistoryRepository LeadStatusHistoryRepo { get; }
        ILeadLevelHistoryRepository LeadLevelHistoryRepo { get; }
        ILeadTeamRepository LeadTeamRepo { get; }
        ILeadUserRepository LeadUserRepo { get; }

        IClientRepository ClientRepo { get; }
        IDebtRepository DebtRepo { get; }
        IDocumentRepository DocumentRepo { get; }
        IIncomeRepository IncomeRepo { get; }
        ILoanApplicationRepository LoanApplicationRepo { get; }
        Lead.ILoanPackageRepository LoanPackageRepo { get; }
        ILoanNoteRepository LoanNoteRepo { get; }
        IOrganizationRepository OrganizationRepo { get; }
        IWarehouseRepository WarehouseRepo { get; }

        IOriginalFileRepository OriginalFileRepo { get; }
        IOriginalFileAdditionRepository OriginalFileAdditionRepo { get; }
        IIncomeStreamRepository IncomeStreamRepo { get; }
        IIncomeAmountRepository IncomeAmountRepo { get; }

        ICommentRepository CommentRepo { get; }
        ILoanBookRepository LoanBookRepo { get; }
        IBorrowerRepository BorrowerRepo { get; }

        IBorrowerAccessLevelHistoryRepository BorrowerAccessLevelHistoryRepo { get; }
        IBorrowerStatusHistoryRepository BorrowerStatusHistoryRepo { get; }
        IBorrowerLevelHistoryRepository BorrowerLevelHistoryRepo { get; }

        IAssetRepository AssetRepo { get; }
        IAssetAttributeRepository AssetAttributeRepo { get; }
        IAssetLoanRepository AssetLoanRepo { get; }
        IAssetPropertyRepository AssetPropertyRepo { get; }
        IAssetTypeRepository AssetTypeRepo { get; }
        ILoanRepository LoanRepo { get; }
        Loan.ILoanProductRepository LoanProductRepo { get; }
        ILoanProductAttributeRepository LoanProductAttributeRepo { get; }
        ILoanProductCategoryRepository LoanProductCategoryRepo { get; }
        ILoanProductPropertyRepository LoanProductPropertyRepo { get; }


        // Members
        ILoyaltyPointRepository LoyaltyPointRepo { get; }
        ILoyaltyPointTypeRepository LoyaltyPointTypeRepo { get; }
        IMemberLoyaltyTransactionRepository MemberLoyaltyTransactionRepo { get; }
        IMemberRepository MemberRepo { get; }
        IMemberTagRepository MemberTagRepo { get; }
        IMemberTagCategoryRepository MemberTagCategoryRepo { get; }
        IMemberTagValueRepository MemberTagValueRepo { get; }
        ITagKeyRepository TagKeyRepo { get; }
        //IMemberTaggingRepository MemberTaggingRepo { get; }

        //add new loyalty
        IPointRuleStoresRepository PointRuleStoresRepo { get; }
        IPointRuleProductRepository PointRuleProductRepo { get; }
        IPointRuleChannelRepository PointRuleChannelRepo { get; }
        IPointRuleCategoryRepository PointRuleCategoryRepo { get; }
        IPointRuleBrandRepository PointRuleBrandRepo { get; }
        //ad new ProductCollectionItem, ProductCollection
        IProductCollectionRepository ProductCollectionRepo { get; }
        IProductCollectionItemRepository ProductCollectionItemRepo { get; }

        IMemberWalletRepository MemberWalletRepo { get; }
        IPreferredBrandRepository PreferredBrandRepo { get; }
        IPreferredProductRepository PreferredProductRepo { get; }


        ILoyaltyPointRuleRepository LoyaltyPointRuleRepo { get; }
        ILoyaltyPointSettingRepository LoyaltyPointSettingRepo { get; }
        ILoyaltyTierRepository LoyaltyTierRepo { get; }
        IMemberLoyaltyTierRepository MemberLoyaltyTierRepo { get; }

        IStoreRepository StoreRepo { get; }
        IMemberChannelRepository MemberChannelRepo { get; }
        //IBrandRepository BrandRepo { get; }
        IProductRepository ProductRepo { get; }
        IProductOrderRepository ProductOrderRepo { get; }
        IProductAttributeRepository ProductAttributeRepo { get; }
        IProductCategoryRepository ProductCategoryRepo { get; }
        IProductPropertyRepository ProductPropertyRepo { get; }
        IUnitRepository UnitRepo { get; }
        IPurchasedTransactionRepository PurchasedTransactionRepo { get; }

        IMemberGroupRepository MemberGroupRepo { get; }
        IMemberGroupTagRepository MemberGroupTagRepo { get; }
        IMemberLifeCycleRepository MemberLifeCycleSettingRepo { get; }
   
        IMemberSegmentRepository MemberSegmentRepo { get; }

        ICouponRepository CouponRepo { get; }
        IMemberCouponRepository MemberCouponRepo { get; }
        IMemberVoucherRepository MemberVoucherRepo { get; }
        IMemberGiftRepository MemberGiftRepo { get; }
        ICouponRedemptionRepository CouponRedemptionRepo { get; }

        IGiftRepository GiftRepo { get; }
        IVoucherRepository VoucherRepo { get; }

        ICouponCategoryRepository CouponCategoryRepo { get; }
        IGiftCategoryRepository GiftCategoryRepo { get; }
        IVoucherCategoryRepository VoucherCategoryRepo { get; }

        IMemberProductQrCodeRepository MemberProductQrCodeRepo { get; }
        IClaimsReprository ClaimsReprository { get; }

    }
}
