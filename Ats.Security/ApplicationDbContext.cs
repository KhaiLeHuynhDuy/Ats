using Ats.Domain.Accounts.Models;
using Ats.Domain.Identity.Models;
using Ats.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ats.Security.Identity
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleClaim>().ToTable("roleclaims");
            modelBuilder.Entity<UserClaim>().ToTable("userclaims");
            modelBuilder.Entity<UserLogin>().ToTable("userlogins");
            modelBuilder.Entity<UserRole>().ToTable("userroles");
            modelBuilder.Entity<UserToken>().ToTable("usertokens");

            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("roles");
            });
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("users");
            });

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

            
        }

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
    }
}
