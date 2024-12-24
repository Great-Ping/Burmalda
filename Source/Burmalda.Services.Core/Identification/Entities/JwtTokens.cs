namespace Burmalda.Services.Identification.Entities;

public record JwtTokens(
    string Refresh,
    string Access
);