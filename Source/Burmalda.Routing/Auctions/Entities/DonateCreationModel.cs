using Burmalda.Services.Auctions.Entitites;
using Microsoft.AspNetCore.Mvc;

namespace Burmalda.Routing.Auctions.Entities;

public class DonateCreationModel : IDonateCreationModel
{
    public ulong UserId { get; set; } = 0;
    [FromRoute] public ulong LotId { get; set; } = 0;
    [FromBody] public string Message { get; set; } = String.Empty;
    [FromBody] public decimal Amount { get; } = 0;
}