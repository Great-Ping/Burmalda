using Burmalda.Entities.Auctions;

namespace Burmalda.Services.Users.Entities;

public record AuctionPreview(
    ulong Id,
    ulong OwnerId,
    string? Title,
    bool IsCompleted,
    DateTimeOffset StartDate,
    TimeSpan Duration,
    AuctionLotPreview? Winner
){
    public static AuctionPreview FromAuction(Auction auction)
    {
        return new AuctionPreview(
            auction.Id,
            auction.OwnerId,
            auction.Title,
            auction.IsCompleted,
            auction.StartDate,
            auction.Duration,
            (auction.Winner != null) ? AuctionLotPreview.FromLot(auction.Winner) : null
        );
    }
}