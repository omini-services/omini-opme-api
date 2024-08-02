using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Infrastructure.Extensions;

namespace Omini.Opme.Infrastructure.Mappings;

internal sealed class HospitalMapping : MasterEntityMapping<Hospital>
{
    public override void Configure(EntityTypeBuilder<Hospital> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .HasDefaultValueSql($"nextval ('{ModelBuilderExtensions.HospitalCodeSequence}')")
            .IsRequired();

        builder.OwnsOne(x => x.Name, name => {
            name.Property(n => n.TradeName)
                .HasColumnName("TradeName")
                .IsRequired();

            name.Property(n => n.LegalName)
                .HasColumnName("LegalName")
                .IsRequired();
        });

        builder.Property(x => x.Cnpj)
            .HasMaxLength(18);

        builder.ToTable("Hospitals");
    }
}
