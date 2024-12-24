using Burmalda.Entities.Auctions;

namespace Burmalda.Services.Auctions.Entitites;

public record AuctionCreationModel(
    string Title,
    TimeSpan InitialDuration,
    TimeIncrementingStrategy TimeIncrementingStrategy,
    ulong OwnerId
);