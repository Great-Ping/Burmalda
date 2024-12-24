using Burmalda.Entities.Auctions;
using Burmalda.Services.Auctions.Entitites;

namespace Burmalda.Routing.Auctions.Entities;

public class CreateAuctionRequestBody
{
    public required string Title { get; set; }
    public required TimeSpan InitialDuration { get; set; }
    public required TimeIncrementingStrategy TimeIncrementingStrategy { get; set; }
}