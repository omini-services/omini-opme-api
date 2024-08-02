using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.BusinessPartners;
using Omini.Opme.Infrastructure.Extensions;

namespace Omini.Opme.Infrastructure.Mappings;

internal sealed class PatientMapping : MasterEntityMapping<Patient>
{
    public override void Configure(EntityTypeBuilder<Patient> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .HasDefaultValueSql($"nextval ('{ModelBuilderExtensions.PatientCodeSequence}')")
            .IsRequired();

        builder.OwnsOne(x => x.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("FirstName")
                .IsRequired();

            name.Property(n => n.MiddleName)
                .HasColumnName("MiddleName");

            name.Property(n => n.LastName)
                .HasColumnName("LastName")
                .IsRequired();
        });

        builder.Property(x => x.Cpf)
            .HasMaxLength(14);

        builder.ToTable("Patients");
    }
}
