using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Omini.Opme.Domain.Common;

namespace Omini.Opme.Infrastructure.Extensions;

internal static class ModelBuilderExtensions
{
    public static void EnableSoftDelete(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes()
            .Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType)))
        {
            var param = Expression.Parameter(entityType.ClrType, "entity");
            var prop = Expression.PropertyOrField(param, nameof(ISoftDeletable.IsDeleted));
            var entityNotDeleted = Expression.Lambda(Expression.Equal(prop, Expression.Constant(false)), param);

            entityType.SetQueryFilter(entityNotDeleted);
        }
    }

    public static void ApplyDefaultRules(this ModelBuilder builder)
    {
        var notIncludedEntities = new string[] { "Item" };
        var notIncludedFields = new string[] { "Code" };

        var stringProperties = builder.Model.GetEntityTypes()
           .Where(t => !notIncludedEntities.Contains(t.ClrType.Name))
           .SelectMany(t => t.GetProperties())
           .Where(p => p.ClrType == typeof(string) && !notIncludedFields.All(n => p.Name.EndsWith(n)));

        foreach (var property in stringProperties)
            property.SetMaxLength(100);
    }
}