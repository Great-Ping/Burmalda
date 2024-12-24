using System.Security.Claims;
using Burmalda.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Burmalda.Routing.Users;

public static class UsersEndpoint
{
    public const string Tag = "Пользователи";
    
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder builder,
        string prefix = "/users"
    ){
        RouteGroupBuilder groupBuilder = builder.MapGroup(prefix)
            .WithTags([Tag]);

        groupBuilder.MapGet("/{userId:ulong}", GetUserPreview);
        groupBuilder.MapGet("/@me", GetMeDetailsAsync)
            .RequireAuthorization()
            .WithSummary("Вернет информацию о пользователе");
        
        groupBuilder.MapGet("/@me/donations", GetMyDonationsAsync)
            .RequireAuthorization()
            .WithSummary("Вернет донаты пользователя");
        
        groupBuilder.MapGet("/@me/subscribes", GetMySubscribesAsync)
            .RequireAuthorization()
            .WithSummary("Вернет подписки пользователя");
        
        groupBuilder.MapGet("/@me/auctions", GetMyAuctionsAsync)
            .RequireAuthorization()
            .WithSummary("Вернет аукционы пользователя");
        
        return groupBuilder;
    }

    private static async Task<IResult> GetMyAuctionsAsync(
        HttpContext context,
        [FromServices] IUsersService users   
    ){
        string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Results.Ok(
            await users.FindUserAuctionsAsync(ulong.Parse(userId))
        );
    }

    private static async Task<IResult> GetMySubscribesAsync(
        HttpContext context,
        [FromServices] IUsersService users
    ){
        string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Results.Ok(
            await users.FindUserSubscribesAsync(ulong.Parse(userId))
        );
    }

    private static async Task<IResult> GetMyDonationsAsync(    
        HttpContext context,
        [FromServices] IUsersService users
    ){
        string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Results.Ok(
            await users.FindUserDonates(ulong.Parse(userId))
        );
    }

    private static async Task<IResult> GetMeDetailsAsync(
        HttpContext context,
        [FromServices] IUsersService users
    ){
        string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Results.Ok(
            await users.FindUserDetailsAsync(ulong.Parse(userId))
        );
    }

    private static async Task<IResult> GetUserPreview(
        [FromRoute] ulong userId,
        [FromServices] IUsersService users
    ){
        return Results.Ok(
            await users.FindStreamerPreviewAsync(userId)
        );
    }
}