using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Address;
using Ats.Domain.Address.Models;
using Ats.Domain.Loyalty;
using Ats.Domain.Loyalty.Models;
using Ats.Domain.Departments.Models;
using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Organization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ats.Domain.Member.Models;
using Ats.Domain.Store.Models;
using Ats.Domain.Channel.Models;
using Ats.Domain.Coupon.Models;
using Ats.Domain.Gift.Models;
using Ats.Domain.Voucher.Models;
using Ats.Domain.Claims;
using System.Security.Claims;
using Ats.Domain.Claims.Models;

namespace Ats.Data.EntityFramework
{
    public class SCDataContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>//, ISCDataContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Contructor

        public SCDataContext(DbContextOptions<SCDataContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //IMPORTANT: must to declare key for table. use HasKey() or [Key] in each table

            modelBuilder.Entity<User>().ToTable("users").HasKey(x => x.Id);
            modelBuilder.Entity<Role>().ToTable("roles").HasKey(x => x.Id);
            modelBuilder.Entity<RoleClaim>().ToTable("roleclaims").HasKey(x => x.Id);
            modelBuilder.Entity<UserClaim>().ToTable("userclaims").HasKey(x => x.Id);
            modelBuilder.Entity<UserLogin>().ToTable("userlogins").HasKey(x => x.UserId);
            modelBuilder.Entity<UserRole>().ToTable("userroles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<UserToken>().ToTable("usertokens").HasKey(x => x.UserId);

            modelBuilder.Entity<UserGroup>()
                 .ToTable("usergroups")
                 .HasKey(c => new { c.GroupId, c.UserId });
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            modelBuilder.Entity<GroupRole>()
                .ToTable("grouproles")
                .HasKey(c => new { c.GroupId, c.RoleId });
            modelBuilder.Entity<GroupRole>()
                .HasOne(gr => gr.Group)
                .WithMany(a => a.GroupRoles)
                .HasForeignKey(gr => gr.GroupId);
            modelBuilder.Entity<GroupRole>()
                .HasOne(gr => gr.Role)
                .WithMany(a => a.GroupRoles)
                .HasForeignKey(gr => gr.RoleId);

            modelBuilder.Entity<MemberTagging>()
                .ToTable("membertaggings")
                .HasKey(c => new { c.MemberId, c.MemberTagId});
            modelBuilder.Entity<MemberTagging>()
                .HasOne(gr => gr.Member)
                .WithMany(a => a.Tags)
                .HasForeignKey(gr => gr.MemberId);
       

            modelBuilder.Entity<MemberGroupLink>()
                .ToTable("membergrouplinks")
                .HasKey(c => new { c.MemberId, c.MemberGroupId});
            modelBuilder.Entity<MemberGroupLink>()
                .HasOne(gr => gr.Member)
                .WithMany(a => a.MemberGroupLinks)
                .HasForeignKey(gr => gr.MemberId);
            modelBuilder.Entity<MemberGroupLink>()
                .HasOne(gr => gr.MemberGroup)
                .WithMany(a => a.MemberGroupLinks)
                .HasForeignKey(gr => gr.MemberGroupId);

            modelBuilder.Entity<MemberSegmentLink>()
                .ToTable("membersegmentlinks")
                .HasKey(c => new { c.MemberId, c.MemberSegmentId});
            modelBuilder.Entity<MemberSegmentLink>()
                .HasOne(gr => gr.Member)
                .WithMany(a => a.MemberSegmentLinks)
                .HasForeignKey(gr => gr.MemberId);
            modelBuilder.Entity<MemberSegmentLink>()
                .HasOne(gr => gr.MemberSegment)
                .WithMany(a => a.MemberSegmentLinks)
                .HasForeignKey(gr => gr.MemberSegmentId);

            modelBuilder.Entity<MemberLifeCycleLink>()
                .ToTable("memberlifecyclelinks")
                .HasKey(c => new { c.MemberId, c.MemberLifeCycleId });
            modelBuilder.Entity<MemberLifeCycleLink>()
                .HasOne(gr => gr.Member)
                .WithMany(a => a.MemberLifeCycleLinks)
                .HasForeignKey(gr => gr.MemberId);
            modelBuilder.Entity<MemberLifeCycleLink>()
                .HasOne(gr => gr.MemberLifeCycle)
                .WithMany(a => a.MemberLifeCycleLinks)
                .HasForeignKey(gr => gr.MemberLifeCycleId);

            // Need to review these below to remove
            modelBuilder.Entity<Lead>().HasIndex(x=>x.LeadId).IsUnique();

            modelBuilder.Entity<Product>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();


            modelBuilder.Entity<PointRuleProduct>()
               .HasOne(s => s.LoyaltyPointRule)
               .WithMany(c=> c.PointRuleProducts)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<PointRuleStore>()
              .HasOne(s => s.LoyaltyPointRule)
              .WithMany(c => c.PointRuleStores)
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PointRuleBrand>()
                .HasOne(s => s.LoyaltyPointRule)
                .WithMany(c => c.PointRuleBrands)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<PointRuleCategory>()
                .HasOne(s => s.LoyaltyPointRule)
                .WithMany(c => c.PointRuleCategories)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PointRuleChannel>()
                .HasOne(s => s.LoyaltyPointRule)
                .WithMany(c => c.PointRuleChannels)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion Constructor

        #region DECLARE TABLES        
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Address> Address { get; set; }

        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupRole> GroupRole { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleClaim> RoleClaim { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserClaim> UserClaim { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserToken> UserToken { get; set; }

        public virtual DbSet<Profile> Profile { get; set; }

        public virtual DbSet<Wallet> Wallet { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }

        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DepartmentUser> DepartmentUser { get; set; }

        public virtual DbSet<JobTitle> JobTitle { get; set; }
        public virtual DbSet<Occupation> Occupation { get; set; }

        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamUser> TeamUser { get; set; }

        public virtual DbSet<Lead> Lead { get; set; }
        public virtual DbSet<LeadAccessLevelHistory> LeadAccessLevelHistory { get; set; }
        public virtual DbSet<LeadLevelHistory> LeadLevelHistory { get; set; }
        public virtual DbSet<LeadExchangeRequest> LeadExchangeRequest { get; set; }
        public virtual DbSet<LeadStatusHistory> LeadStatusHistory { get; set; }
        public virtual DbSet<LeadTeam> LeadTeam { get; set; }
        public virtual DbSet<LeadUser> LeadUser { get; set; }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Debt> Debt { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Income> Income { get; set; }
        public virtual DbSet<LoanApplication> LoanApplication { get; set; }
        public virtual DbSet<Domain.Lead.Models.LoanPackage> LoanPackage { get; set; }
        public virtual DbSet<LoanNote> LoanNote { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }

        public virtual DbSet<IncomeStream> IncomeStream { get; set; }
        public virtual DbSet<IncomeAmount> IncomeAmount { get; set; }
        public virtual DbSet<OriginalFile> OriginalFile { get; set; }
        public virtual DbSet<OriginalFileAddition> OriginalFileAddition { get; set; }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<LoanBook> LoanBook { get; set; }
        public virtual DbSet<Borrower> Borrower { get; set; }

        public virtual DbSet<BorrowerAccessLevelHistory> BorrowerAccessLevelHistory { get; set; }
        public virtual DbSet<BorrowerLevelHistory> BorrowerLevelHistory { get; set; }
        public virtual DbSet<BorrowerStatusHistory> BorrowerStatusHistory { get; set; }

        public virtual DbSet<Asset> Asset { get; set; }
        public virtual DbSet<AssetAttribute> AssetAttribute { get; set; }
        public virtual DbSet<AssetLoan> AssetLoan { get; set; }
        public virtual DbSet<AssetProperty> AssetProperty { get; set; }
        public virtual DbSet<AssetType> AssetType { get; set; }
        public virtual DbSet<Loan> Loan { get; set; }
        public virtual DbSet<LoanProduct> LoanProduct { get; set; }
        public virtual DbSet<LoanProductAttribute> LoanProductAttribute { get; set; }
        public virtual DbSet<LoanProductCategory> LoanProductCategory { get; set; }
        public virtual DbSet<LoanProductProperty> LoanProductProperty { get; set; }
        public virtual DbSet<Warehouse> Warehouse { get; set; }

        public virtual DbSet<LoyaltyPoint> LoyaltyPoint { get; set; }
        public virtual DbSet<LoyaltyPointType> LoyaltyPointType { get; set; }
        public virtual DbSet<MemberLoyaltyTransaction> MemberLoyaltyTransaction { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberWallet> MemberWallet { get; set; }
        public virtual DbSet<PreferredBrand> PreferredBrand { get; set; }
        public virtual DbSet<PreferredProduct> PreferredProduct { get; set; }

        public virtual DbSet<LoyaltyPointRule> LoyaltyPointRule { get; set; }
        public virtual DbSet<LoyaltyPointSetting> LoyaltyPointSetting { get; set; }
        public virtual DbSet<LoyaltyTier> LoyaltyTier { get; set; }
        public virtual DbSet<MemberLoyaltyTier> MemberLoyaltyTier { get; set; }

        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<MemberChannel> MemberChannel { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductAttribute> ProductAttribute { get; set; }
        public virtual DbSet<ProductProperty> ProductProperty { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<PurchasedTransaction> PurchasedTransaction { get; set; }

        public virtual DbSet<ProductOrder> ProductOrder { get; set; }
        public virtual DbSet<ProductCollectionItem> ProductCollectionItem { get; set; }
        public virtual DbSet<ProductCollection> ProductCollection { get; set; }
   

        public virtual DbSet<MemberTag> MemberTag { get; set; }
        public virtual DbSet<MemberTagCategory> MemberTagCategory { get; set; }

        public virtual DbSet<TagKey> TagKey { get; set; }
        public virtual DbSet<Domain.Member.Models.MemberGroup> MemberGroup { get; set; }
        public virtual DbSet<MemberLifeCycle> MemberLifeCycle { get; set; }
        public virtual DbSet<MemberTagValue> MemberTagValue { get; set; }
        public virtual DbSet<MemberGroupTag> MemberGroupTag { get; set; }
        public virtual DbSet<Domain.Member.Models.MemberSegment> MemberSegment { get; set; }

        public virtual DbSet<MemberTagging> MemberTagging { get; set; }
        public virtual DbSet<MemberGroupLink> MemberGroupLink { get; set; }
        public virtual DbSet<MemberSegmentLink> MemberSegmentLink { get; set; }
        public virtual DbSet<MemberLifeCycleLink> MemberLifeCycleLink { get; set; }

        public virtual DbSet<Coupon> Coupon { get; set; }
        public virtual DbSet<MemberCoupon> MemberCoupon { get; set; }
        public virtual DbSet<CouponRedemption> CouponRedemption { get; set; }
        public virtual DbSet<Gift> Gift { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }

        public virtual DbSet<CouponCategory> CouponCategory { get; set; }
        public virtual DbSet<GiftCategory> GiftCategory { get; set; }
        public virtual DbSet<VoucherCategory> VoucherCategory { get; set; }


        //Loyalty Point Rule
        public virtual DbSet<PointRuleBrand> PointRuleBrand { get; set; }
        public virtual DbSet<PointRuleCategory> PointRuleCategory { get; set; }
        public virtual DbSet<PointRuleProduct> PointRuleProduct { get; set; }
        public virtual DbSet<PointRuleStore> PointRuleStore { get; set; }
        public virtual DbSet<PointRuleChannel> PointRuleChannel { get; set; }

        //Member Voucher
        public virtual DbSet<MemberVoucher> MemberVoucher { get; set; }
        //Member Gift
        public virtual DbSet<MemberGift> MemberGift { get; set; }


        public virtual DbSet<MemberProductQrCode> MemberProductQrCode { get; set; }

        public virtual DbSet<Claims> Claims { get; set; }



        #endregion

        #region Extension
        public TEntity FindById<TEntity>(params object[] ids) where TEntity : class
        {
            return base.Set<TEntity>().Find(ids);
        }
        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class
        {
            var result = base.Set<TEntity>().Add(entity).Entity;

            var creationTrackingEntity = entity as IEntityTrackingCreation;
            if (creationTrackingEntity != null)
            {
                creationTrackingEntity.DateCreated = DateTime.UtcNow;
            }

            //((IObjectState)entity).State = ObjectState.Added;
            return result;
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            base.Set<TEntity>().Attach(entity);

            var modifyTrackingEntity = entity as IEntityTrackingModified;
            if (modifyTrackingEntity != null)
            {
                modifyTrackingEntity.DateModified = DateTime.UtcNow;
            }

            //((IObjectState)entity).State = ObjectState.Modified;
        }

        public void Update<TEntity, TKey>(TEntity entity, params Expression<Func<TEntity, object>>[] properties) where TEntity : class, IEntity<TKey>
        {
            //base.Set<TEntity>().Attach(entity);
            //DbEntityEntry<TEntity> entry = base.Entry(entity);

            //foreach (var selector in properties)
            //{
            //    entry.Property(selector).IsModified = true;
            //}

            Dictionary<object, object> originalValues = new Dictionary<object, object>();
            TEntity entityToUpdate = base.Set<TEntity>().Find(entity.Id);

            foreach (var property in properties)
            {
                var val = base.Entry(entityToUpdate).Property(property).OriginalValue;
                originalValues.Add(property, val);
            }

            //base.Entry(entityToUpdate).State = EntityState.Detached;

            //base.Entry(entity).State = EntityState.Unchanged;
            foreach (var property in properties)
            {
                base.Entry(entity).Property(property).OriginalValue = originalValues[property];
                base.Entry(entity).Property(property).IsModified = true;
            }
        }

        public void Delete<TEntity>(params object[] ids) where TEntity : class
        {
            var entity = FindById<TEntity>(ids);
            //((IObjectState)entity).State = ObjectState.Deleted;
            Delete(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            base.Set<TEntity>().Attach(entity);
            base.Set<TEntity>().Remove(entity);
        }
        public void Delete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            base.Set<TEntity>().RemoveRange(entities);
        }

        public int Execute(string sqlCommand)
        {
            return Database.ExecuteSqlCommand(sqlCommand);
        }

        public int Execute(string sqlCommand, params object[] args)
        {
            var result = Database.ExecuteSqlCommand(sqlCommand, args);
            return result;
        }
        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (!(entry.Entity is IAudit audit)) continue;
                var authenticatedUserId = _httpContextAccessor?.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (String.IsNullOrEmpty(authenticatedUserId))
                {
                    authenticatedUserId = _httpContextAccessor?.HttpContext?.User?.Identity.Name;
                }

                var now = DateTime.UtcNow;
                switch (entry.State)
                {
                    case EntityState.Modified:
                        audit.ChangedTimestamp = now;
                        audit.ChangedUserId = authenticatedUserId;
                        break;

                    case EntityState.Added:
                        audit.AddedTimestamp = now;
                        audit.AddedUserId = authenticatedUserId;
                        break;
                }
            }
        }

        #endregion

        #region Dispose
        private bool _disposed;

        public override void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        // This is overridden to prevent someone from calling SaveChanges without specifying the user making the change
        #endregion Dispose

    }


}