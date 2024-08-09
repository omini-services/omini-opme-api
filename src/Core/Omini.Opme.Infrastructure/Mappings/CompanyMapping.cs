using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.Authentication;

namespace Omini.Opme.Infrastructure.Mappings;

internal sealed class CompanyMapping : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(x => x.Name, name => {
            name.Property(n => n.TradeName)
                .HasColumnName("TradeName")
                .IsRequired();

            name.Property(n => n.LegalName)
                .HasColumnName("LegalName")
                .IsRequired();
        });    

        builder.ToTable("Companies");
    }
}
