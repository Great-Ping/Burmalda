using Burmalda.Entities.Auctions;

namespace Burmalda.Services.Users.Entities;

public record AuctionLotPreview(
    ulong Id, 
    ulong AuctionId, 
    string Title
){
    public static AuctionLotPreview FromLot(AuctionLot auctionLot)
    {
        return new AuctionLotPreview(
            auctionLot.Id,
            auctionLot.AuctionId,
            auctionLot.Title
        );
    }
}