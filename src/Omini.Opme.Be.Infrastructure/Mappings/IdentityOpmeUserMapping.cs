using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Be.Domain.Entities;

internal class IdentityOpmeUserMapping : IEntityTypeConfiguration<IdentityOpmeUser>
{
    public void Configure(EntityTypeBuilder<IdentityOpmeUser> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x=>x.Email)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.ToTable("IdentityOpmeUsers");
    }
}
