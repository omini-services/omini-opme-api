using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Omini.Opme.Infrastructure.Extensions;
using Omini.Opme.Shared.Interfaces;
using Omini.Opme.Domain.Admin;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Domain.Exceptions;
using Omini.Opme.Domain.Entities;

namespace Omini.Opme.Infrastructure.Contexts;

internal sealed class OpmeContext : DbContext
{
    private readonly IClaimsService _claimsProvider;

    public OpmeContext(DbContextOptions<OpmeContext> options, IClaimsService claimsProvider)
        : base(options)
    {
        _claimsProvider = claimsProvider;
    }
    public DbSet<OpmeUser> OpmeUsers { get; set; }
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
            entry.Property(nameof(Auditable.CreatedOn)).CurrentValue = DateTime.UtcNow;
        }

        entry.Property(nameof(Auditable.UpdatedBy)).CurrentValue = userId;
        entry.Property(nameof(Auditable.UpdatedOn)).CurrentValue = DateTime.UtcNow;
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
