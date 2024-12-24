using System.Text.RegularExpressions;
using Burmalda.Entities.Donation;
using Burmalda.Routing.Auctions.Entities;
using Burmalda.Services.Auctions;
using Burmalda.Services.Auctions.Entitites;
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

        groupBuilder.MapPost("/", CreateAuctionAsync)
            .RequireAuthorization()
            .WithSummary("Создаст новый аукцион, вернет его данные");

        groupBuilder.MapGet("/{auctionId:ulong}", GetAuctionByIdAsync)
            .WithSummary("Вернет аукцион по его идентификатору");
        
        groupBuilder.MapGet("/@my", GetAuctionsAsync)
            .RequireAuthorization()
            .WithSummary("Вернет список всех аукционов пользователя");

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

    private static async Task<IResult> GetAuctionByIdAsync(
        [FromRoute] ulong auctionId,
        [FromServices] IAuctionService auctions
    )
    {
        AuctionDetails? auction = await auctions.FindByIdAsync(auctionId);
        if (auction is null)
            return Results.NotFound();
        
        return Results.Ok(auction);
    }

    private static async Task<IResult> SendDonateToNewLotAsync(
        HttpContext context,
        [FromRoute] ulong auctionId,
        [FromBody] SendDonateToNewLotRequestBody requestBody,
        [FromServices] IAuctionPaymentService auctionsPayment
    ){
        DonateCreationModel donate = new(
            context.GetUserId(),
            requestBody.Message,
            requestBody.Amount
        );

        LotCreationModel lot = new(
            auctionId,
            requestBody.LotTitle
        );
        
        Payments<DonatePreview> payments = await auctionsPayment.SendDonateToNewLotAsync(donate, lot);
        return Results.Ok(payments);
    }

    private static async Task<IResult> SendDonateAsync(
        HttpContext context,
        [FromRoute] ulong auctionId,
        [FromRoute] ulong lotId,
        [FromBody] SendDonateRequestBody requestBody,
        [FromServices] IAuctionPaymentService auctionsPayment
    ){
        DonateCreationModel donate = new(
            context.GetUserId(),
            requestBody.Message,
            requestBody.Amount
        );

        LotSummary lot = new LotSummary(
            auctionId,
            lotId
        );
        
        Payments<DonatePreview> payments = await auctionsPayment.SendDonateAsync(donate, lot);
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

    internal static async Task<IResult> CreateAuctionAsync(
        HttpContext context,
        [FromServices] IAuctionService auctions,
        [FromBody] CreateAuctionRequestBody requestBody
    )
    {
        AuctionCreationModel auction = new(
            requestBody.Title,
            requestBody.InitialDuration,
            requestBody.TimeIncrementingStrategy,
            context.GetUserId()
        );
        
        AuctionDetails created = await auctions.CreateAuctionAsync(auction);
        return Results.Ok(created);
    }


    internal static async Task<IResult> GetAuctionsAsync()
    {
        throw new NotImplementedException();
   
    }
}