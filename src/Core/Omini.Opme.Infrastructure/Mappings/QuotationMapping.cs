using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Warehouse;

namespace Omini.Opme.Infrastructure.Mappings;

internal sealed class QuotationMapping : DocumentEntityMapping<Quotation>
{
    public override void Configure(EntityTypeBuilder<Quotation> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .UseIdentityColumn();

        builder.Property(x => x.PayingSourceType)
            .HasConversion(
                v => v.ToString(),
                v => (PayingSourceType)Enum.Parse(typeof(PayingSourceType), v))
            .IsRequired();

        builder.OwnsOne(x => x.PatientName)
            .Property(x => x.FirstName)
            .HasColumnName("PatientFirstName")
            .IsRequired();

        builder.OwnsOne(x => x.PatientName)
            .Property(x => x.MiddleName)
            .HasColumnName("PatientMiddleName");

        builder.OwnsOne(x => x.PatientName)
            .Property(x => x.LastName)
            .HasColumnName("PatientLastName")
            .IsRequired();

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientCode)
            .IsRequired();

        builder.OwnsOne(x => x.PhysicianName)
            .Property(x => x.FirstName)
            .HasColumnName("PhysicianFirstName")
            .IsRequired();

        builder.OwnsOne(x => x.PhysicianName)
            .Property(x => x.MiddleName)
            .HasColumnName("PhysicianMiddleName");

        builder.OwnsOne(x => x.PhysicianName)
            .Property(x => x.LastName)
            .HasColumnName("PhysicianLastName")
            .IsRequired();

        builder.HasOne<Physician>()
            .WithMany()
            .HasForeignKey(x => x.PhysicianCode)
            .IsRequired();

        builder.HasOne<Hospital>()
            .WithMany()
            .HasForeignKey(x => x.HospitalCode)
            .IsRequired();

        builder.HasOne<InsuranceCompany>()
            .WithMany()
            .HasForeignKey(x => x.InsuranceCompanyCode)
            .IsRequired();

        builder.Property(x => x.Comments)
            .HasMaxLength(1000);

        // builder.HasOne(x => x.InsuranceCompany)
        //     .WithMany()
        //     .HasForeignKey(x => x.InsuranceCompanyCode)
        //     .IsRequired();

        // builder.Property(x => x.InternalSpecialistCode)
        //     .HasMaxLength(50)
        //     .IsRequired();

        builder.ToTable("Quotations");
    }
}

internal class QuotationItemMapping : IEntityTypeConfiguration<QuotationItem>
{
    public void Configure(EntityTypeBuilder<QuotationItem> builder)
    {
        builder.HasKey(x => new { x.DocumentId, x.LineId });

        builder.HasOne<Quotation>()
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.DocumentId)
            .IsRequired();

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemCode)
            .IsRequired();

        builder.ToTable("QuotationItems");
    }
}