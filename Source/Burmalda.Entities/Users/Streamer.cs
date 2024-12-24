using Burmalda.Entities.Auctions;

namespace Burmalda.Entities.Users;

public class Streamer : User
{
    public virtual required List<User>? Moderators { get; set; }
    public virtual required List<Auction>? Auctions { get; set; }
}