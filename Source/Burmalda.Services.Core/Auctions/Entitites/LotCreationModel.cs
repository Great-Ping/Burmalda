namespace Burmalda.Services.Auctions.Entitites;

public record LotCreationModel(
    ulong AuctionId,
    string Title
);