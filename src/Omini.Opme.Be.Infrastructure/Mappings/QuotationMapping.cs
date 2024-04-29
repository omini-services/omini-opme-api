using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Be.Domain.Entities;

internal class QuotationMapping : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.ToTable("Quotations");
    }
}

internal class QuotationItemMapping : IEntityTypeConfiguration<QuotationItem>
{
    public void Configure(EntityTypeBuilder<QuotationItem> builder)
    {
        builder.HasKey(x => new { x.QuotationId, x.LineId });

        builder.HasOne<Quotation>()
               .WithMany(x => x.Items)
               .HasForeignKey(x=> x.QuotationId)
               .IsRequired();

        builder.ToTable("QuotationItems");
    }
}