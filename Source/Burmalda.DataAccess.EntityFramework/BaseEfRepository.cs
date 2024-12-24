using System.Linq.Expressions;
using Burmalda.Entities.Auctions;
using Burmalda.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Burmalda.DataAccess;

public abstract class BaseEfRepository<T>: IRepository<T> where T : class
{
    public BaseEfRepository(BurmaldaDbContext context)
    {
        _context = context;
    }
    protected BurmaldaDbContext _context { get; }
    
    public virtual async Task<T> AddAsync(T target, CancellationToken cancellationToken = default)
    {
        
        EntityEntry<T> entry = await _context.Set<T>().AddAsync(target, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public virtual async Task<T> UpdateAsync(T target, CancellationToken cancellationToken = default)
    {
        
        EntityEntry<T> entry = _context.Set<T>().Update(target);
        await _context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public virtual async Task DeleteAsync(T target, CancellationToken cancellationToken = default)
    {
        
        _context.Set<T>().Remove(target);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        
        return await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public virtual async Task<T[]> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        
        return await _context.Set<T>().Where(predicate).ToArrayAsync(cancellationToken);
    }
}