using System.Linq.Expressions;
using Burmalda.Entities.Auctions;
using Burmalda.Entities.Users;

namespace Burmalda.DataAccess;

public interface IUsersRepository : IRepository<User>
{
    Task<Streamer?> FindStreamerAsync(Expression<Func<Streamer, bool>> predicate, CancellationToken cancellationToken = default);
}