using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Be.Domain.Entities;

internal class PhysicianMapping : IEntityTypeConfiguration<Physician>
{
    public void Configure(EntityTypeBuilder<Physician> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name)
            .Property(x => x.FirstName)
            .HasColumnName("FirstName")
            .IsRequired();

        builder.OwnsOne(x => x.Name)
            .Property(x => x.MiddleName)
            .HasColumnName("MiddleName")
            .IsRequired();

        builder.OwnsOne(x => x.Name)
            .Property(x => x.LastName)
            .HasColumnName("LastName")
            .IsRequired();

        builder.ToTable("Physicians");
    }
}
