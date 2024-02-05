using Common.Utilities;
using Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IConfiguration _configRoot;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IDataProtectionProvider dataProtectionProvider,
            IConfiguration configRoot) : base(options)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _configRoot = configRoot;
            DbInitializer.Initialize(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IBaseEntity).Assembly;

            modelBuilder.RegisterAllEntities<IBaseEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.ConfigureIdentityTableName();
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddPluralizingTableNameConvention();
            modelBuilder.AddColumnProtector(_dataProtectionProvider, _configRoot);
        }
    }
}
