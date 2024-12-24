using Burmalda.Entities.Users;

namespace Burmalda.Services.Users.Entities;

public record StreamerPreview(
    ulong Id, 
    string Nickname, 
    IEnumerable<AuctionPreview>? Auction
){
    public static StreamerPreview FromStreamer(Streamer streamer)
    {
        return new StreamerPreview(
            streamer.Id,
            streamer.Nickname,
            streamer.Auctions?.Select(AuctionPreview.FromAuction)
        );
    }
}