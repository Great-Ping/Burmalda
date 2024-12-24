namespace Burmalda.Services.Auctions.Entitites;

public record DonateCreationModel(
   ulong DonatorId,
   string Message,
   decimal Amount
);
