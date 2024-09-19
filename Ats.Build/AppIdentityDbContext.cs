using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ats.Domain.Identity.Models;
using Ats.Domain.Accounts.Models;
using Ats.Domain.Departments.Models;
using Ats.Domain.Address;
using Ats.Domain.Address.Models;
using Ats.Data.EntityFramework;

namespace Ats.Build
{
    public class AppIdentityDbContext : SCDataContext
    {
        public AppIdentityDbContext(DbContextOptions<SCDataContext> options) : base(options, null)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
