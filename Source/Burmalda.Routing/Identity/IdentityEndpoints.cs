using Burmalda.Services.Identification;
using Burmalda.Services.Identification.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Burmalda.Routing.Identity;

public static class IdentityEndpoints
{
    public static string Tag = "Идентификация";
    public static IEndpointRouteBuilder MapIdentityRoutes(
        this IEndpointRouteBuilder builder, 
        string prefix = "/auth"
    )
    {
        RouteGroupBuilder groupBuilder = builder.MapGroup(prefix)
            .WithTags([Tag]);

        groupBuilder.MapPost("/login", LoginAsync)
            .Produces<JwtTokens>();
        
        groupBuilder.MapPost("/signup", SignupAsync)            
            .Produces<JwtTokens>();

        groupBuilder.MapGet("/refresh", RefreshAsync)
            .Produces<AccessToken>();
        
        return groupBuilder;
    }

    private static async Task<IResult> SignupAsync(
        [FromBody] UserRegistrationData registrationData,
        [FromServices] IIdentificationService identificationService
    ){
        return Results.Ok(
            await identificationService.SignupAsync(registrationData)
        );
    }

    private static async Task<IResult> RefreshAsync(
        [FromQuery] string refresh,
        [FromServices] IIdentificationService identificationService
    )
    {
        return Results.Ok(
            await identificationService.RefreshAsync(refresh)
        );
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] UserLoginData loginData,
        [FromServices] IIdentificationService identificationService
    ){
        return Results.Ok(
            await identificationService.LoginAsync(loginData)
        );
    }
}

