using Burmalda.Entities.Auctions;
using Burmalda.Entities.Users;

namespace Burmalda.Entities.Donation;

public class AuctionBet: BaseEntity
{
    public required Decimal Amount { get; set; }
    public required bool IsFake { get; set; }
    
    public required ulong LotId { get; set; } 
    public virtual required AuctionLot? Lot { get; set; }
    
    public required ulong? OwnerId { get; set; }
    public virtual required User? Owner { get; set; }
    
    public required ulong DonateId { get; set; }
    public virtual required Donate Donate { get; set; }
}