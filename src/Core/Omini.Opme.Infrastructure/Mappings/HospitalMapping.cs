using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.BusinessPartners;

namespace Omini.Opme.Infrastructure.Mappings;

internal class HospitalMapping : IEntityTypeConfiguration<Hospital>
{
    public void Configure(EntityTypeBuilder<Hospital> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name)
            .Property(x => x.TradeName)
            .HasColumnName("TradeName")
            .IsRequired();

        builder.OwnsOne(x => x.Name)
            .Property(x => x.LegalName)
            .HasColumnName("LegalName")
            .IsRequired();

        builder.Property(x => x.Cnpj)
            .HasMaxLength(18);

        builder.ToTable("Hospitals");
    }
}
