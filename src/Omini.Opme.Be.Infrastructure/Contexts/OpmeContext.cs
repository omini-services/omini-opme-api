
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Omini.Opme.Be.Application;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Services;
using Omini.Opme.Be.Shared.Entities;
using Omini.Opme.Be.Shared.Interfaces;

namespace Omini.Opme.Be.Infrastructure.Contexts
{
    public class OpmeContext : DbContext, IOpmeContext
    {
        private readonly IClaimsProvider _claimsProvider;

        public OpmeContext(DbContextOptions<OpmeContext> options, IClaimsProvider claimsProvider)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
            _claimsProvider = claimsProvider;
        }
        public DbSet<IdentityOpmeUser> IdentityOpmeUsers { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Ignore<Notification>();
            builder.ApplyConfigurationsFromAssembly(typeof(OpmeContext).Assembly);

            builder.Entity<Item>().HasQueryFilter(p => !p.IsDeleted);
            //builder.Entity<ExpenseReport>().HasQueryFilter(p => p.CompanyId == _userService.GetCompanyId() && p.CreatedBy == _userService.UserId);

            base.OnModelCreating(builder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<string>()
                .HaveMaxLength(100);

            base.ConfigureConventions(configurationBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _claimsProvider.GetUserId();

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().BaseType == typeof(Auditable)))
            {
                SetAuditable(userId, entry);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private static void SetAuditable(Guid userId, EntityEntry entry)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(Auditable.CreatedBy)).CurrentValue = userId;
                entry.Property(nameof(Auditable.CreatedDate)).CurrentValue = DateTime.UtcNow;
            }

            entry.Property(nameof(Auditable.LastModifiedBy)).CurrentValue = userId;
            entry.Property(nameof(Auditable.LastModified)).CurrentValue = DateTime.UtcNow;
        }

        // private void UpdateCompanyId(EntityEntry entry)
        // {
        //     if (ShouldUpdateCompanyId(entry))
        //     {
        //         if (entry.State == EntityState.Added)
        //             entry.Property("CompanyId").CurrentValue = _userService.GetCompanyId();
        //         else if (entry.State == EntityState.Modified)
        //             entry.Property("CompanyId").IsModified = false;
        //     }
        // }

        // private static bool ShouldUpdateCompanyId(EntityEntry entry)
        // {
        //     return entry.Entity.GetType().GetProperty("CompanyId") != null;
        // }
    }
}
