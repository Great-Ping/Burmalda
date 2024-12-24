using Burmalda.Entities.Donation;

namespace Burmalda.Entities.Auctions;

public class AuctionLot: BaseEntity
{
    public required string Title { get; set; }
    public virtual required List<AuctionBet>? Bets { get; set; }
    public required ulong AuctionId { get; set; }
    public virtual required Auction? Auction { get; set; }
}