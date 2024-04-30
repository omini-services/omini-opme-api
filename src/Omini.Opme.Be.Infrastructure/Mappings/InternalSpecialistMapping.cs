using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Be.Domain.Entities;

internal class InternalSpecialistMapping : IEntityTypeConfiguration<InternalSpecialist>
{
    public void Configure(EntityTypeBuilder<InternalSpecialist> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name)
            .Property(x => x.FirstName)
            .IsRequired();

        builder.OwnsOne(x => x.Name)
            .Property(x => x.LastName)
            .IsRequired();

        builder.ToTable("InternalSpecialists");
    }
}
