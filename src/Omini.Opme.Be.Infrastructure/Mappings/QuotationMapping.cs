using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Be.Domain.Entities;
using Omini.Opme.Be.Domain.Enums;

internal class QuotationMapping : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.PayingSourceType)
            .HasConversion(
                v => v.ToString(),
                v => (PayingSourceType)Enum.Parse(typeof(PayingSourceType), v))
            .IsRequired();

        builder.Property(x => x.PayingSourceId)
            .IsRequired();

        builder.HasOne<Patient>()
            .WithMany()
            .HasForeignKey(x => x.PatientId)
            .IsRequired();

        builder.HasOne<Physician>()
            .WithMany()
            .HasForeignKey(x => x.PhysicianId)
            .IsRequired();

        builder.HasOne<Hospital>()
            .WithMany()
            .HasForeignKey(x => x.HospitalId)
            .IsRequired();

        builder.HasOne<InsuranceCompany>()
            .WithMany()
            .HasForeignKey(x => x.InsuranceCompanyId)
            .IsRequired();

        //------
        // builder.HasOne<Patient>()
        //     .WithOne()
        //     .HasForeignKey<Quotation>(x => x.PayingSourceId);

        // builder.HasOne<InsuranceCompany>()
        //     .WithOne()
        //     .HasForeignKey<Quotation>(x => x.PayingSourceId);

        // builder.HasOne<Physician>()
        //     .WithOne()
        //     .HasForeignKey<Quotation>(x => x.PayingSourceId);

        // builder.HasOne<Hospital>()
        //     .WithOne()
        //     .HasForeignKey<Quotation>(x => x.PayingSourceId);
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
            .HasForeignKey(x => x.ItemId)
            .IsRequired();

        builder.ToTable("QuotationItems");
    }
}