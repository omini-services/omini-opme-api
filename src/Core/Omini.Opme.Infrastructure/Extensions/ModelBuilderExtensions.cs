using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Entities;
namespace Omini.Opme.Infrastructure.Extensions;

internal static class ModelBuilderExtensions
{
    public static void EnableSoftDelete(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes()
            .Where(e => e.ClrType.IsSubclassOf(typeof(Auditable))))
        {
            var param = Expression.Parameter(entityType.ClrType, "entity");
            var prop = Expression.PropertyOrField(param, nameof(Auditable.IsDeleted));
            var entityNotDeleted = Expression.Lambda(Expression.Equal(prop, Expression.Constant(false)), param);

            entityType.SetQueryFilter(entityNotDeleted);
        }
    }
}