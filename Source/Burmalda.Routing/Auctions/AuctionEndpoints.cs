using System.Text.RegularExpressions;
using Burmalda.Entities.Donation;
using Burmalda.Routing.Auctions.Entities;
using Burmalda.Services.Auctions;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Payment.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Burmalda.Routing.Auctions;

public static class AuctionEndpoints
{
    public const string Tag = "Аукционы";
    public const string TagDescription = "Методы управления аукционом";
    
    public static IEndpointRouteBuilder MapAuctinsRoutes(this IEndpointRouteBuilder builder, string prefix = "/auctions")
    {
        RouteGroupBuilder groupBuilder = builder.MapGroup(prefix)
            .WithTags([Tag]);           

        groupBuilder.MapGet("/", GetAuctionsAsync)
            .WithSummary("Вернет список аукционов");
        groupBuilder.MapPost("/", CreateAuctionAsync)
            .WithSummary("Создаст новый аукцион, вернет его id");
        
        groupBuilder.MapPatch("/{auctionId:ulong}", UpdateAuctionAsync)
            .WithSummary("Обновление параметров аукциона");
        groupBuilder.MapDelete("/{auctionId:ulong}", DeleteAuctionAsync)
            .WithSummary("Удаление аукциона");

        groupBuilder.MapGet("/{auctionId:ulong}/lots", GetLotsAsync)
            .WithSummary("Получение списка всех лотов");
        groupBuilder.MapPost("/{auctionId:ulong}/lots", CreateLotAsync)
            .RequireAuthorization()
            .WithSummary("Создать лот. Доступно только владельцу");

        groupBuilder.MapDelete("/{auctionId:ulong}/lots", DeleteLotAsync)
            .RequireAuthorization()
            .WithSummary("Удалить лот. Доступно только владельцу");
        
        groupBuilder.MapPost("/{auctionId:ulong}/lots/{lotId:ulong}", SendDonateAsync)
            .RequireAuthorization()
            .WithSummary("Создаст новый донат на лот");
        groupBuilder.MapPost("/{auctionId:ulong}/lots/@new", SendDonateToNewLotAsync)
            .RequireAuthorization()
            .WithSummary("Создаст новый донат на новый лот");
        
        return groupBuilder;
    }

    private static async Task<IResult> SendDonateToNewLotAsync(
        HttpContext context,
        [AsParameters] DonateCreationModel donate,
        [AsParameters] LotCreationModel lot,
        [FromServices] IAuctionService auctions
    ){
        Payments<DonatePreview> payments = await auctions.SendDonateToNewLotAsync(donate, lot);
        return Results.Ok(payments);
    }

    private static async Task<IResult> SendDonateAsync(
        HttpContext context,
        [AsParameters] DonateCreationModel args,
        [FromServices] IAuctionService auctions
    )
    {
        Payments<DonatePreview> payments = await auctions.SendDonateAsync(args);
        return Results.Ok(payments);
    }

    private static Task DeleteLotAsync(
        HttpContext context,
        [FromRoute] ulong auctionId
    ){
        throw new NotImplementedException();
    }


    private static Task<IResult> CreateLotAsync(
        HttpContext context,
        [FromRoute] ulong auctionId
    ){
        throw new NotImplementedException();
    }

    private static Task<IResult> GetLotsAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task<IResult> DeleteAuctionAsync()
    {
        throw new NotImplementedException();
    }

    internal static async Task<IResult> UpdateAuctionAsync(ulong auctionId)
    {
        throw new NotImplementedException();
    }

    internal static async Task<IResult> CreateAuctionAsync()
    {
        throw new NotImplementedException();
    }


    internal static async Task<IResult> GetAuctionsAsync()
    {
        throw new NotImplementedException();
   
    }
}