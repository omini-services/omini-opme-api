using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.Warehouse;
using Omini.Opme.Infrastructure.Extensions;

namespace Omini.Opme.Infrastructure.Mappings;

internal sealed class ItemMapping : MasterEntityMapping<Item>
{
    public override void Configure(EntityTypeBuilder<Item> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .HasDefaultValueSql($"nextval ('{ModelBuilderExtensions.ItemCodeSequence}')")
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
