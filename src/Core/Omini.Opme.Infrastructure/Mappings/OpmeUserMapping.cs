using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.Authentication;

namespace Omini.Opme.Infrastructure.Mappings;

internal sealed class IdentityOpmeUserMapping : IEntityTypeConfiguration<OpmeUser>
{
    public void Configure(EntityTypeBuilder<OpmeUser> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(x=>x.CompanyId);

        builder.ToTable("OpmeUsers");
    }
}
