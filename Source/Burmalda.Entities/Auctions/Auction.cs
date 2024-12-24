using Burmalda.Entities.Users;

namespace Burmalda.Entities.Auctions;

public class Auction: BaseEntity
{
    public required string? Title { get; set; }
    public required bool IsCompleted { get; set; }
    public required TimeIncrementingStrategy TimeIncrementingStrategy { get; set; }
    
    public required DateTimeOffset StartDate { get; set; }
    public required TimeSpan Duration { get; set; }
    
    public required ulong? WinnerId { get; set; }
    public virtual required AuctionLot? Winner { get; set; }
    
    public required ulong OwnerId { get; set; }
    public virtual required Streamer? Owner { get; set; }
    
    public virtual  required List<AuctionLot>? Lots { get; set; }
}
