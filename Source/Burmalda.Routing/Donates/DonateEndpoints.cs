using System.Security.Claims;
using Burmalda.Services.Dontes;
using Burmalda.Services.Dontes.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Burmalda.Routing.Donates;

public static class DonateEndpoints
{
    public const string Tag = "Донаты";

    public static IEndpointRouteBuilder MapDonationRoutes(
        this IEndpointRouteBuilder builder,
        string prefix = "/donates"
    ){
        RouteGroupBuilder groupBuilder = builder.MapGroup(prefix)
            .WithTags(Tag);
        
        groupBuilder.MapGet("/{donateId:ulong}", FindDonteAsync)
            .WithSummary("Вернет информацию об определенном донате пользователя")
            .RequireAuthorization();
        
        groupBuilder.MapDelete("/{donateId:ulong}", DeleteDonteAsync)
            .WithSummary("Удалит донат, но только если тот не оплачен.")
            .RequireAuthorization();
        
        return groupBuilder;
    }

    private static async Task<IResult> DeleteDonteAsync(
        [FromRoute] ulong donateId,
        HttpContext context,
        [FromServices] IDonatesService donates
    ){
        string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        await donates.DeleteDonateAsync(ulong.Parse(userId), donateId);
        return Results.Ok();
    }

    private static async Task<IResult> FindDonteAsync(
        [FromRoute] ulong donateId,
        HttpContext context,
        [FromServices] IDonatesService donates   
    ){
        string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        DonatePreview? donate = await donates.FindDonateAsync(ulong.Parse(userId), donateId);
        
        if (donate == null)
            return Results.NotFound();
        
        return Results.Ok(donate);
    }
}