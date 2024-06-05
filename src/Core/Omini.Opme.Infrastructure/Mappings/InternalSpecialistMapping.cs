using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.Admin;

namespace Omini.Opme.Infrastructure.Mappings;

internal class InternalSpecialistMapping : IEntityTypeConfiguration<InternalSpecialist>
{
    public void Configure(EntityTypeBuilder<InternalSpecialist> builder)
    {
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(x => x.Name)
            .Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .IsRequired();

        builder.OwnsOne(x => x.Name)
            .Property(x => x.MiddleName)
            .HasColumnName("MiddleName");

        builder.OwnsOne(x => x.Name)
            .Property(x => x.LastName)
            .HasColumnName("LastName")
            .IsRequired();

        builder.ToTable("InternalSpecialists");
    }
}
