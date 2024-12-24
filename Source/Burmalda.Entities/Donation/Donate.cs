using Burmalda.Entities.Auctions;
using Burmalda.Entities.Users;

namespace Burmalda.Entities.Donation;

public class Donate: BaseEntity
{
    public required bool IsPaid { get; set; }
    public required decimal Amount { get; set; }
    public required string Message { get; set; }

    public required ulong DonatorId { get; set; }
    public virtual required User Donator { get; set; }
    
    public ulong? BetId { get; set; }
    public virtual required AuctionBet? Bet { get; set; }

}