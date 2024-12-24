using Burmalda.Entities.Auctions;
using Burmalda.Services.Users.Entities;

namespace Burmalda.Services.Auctions.Entitites;

public record AuctionDetails(
    ulong Id, 
    string? Title, 
    ulong OwnerId, 
    DateTimeOffset StartDate, 
    TimeSpan Duration, 
    bool IsCompleted, 
    IEnumerable<AuctionLotPreview> Lots, 
    ulong? WinnerId
){
    public static AuctionDetails FromAuction(Auction auction)
    {
        return new AuctionDetails(
            auction.Id,
            auction.Title,
            auction.OwnerId,
            auction.StartDate,
            auction.Duration,
            auction.IsCompleted,
            auction.Lots?.Select(AuctionLotPreview.FromLot) ?? [],
            auction.WinnerId
        );
    }
}