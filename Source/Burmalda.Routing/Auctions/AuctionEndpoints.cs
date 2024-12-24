using System.Text.RegularExpressions;
using Burmalda.Entities.Donation;
using Burmalda.Routing.Auctions.Entities;
using Burmalda.Services.Auctions;
using Burmalda.Services.Auctions.Entitites;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Payment.Entities;
using Burmalda.Services.Users.Entities;
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
        
        groupBuilder.MapGet("/@my", GetMyAuctionsAsync)
            .RequireAuthorization()
            .WithSummary("Вернет список всех аукционов пользователя");

        groupBuilder.MapPost("/{auctionId:ulong}", UpdateAuctionAsync)
            .RequireAuthorization()
            .WithSummary("Обновление параметров аукциона");
        
        groupBuilder.MapPost("/{auctionId:ulong}/complete", CompleteAuctionAsync)
            .RequireAuthorization()
            .WithSummary("Завершение аукциона");

        groupBuilder.MapGet("/{auctionId:ulong}/lots", GetLotsAsync)
            .WithSummary("Получение списка всех лотов");
        groupBuilder.MapPost("/{auctionId:ulong}/lots", CreateLotAsync)
            .RequireAuthorization()
            .WithSummary("Создать лот. Доступно только владельцу");
        
        groupBuilder.MapPost("/{auctionId:ulong}/lots/{lotId:ulong}", SendDonateAsync)
            .RequireAuthorization()
            .WithSummary("Создаст новый донат на лот");
        groupBuilder.MapPost("/{auctionId:ulong}/lots/@new", SendDonateToNewLotAsync)
            .RequireAuthorization()
            .WithSummary("Создаст новый донат на новый лот");
        
        return groupBuilder;
    }

    private static async Task<IResult> CompleteAuctionAsync(
        HttpContext context,
        [FromRoute] ulong auctionId,
        [FromBody] CompleteAuctionRequestBody requestBody,
        [FromServices] IAuctionService auctions
    ){
        ulong userId = context.GetUserId();
        
        AuctionPreview auction = await auctions.CompleteAuctionAsync(userId, auctionId, requestBody.WinnerId);
        return Results.Ok(auction);
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
    
    private static async Task<IResult> CreateLotAsync(
        HttpContext context,
        [FromRoute] ulong auctionId,
        [FromBody] CreateLotByOwnerRequestBody requestBody,
        [FromServices] IAuctionService auctions
    ){ 
        AuctionLotPreview lot = await auctions.CreateLotByOwnerAsync(
            context.GetUserId(), 
            new LotCreationModel(auctionId, requestBody.Title)
        );

        return Results.Ok(lot);
    }

    private static async Task<IResult> GetLotsAsync(
        HttpContext context,
        [FromRoute] ulong auctionId,
        [FromServices] IAuctionService auctions   
    ){
        IEnumerable<AuctionLotPreview> lots = await auctions.GetAuctionLotsAsync(auctionId);
        return Results.Ok(lots);
    }
    
    internal static async Task<IResult> UpdateAuctionAsync(
        HttpContext context,
        [FromRoute] ulong auctionId,
        [FromBody] CreateAuctionRequestBody requestBody,
        [FromServices] IAuctionService auctions
    ){
        
        AuctionCreationModel auction = new(
            requestBody.Title,
            requestBody.InitialDuration,
            requestBody.TimeIncrementingStrategy,
            context.GetUserId()
        );

        AuctionDetails newAuction = await auctions.UpdateAuctionAsync(auctionId, auction);
        return Results.Ok(newAuction);
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


    internal static async Task<IResult> GetMyAuctionsAsync(
        HttpContext context,
        [FromServices] IAuctionService auctions
    ){
        ulong userId = context.GetUserId();
        IEnumerable<AuctionPreview> result = await auctions.GetUserAuctionsAsync(userId);
        return Results.Ok(result);
    }
}