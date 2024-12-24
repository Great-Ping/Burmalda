using System.Linq.Expressions;
using Burmalda.Entities.Auctions;
using Burmalda.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Burmalda.DataAccess;

public abstract class BaseEfRepository<T>: IRepository<T> where T : class
{
    public BaseEfRepository(IDbContextFactory<BurmaldaDbContext> contextFactory)
    {
        ContextFactory = contextFactory;
    }
    protected IDbContextFactory<BurmaldaDbContext> ContextFactory { get; }
    
    public virtual async Task<T> AddAsync(T target, CancellationToken cancellationToken = default)
    {
        await using BurmaldaDbContext context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        EntityEntry<T> entry = await context.Set<T>().AddAsync(target, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public virtual async Task<T> UpdateAsync(T target, CancellationToken cancellationToken = default)
    {
        await using BurmaldaDbContext context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        EntityEntry<T> entry = context.Set<T>().Update(target);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public virtual async Task DeleteAsync(T target, CancellationToken cancellationToken = default)
    {
        await using BurmaldaDbContext context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        context.Set<T>().Remove(target);
        await context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await using BurmaldaDbContext context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public virtual async Task<T[]> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await using BurmaldaDbContext context = await ContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Set<T>().AsNoTracking().Where(predicate).ToArrayAsync(cancellationToken);
    }
}