using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using PriceAggregator.Data.Context.Entities;

namespace PriceAggregator.Data.Context.Extensions;

public static class DbSetExtensions
{
    public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : KeyEntity, new()
    {
        var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any(x => x.Id == entity.Id);
        return !exists ? dbSet.Add(entity).Entity : null;
    }

}