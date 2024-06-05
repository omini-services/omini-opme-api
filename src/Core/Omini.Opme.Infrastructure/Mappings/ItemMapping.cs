using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.Warehouse;

namespace Omini.Opme.Infrastructure.Mappings;

internal class ItemMapping : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SalesName)            
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Uom)            
            .HasMaxLength(100);

        builder.Property(x => x.AnvisaCode)            
            .HasMaxLength(100);            

        builder.Property(x => x.SupplierCode)            
            .HasMaxLength(100);

        builder.Property(x => x.Cst)            
            .HasMaxLength(100);

        builder.Property(x => x.SusCode)            
            .HasMaxLength(100);

        builder.Property(x => x.NcmCode)            
            .HasMaxLength(100);

        builder.ToTable("Items");
    }
}
