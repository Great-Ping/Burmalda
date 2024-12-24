using Burmalda.Services.Auctions.Entitites;
using Microsoft.AspNetCore.Mvc;

namespace Burmalda.Routing.Auctions.Entities;

public class LotCreationModel: ILotCreationModel
{
    [FromBody] public string Title { get; set; }
}