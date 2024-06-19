using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omini.Opme.Domain.Admin;
using Omini.Opme.Domain.Common;

internal abstract class MasterEntityMapping<T> : IEntityTypeConfiguration<T> where T : MasterEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasOne<OpmeUser>()
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .IsRequired();

        builder.HasOne<OpmeUser>()
            .WithMany()
            .HasForeignKey(x => x.UpdatedBy);
    }
}