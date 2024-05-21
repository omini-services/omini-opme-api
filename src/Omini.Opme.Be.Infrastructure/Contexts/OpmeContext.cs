using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Exceptions;
using Omini.Opme.Be.Infrastructure.Extensions;
using Omini.Opme.Be.Shared.Interfaces;

namespace Omini.Opme.Be.Infrastructure.Contexts;

internal sealed class OpmeContext : DbContext
{
    private readonly IClaimsService _claimsProvider;

    public OpmeContext(DbContextOptions<OpmeContext> options, IClaimsService claimsProvider)
        : base(options)
    {
        _claimsProvider = claimsProvider;
    }
    public DbSet<IdentityOpmeUser> IdentityOpmeUsers { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Physician> Physicians { get; set; }
    public DbSet<InternalSpecialist> InternalSpecialists { get; set; }
    public DbSet<Quotation> Quotations { get; set; }
    public DbSet<QuotationItem> QuotationItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.Ignore<Notification>();
        builder.ApplyConfigurationsFromAssembly(typeof(OpmeContext).Assembly);

        builder.EnableSoftDelete();

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
        var opmeUserId = _claimsProvider.OpmeUserId;

        if (opmeUserId is null)
        {
            throw new InvalidUserException();
        }

        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().BaseType == typeof(Auditable)))
        {
            SetAuditable(opmeUserId.Value, entry);
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
