using System;
using Ats.Data.EntityFramework;
using System.Threading.Tasks;
using Ats.Domain.Accounts.Repositories;
using Ats.Domain.Departments.Repositories;
using Ats.Data.Accounts.Repositories;
using Ats.Data.Departments.Repositories;
using Ats.Domain.Address;
using Ats.Data.Repositories.Address;
using Ats.Domain.Account;
using Ats.Domain.Lead;
using Ats.Domain.Organization;
using Ats.Data.Repositories.Account;
using Ats.Data.Repositories.Lead;
using Ats.Data.Repositories.Organization;
using Microsoft.EntityFrameworkCore.Storage;
using Ats.Domain.Loan;
using Ats.Data.Repositories.Loan;
using Ats.Domain.Member;
using Ats.Data.Repositories.Member;
using Ats.Domain.Loyalty;
using Ats.Data.Repositories.Loyalty;
using Ats.Domain.Member.Repositories;
using Ats.Domain.Store;
using Ats.Domain.Channel;
using Ats.Data.Repositories.Store;
using Ats.Data.Repositories.Channel;
using Ats.Data.Repositories.TagMember;
using Ats.Domain.Coupon;
using Ats.Domain.Gift;
using Ats.Domain.Voucher;
using Ats.Data.Repositories.Coupon;
using Ats.Data.Repositories.Gift;
using Ats.Data.Repositories.Voucher;
using Ats.Domain.Claims;
using Ats.Data.Repositories.Claims;

namespace Ats.Data.Repositories
{
    public class UnitOfWork 
    {
        private Guid _userId;
        private IDbFactory _dbFactory;
        private SCDataContext _dbContext;
        private IDbContextTransaction transaction;

        public UnitOfWork(SCDataContext context)
        {
            _dbContext = context;
        }

        public void SetUser(Guid userId)
        {
            this._userId = userId;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
        }

        #region Declare IRepository

        private IGroupRepository _groupRepository;
        private IRoleRepository _roleRepo;

        private IProfileRepository _profileRepository;
        private IUserRepository _userRepo;

        private IWalletRepository _walletRepo;
        private ITransactionRepository _transactionRepo;

        private IDepartmentRepository _departmentRepo;
        private IDepartmentUserRepository _departmentUserRepo;        

        private IAddressRepository _addressRepository;
        private ICountryRepository _countryRepo;
        private IProvinceRepository _provinceRepo;
        private IDistrictRepository _districtRepo;

        private IJobTitleRepository _jobTitleRepo;
        private IOccupationRepository _occupationRepo;
        private ITeamRepository _teamRepo;
        private ITeamUserRepository _teamUserRepo;

        private ILeadRepository _leadRepo;
        private ILeadAccessLevelHistoryRepository _leadAccessLevelHistoryRepo;
        private ILeadExchangeRequestRepository _leadExchangeRequestRepo;
        private ILeadStatusHistoryRepository _leadStatusHistoryRepo;
        private ILeadLevelHistoryRepository _leadLevelHistoryRepo;
        private ILeadTeamRepository _leadTeamRepo;
        private ILeadUserRepository _leadUserRepo;

        private IDebtRepository _debtRepo;
        private IDocumentRepository _documentRepo;
        private IIncomeRepository _incomeRepo;
        private ILoanApplicationRepository _loanApplicationRepo;
        private Domain.Lead.ILoanPackageRepository _loanPackageRepo;
        private ILoanNoteRepository _loanNoteRepo;

        private IClientRepository _clientRepo;
        private IOrganizationRepository _organizationRepo;
        private IWarehouseRepository _warehouseRepo;

        private IIncomeStreamRepository _incomeStreamRepo;
        private IIncomeAmountRepository _incomeAmountRepo;
        private IOriginalFileRepository _originalFileRepo;
        private IOriginalFileAdditionRepository _originalFileAdditionRepo;

        private ICommentRepository _commentRepo;
        private ILoanBookRepository _loanBookRepo;
        private IBorrowerRepository _borrowerRepo;

        private IBorrowerAccessLevelHistoryRepository _borrowerAccessLevelHistoryRepo;
        private IBorrowerStatusHistoryRepository _borrowerStatusHistoryRepo;
        private IBorrowerLevelHistoryRepository _borrowerLevelHistoryRepo;

        private IAssetRepository _assetRepo;
        private IAssetLoanRepository _assetLoanRepo;
        private IAssetPropertyRepository _assetPropertyRepo;
        private IAssetAttributeRepository _assetAttributeRepo;
        private IAssetTypeRepository _assetTypeRepo;
        private ILoanRepository _loanRepo;
        private Domain.Loan.ILoanProductRepository _loanProductRepo;
        private ILoanProductAttributeRepository _loanProductAttributeRepo;
        private ILoanProductCategoryRepository _loanProductCategoryRepo;
        private ILoanProductPropertyRepository _loanProductPropertyRepo;

        //Member
        private ILoyaltyPointRepository _loyaltyPointRepo;
        private ILoyaltyPointTypeRepository _loyaltyPointTypeRepo;
        private IMemberLoyaltyTransactionRepository _memberLoyaltyTransactionRepo;
        private IMemberRepository _memberRepo;
        private IMemberWalletRepository _memberWalletRepo;

        private IPreferredBrandRepository _preferredBrandRepo;
        private IPreferredProductRepository _preferredProductRepo;

        private ILoyaltyPointRuleRepository _loyaltyPointRuleRepo;
        private ILoyaltyPointSettingRepository _loyaltyPointSettingRepo;
        private ILoyaltyTierRepository _loyaltyTierRepo;
        private IMemberLoyaltyTierRepository _memberLoyaltyTierRepo;
        // add new loyalty
        private IPointRuleStoresRepository _pointRuleStoresRepo;
        private IPointRuleProductRepository _pointRuleProductRepo;
        private IPointRuleChannelRepository _pointRuleChannelRepo;
        private IPointRuleCategoryRepository _pointRuleCategoryRepo;
        private IPointRuleBrandRepository _pointRuleBrandRepo;
        //add new ProductCollectionItem,  ProductCollection
        private IProductCollectionRepository _productCollectionRepo;
        private IProductCollectionItemRepository _productCollectionItemRepo;


        private IStoreRepository _storeRepo;
        //private IBrandRepository _brandRepo;
        private IMemberChannelRepository _channelRepo;

        private ICouponCategoryRepository _couponCategoryRepo;
        private IGiftCategoryRepository _giftCategoryRepo;
        private IVoucherCategoryRepository _voucherCategoryRepo;

        private ICouponRepository _couponRepo;
        private IMemberCouponRepository _memberCouponRepo;
        private IMemberVoucherRepository _memberVoucherRepo;
        private IMemberGiftRepository _memberGiftRepo;
        private ICouponRedemptionRepository _CouponRedemptionRepo;

        private IGiftRepository _giftRepo;
        private IVoucherRepository _voucherRepo;

        private IProductRepository _productRepo;
        private IProductOrderRepository _productOrderRepo;
        private IProductAttributeRepository _productAttributeRepo;
        private IProductCategoryRepository _productCategoryRepo;
        private IProductPropertyRepository _productPropertyRepo;
        private IUnitRepository _unitRepo;
        private IPurchasedTransactionRepository _purchasedTransactionRepo;

        private IMemberGroupRepository _memberGroupRepo;
        private IMemberGroupTagRepository _memberGroupTagRepo;
        private IMemberLifeCycleRepository _memberLifeCycleSettingRepo;
        private IMemberSegmentRepository _memberSegmentRepo;

        private IMemberTagRepository _memberTagRepo;
        private IMemberTagCategoryRepository _memberTagCategoryRepo;
        private IMemberTagValueRepository _memberTagValueRepo;
        private ITagKeyRepository _tagKeyRepo;
        private IMemberProductQrCodeRepository _memberProductQrCodeRepo;

        private IClaimsReprository _claimsRepro;

        #region Init repos

        public ICountryRepository CountryRepo => _countryRepo ?? (_countryRepo = new CountryRepository(_dbContext));
        public IAddressRepository AddressRepo => _addressRepository ?? (_addressRepository = new AddressRepository(_dbContext));
        public IProvinceRepository ProvinceRepo => _provinceRepo ?? (_provinceRepo = new ProvinceRepository(_dbContext));
        public IDistrictRepository DistrictRepo => _districtRepo ?? (_districtRepo = new DistrictRepository(_dbContext));


        public IGroupRepository GroupRepo => _groupRepository ?? (_groupRepository = new GroupRepository(_dbContext));

        public IRoleRepository RoleRepo => _roleRepo ?? (_roleRepo = new RoleRepository(_dbContext));

        public IProfileRepository ProfileRepo => _profileRepository ?? (_profileRepository = new ProfileRepository(_dbContext));

        public IUserRepository UserRepo => _userRepo ?? (_userRepo = new UserRepository(_dbContext));
        
        public IDepartmentRepository DepartmentRepo => _departmentRepo ?? (_departmentRepo = new DepartmentRepository(_dbContext));

        public IDepartmentUserRepository DepartmentUserRepo => _departmentUserRepo ?? (_departmentUserRepo = new DepartmentUserRepository(_dbContext));

        public IWalletRepository WalletRepo => _walletRepo ?? (_walletRepo = new WalletRepository(_dbContext));
        public ITransactionRepository TransactionRepo => _transactionRepo ?? (_transactionRepo = new TransactionRepository(_dbContext));

        public IJobTitleRepository JobTitleRepo => _jobTitleRepo ?? (_jobTitleRepo = new JobTitleRepository(_dbContext));

        public IOccupationRepository OccupationRepo => _occupationRepo ?? (_occupationRepo = new OccupationRepository(_dbContext));

        public ITeamRepository TeamRepo => _teamRepo ?? (_teamRepo = new TeamRepository(_dbContext));

        public ITeamUserRepository TeamUserRepo => _teamUserRepo ?? (_teamUserRepo = new TeamUserRepository(_dbContext));


        public ILeadRepository LeadRepo => _leadRepo ?? (_leadRepo = new LeadRepository(_dbContext));

        public ILeadAccessLevelHistoryRepository LeadAccessLevelHistoryRepo => _leadAccessLevelHistoryRepo ?? (_leadAccessLevelHistoryRepo = new LeadAccessLevelHistoryRepository(_dbContext));

        public ILeadExchangeRequestRepository LeadExchangeRequestRepo => _leadExchangeRequestRepo ?? (_leadExchangeRequestRepo = new LeadExchangeRequestRepository(_dbContext));

        public ILeadStatusHistoryRepository LeadStatusHistoryRepo => _leadStatusHistoryRepo ?? (_leadStatusHistoryRepo = new LeadStatusHistoryRepository(_dbContext));

        public ILeadLevelHistoryRepository LeadLevelHistoryRepo => _leadLevelHistoryRepo ?? (_leadLevelHistoryRepo = new LeadLevelHistoryRepository(_dbContext));

        public ILeadTeamRepository LeadTeamRepo => _leadTeamRepo ?? (_leadTeamRepo = new LeadTeamRepository(_dbContext));

        public ILeadUserRepository LeadUserRepo => _leadUserRepo ?? (_leadUserRepo = new LeadUserRepository(_dbContext));

        public IClientRepository ClientRepo => _clientRepo ?? (_clientRepo = new ClientRepository(_dbContext));

        public IDebtRepository DebtRepo => _debtRepo ?? (_debtRepo = new DebtRepository(_dbContext));
        public IDocumentRepository DocumentRepo => _documentRepo ?? (_documentRepo = new DocumentRepository(_dbContext));
        public IIncomeRepository IncomeRepo => _incomeRepo ?? (_incomeRepo = new IncomeRepository(_dbContext));
        public ILoanApplicationRepository LoanApplicationRepo => _loanApplicationRepo ?? (_loanApplicationRepo = new LoanApplicationRepository(_dbContext));
        public Domain.Lead.ILoanPackageRepository LoanPackageRepo => _loanPackageRepo ?? (_loanPackageRepo = new Lead.LoanPackageRepository(_dbContext));
        public ILoanNoteRepository LoanNoteRepo => _loanNoteRepo ?? (_loanNoteRepo = new LoanNoteRepository(_dbContext));
        public IOrganizationRepository OrganizationRepo => _organizationRepo ?? (_organizationRepo = new OrganizationRepository(_dbContext));

        public IOriginalFileRepository OriginalFileRepo => _originalFileRepo ?? (_originalFileRepo = new OriginalFileRepository(_dbContext));
        public IOriginalFileAdditionRepository OriginalFileAdditionRepo => _originalFileAdditionRepo ?? (_originalFileAdditionRepo = new OriginalFileAdditionRepository(_dbContext));
        public IIncomeStreamRepository IncomeStreamRepo => _incomeStreamRepo ?? (_incomeStreamRepo = new IncomeStreamRepository(_dbContext));
        public IIncomeAmountRepository IncomeAmountRepo => _incomeAmountRepo ?? (_incomeAmountRepo = new IncomeAmountRepository(_dbContext));

        public ICommentRepository CommentRepo => _commentRepo ?? (_commentRepo = new CommentRepository(_dbContext));
        public ILoanBookRepository LoanBookRepo => _loanBookRepo ?? (_loanBookRepo = new LoanBookRepository(_dbContext));
        public IBorrowerRepository BorrowerRepo => _borrowerRepo ?? (_borrowerRepo = new BorrowerRepository(_dbContext));

        public IBorrowerAccessLevelHistoryRepository BorrowerAccessLevelHistoryRepo => _borrowerAccessLevelHistoryRepo ?? (_borrowerAccessLevelHistoryRepo = new BorrowerAccessLevelHistoryRepository(_dbContext));
        public IBorrowerStatusHistoryRepository BorrowerStatusHistoryRepo => _borrowerStatusHistoryRepo ?? (_borrowerStatusHistoryRepo = new BorrowerStatusHistoryRepository(_dbContext));
        public IBorrowerLevelHistoryRepository BorrowerLevelHistoryRepo => _borrowerLevelHistoryRepo ?? (_borrowerLevelHistoryRepo = new BorrowerLevelHistoryRepository(_dbContext));
        public IWarehouseRepository WarehouseRepo => _warehouseRepo ?? (_warehouseRepo = new WarehouseRepository(_dbContext));

        public IAssetRepository AssetRepo => _assetRepo ?? (_assetRepo = new AssetRepository(_dbContext));
        public IAssetAttributeRepository AssetAttributeRepo => _assetAttributeRepo ?? (_assetAttributeRepo = new AssetAttributeRepository(_dbContext));
        public IAssetLoanRepository AssetLoanRepo => _assetLoanRepo ?? (_assetLoanRepo = new AssetLoanRepository(_dbContext));
        public IAssetPropertyRepository AssetPropertyRepo => _assetPropertyRepo ?? (_assetPropertyRepo = new AssetPropertyRepository(_dbContext));
        public IAssetTypeRepository AssetTypeRepo => _assetTypeRepo ?? (_assetTypeRepo = new AssetTypeRepository(_dbContext));
        public ILoanRepository LoanRepo => _loanRepo ?? (_loanRepo = new LoanRepository(_dbContext));
        public Domain.Loan.ILoanProductRepository LoanProductRepo => _loanProductRepo ?? (_loanProductRepo = new Loan.LoanProductRepository(_dbContext));
        public ILoanProductAttributeRepository LoanProductAttributeRepo => _loanProductAttributeRepo ?? (_loanProductAttributeRepo = new Loan.LoanProductAttributeRepository(_dbContext));
        public ILoanProductCategoryRepository LoanProductCategoryRepo => _loanProductCategoryRepo ?? (_loanProductCategoryRepo = new LoanProductCategoryRepository(_dbContext));
        public ILoanProductPropertyRepository LoanProductPropertyRepo => _loanProductPropertyRepo ?? (_loanProductPropertyRepo = new LoanProductPropertyRepository(_dbContext));

        public ILoyaltyPointRepository LoyaltyPointRepo => _loyaltyPointRepo ?? (_loyaltyPointRepo = new LoyaltyPointRepository(_dbContext));
        public ILoyaltyPointTypeRepository LoyaltyPointTypeRepo => _loyaltyPointTypeRepo ?? (_loyaltyPointTypeRepo = new LoyaltyPointTypeRepository(_dbContext));
        public IMemberLoyaltyTransactionRepository MemberLoyaltyTransactionRepo => _memberLoyaltyTransactionRepo ?? (_memberLoyaltyTransactionRepo = new MemberLoyaltyTransactionRepository(_dbContext));
        public IMemberRepository MemberRepo => _memberRepo ?? (_memberRepo = new MemberRepository(_dbContext));
        public IMemberWalletRepository MemberWalletRepo => _memberWalletRepo ?? (_memberWalletRepo = new MemberWalletRepository(_dbContext));
        public IPreferredBrandRepository PreferredBrandRepo => _preferredBrandRepo ?? (_preferredBrandRepo = new PreferredBrandRepository(_dbContext));
        public IPreferredProductRepository PreferredProductRepo => _preferredProductRepo ?? (_preferredProductRepo = new PreferredProductRepository(_dbContext));
        public ILoyaltyPointRuleRepository LoyaltyPointRuleRepo => _loyaltyPointRuleRepo ?? (_loyaltyPointRuleRepo = new LoyaltyPointRuleRepository(_dbContext));
        public ILoyaltyPointSettingRepository LoyaltyPointSettingRepo => _loyaltyPointSettingRepo ?? (_loyaltyPointSettingRepo = new LoyaltyPointSettingRepository(_dbContext));
        public ILoyaltyTierRepository LoyaltyTierRepo => _loyaltyTierRepo ?? (_loyaltyTierRepo = new LoyaltyTierRepository(_dbContext));
        public IMemberLoyaltyTierRepository MemberLoyaltyTierRepo => _memberLoyaltyTierRepo ?? (_memberLoyaltyTierRepo = new MemberLoyaltyTierRepository(_dbContext));
        //add new loyalty
        public IPointRuleStoresRepository PointRuleStoresRepo => _pointRuleStoresRepo ?? (_pointRuleStoresRepo = new PointRuleStoresRepository(_dbContext));
        public IPointRuleProductRepository PointRuleProductRepo => _pointRuleProductRepo ?? (_pointRuleProductRepo = new PointRuleProductRepository(_dbContext));
        public IPointRuleChannelRepository PointRuleChannelRepo => _pointRuleChannelRepo ?? (_pointRuleChannelRepo = new PointRuleChannelRepository(_dbContext));
        public IPointRuleCategoryRepository PointRuleCategoryRepo => _pointRuleCategoryRepo ?? (_pointRuleCategoryRepo = new PointRuleCategoryRepository(_dbContext));
        //public IPointRuleBrandRepository PointRuleBrandRepo => _pointRuleBrandRepo ?? (_pointRuleBrandRepo = new PointRuleBrandRepository(_dbContext));
        //add new ProductCollectionItem, ProductCollection
        public IProductCollectionRepository ProductCollectionRepo => _productCollectionRepo ?? (_productCollectionRepo = new ProductCollectionRepository(_dbContext));
        public IProductCollectionItemRepository ProductCollectionItemRepo => _productCollectionItemRepo ?? (_productCollectionItemRepo = new ProductCollectionItemRepository(_dbContext));


        public ICouponRepository CouponRepo => _couponRepo ?? (_couponRepo = new CouponRepository(_dbContext));
        public IMemberCouponRepository MemberCouponRepo => _memberCouponRepo ?? (_memberCouponRepo = new MemberCouponRepository(_dbContext));
        public IMemberVoucherRepository MemberVoucherRepo => _memberVoucherRepo ?? (_memberVoucherRepo = new MemberVoucherRepository(_dbContext));
        public IMemberGiftRepository MemberGiftRepo => _memberGiftRepo ?? (_memberGiftRepo = new MemberGiftRepository(_dbContext));
        public ICouponRedemptionRepository CouponRedemptionRepo => _CouponRedemptionRepo ?? (_CouponRedemptionRepo = new CouponRedemptionRepository(_dbContext));

        public IGiftRepository GiftRepo => _giftRepo ?? (_giftRepo = new GiftRepository(_dbContext));
        public IVoucherRepository VoucherRepo => _voucherRepo ?? (_voucherRepo = new VoucherRepository(_dbContext));

        public ICouponCategoryRepository CouponCategoryRepo => _couponCategoryRepo ?? (_couponCategoryRepo = new CouponCategoryRepository(_dbContext));
        public IGiftCategoryRepository GiftCategoryRepo => _giftCategoryRepo ?? (_giftCategoryRepo = new GiftCategoryRepository(_dbContext));
        public IVoucherCategoryRepository VoucherCategoryRepo => _voucherCategoryRepo ?? (_voucherCategoryRepo = new VoucherCategoryRepository(_dbContext));

        public IStoreRepository StoreRepo => _storeRepo ?? (_storeRepo = new StoreRepository(_dbContext));
        public IMemberChannelRepository MemberChannelRepo => _channelRepo ?? (_channelRepo = new MemberChannelRepository(_dbContext));
        //public IBrandRepository BrandRepo => _brandRepo ?? (_brandRepo = new BrandRepository(_dbContext));
        public IProductRepository ProductRepo => _productRepo ?? (_productRepo = new ProductRepository(_dbContext));
        public IProductOrderRepository ProductOrderRepo => _productOrderRepo ?? (_productOrderRepo = new ProductOrderRepository(_dbContext));
        public IProductAttributeRepository ProductAttributeRepo => _productAttributeRepo ?? (_productAttributeRepo = new Store.ProductAttributeRepository(_dbContext));
        public IProductCategoryRepository ProductCategoryRepo => _productCategoryRepo ?? (_productCategoryRepo = new ProductCategoryRepository(_dbContext));
        public IProductPropertyRepository ProductPropertyRepo => _productPropertyRepo ?? (_productPropertyRepo = new ProductPropertyRepository(_dbContext));
        public IUnitRepository UnitRepo => _unitRepo ?? (_unitRepo = new UnitRepository(_dbContext));
        public IPurchasedTransactionRepository PurchasedTransactionRepo => _purchasedTransactionRepo ?? (_purchasedTransactionRepo = new PurchasedTransactionRepository(_dbContext));

        public IMemberGroupRepository MemberGroupRepo => _memberGroupRepo ?? (_memberGroupRepo = new MemberGroupRepository(_dbContext));
        public IMemberGroupTagRepository MemberGroupTagRepo => _memberGroupTagRepo ?? (_memberGroupTagRepo = new MemberGroupTagRepository(_dbContext));
        public IMemberLifeCycleRepository MemberLifeCycleSettingRepo => _memberLifeCycleSettingRepo ?? (_memberLifeCycleSettingRepo = new MemberLifeCycleRepository(_dbContext));
        public IMemberSegmentRepository MemberSegmentRepo => _memberSegmentRepo ?? (_memberSegmentRepo = new MemberSegmentRepository(_dbContext));

        public IMemberTagRepository MemberTagRepo => _memberTagRepo ?? (_memberTagRepo = new MemberTagRepository(_dbContext));
        public IMemberTagCategoryRepository MemberTagCategoryRepo => _memberTagCategoryRepo ?? (_memberTagCategoryRepo = new MemberTagCategoryRepository(_dbContext));
        public IMemberTagValueRepository MemberTagValueRepo => _memberTagValueRepo ?? (_memberTagValueRepo = new MemberTagValueRepository(_dbContext));
        public ITagKeyRepository TagKeyRepo => _tagKeyRepo ?? (_tagKeyRepo = new TagKeyRepository(_dbContext));

        public IMemberProductQrCodeRepository MemberProductQrCodeRepo => _memberProductQrCodeRepo ?? (_memberProductQrCodeRepo = new MemberProductQrCodeRepository(_dbContext));

        public IClaimsReprository ClaimsReprository => _claimsRepro ?? (_claimsRepro = new ClaimsRepository(_dbContext));

        //public IMemberTaggingRepository MemberTaggingRepo => _memberTaggingRepo ?? (_memberTaggingRepo= new MemberTaggingRepository(_dbContext));

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_dbContext == null) return;
            _dbContext.Dispose();
            _dbContext = null;

            _profileRepository = null;
        }
    }
}

#endregion