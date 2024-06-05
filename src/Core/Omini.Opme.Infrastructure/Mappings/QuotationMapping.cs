using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.Common;
using Omini.Opme.Domain.Sales;
using Omini.Opme.Domain.Warehouse;

namespace Omini.Opme.Infrastructure.Mappings;

internal class QuotationMapping : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .UseIdentityColumn();

        builder.Property(x => x.PayingSourceType)
            .HasConversion(
                v => v.ToString(),
                v => (PayingSourceType)Enum.Parse(typeof(PayingSourceType), v))
            .IsRequired();

        builder.Ignore(p => p.PayingSource);

        builder.Property(x => x.PayingSourceCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(x => x.Patient)
            .WithMany()
            .HasForeignKey(x => x.PatientCode)
            .IsRequired();

        builder.HasOne(x => x.Physician)
            .WithMany()
            .HasForeignKey(x => x.PhysicianCode)
            .IsRequired();

        builder.HasOne(x => x.Hospital)
            .WithMany()
            .HasForeignKey(x => x.HospitalCode)
            .IsRequired();  

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
        builder.HasKey(x => new { x.QuotationId, x.LineId });

        builder.HasOne<Quotation>()
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.QuotationId)
            .IsRequired();

        builder.HasOne<Item>()
            .WithMany()
            .HasForeignKey(x => x.ItemCode)
            .IsRequired();

        builder.ToTable("QuotationItems");
    }
}