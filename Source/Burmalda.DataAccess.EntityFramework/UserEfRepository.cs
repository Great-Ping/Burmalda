using System.Linq.Expressions;
using Burmalda.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Burmalda.DataAccess;

public class UsersEfRepository : BaseEfRepository<User>, IUsersRepository
{
    public UsersEfRepository(
        BurmaldaDbContext contextFactory
    ) : base(contextFactory) {
    }

    public async Task<Streamer?> FindStreamerAsync(Expression<Func<Streamer, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Streamers.FirstOrDefaultAsync(predicate, cancellationToken);
    }
}