using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.Admin;

namespace Omini.Opme.Infrastructure.Mappings;

internal class InternalSpecialistMapping : IEntityTypeConfiguration<InternalSpecialist>
{
    public void Configure(EntityTypeBuilder<InternalSpecialist> builder)
    {
        builder.HasKey(x => x.Id);

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
