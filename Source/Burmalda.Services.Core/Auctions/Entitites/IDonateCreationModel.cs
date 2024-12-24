namespace Burmalda.Services.Auctions.Entitites;

public interface IDonateCreationModel
{
    ulong UserId { get; }
    ulong LotId { get; set; }
    string Message { get; }
    decimal Amount { get; }
}