using Burmalda.Entities.Auctions;
using Burmalda.Entities.Donation;

namespace Burmalda.Entities.Users;

public class User: BaseEntity
{
    public required string Nickname { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required UserPermissions Permissions { get; set; }
    public virtual required List<Streamer>? Subscriptions { get; set; }
    public virtual required List<Donate>? Donates { get; set; }
    public required UserAccount Account { get; set; }
    public virtual required List<Streamer>? ModeratedStreamers { get; set; }
}
