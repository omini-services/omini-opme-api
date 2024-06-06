using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Infrastructure.Extensions;

namespace Omini.Opme.Infrastructure.Mappings;

internal class PatientMapping : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .HasDefaultValueSql($"nextval ('{ModelBuilderExtensions.PatientCodeSequence}')")
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

        builder.Property(x=>x.Cpf)
            .HasMaxLength(14);

        builder.ToTable("Patients");
    }
}
