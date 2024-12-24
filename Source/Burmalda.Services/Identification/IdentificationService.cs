using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Burmalda.Entities;
using Burmalda.Entities.Users;
using Burmalda.Services.Identification.Entities;
using Burmalda.Services.Identification.Exceptions;
using Burmalda.Services.Users;
using Burmalda.Services.Users.Entities;
using Burmalda.Services.Users.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Burmalda.Services.Identification;

public class IdentificationService(
    IUsersService usersService
): IIdentificationService {
    private static readonly TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(1);
    private static readonly TimeSpan AccessTokenExpiration = TimeSpan.FromMinutes(15);
    
    //Приватность поля, залог безопасности :)
    private static readonly string SafetestKey = "SPASI_I_SOHRANI_SPASI_I_SOHRANI_";
    private static readonly SymmetricSecurityKey SymmetricSecurityKey = new(Encoding.UTF8.GetBytes(SafetestKey));

    public static readonly TokenValidationParameters AccessValidationParameters = new()
    {
        ValidateLifetime = true,
        IssuerSigningKey = SymmetricSecurityKey,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
    
    
    private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
    private readonly IUsersService _usersService = usersService;
    private readonly IPasswordHasher<object> _passwordHasher = 
        new PasswordHasher<object>();



    public async Task<JwtTokens> LoginAsync(UserLoginData userLoginData)
    {
        UserIdentificationData? user = await _usersService.FindUserIdentificationDataAsync(userLoginData.Email);
        if (user == null)
            throw new UserNotFoundException();

        PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(
            user, 
            user.PasswordHash, 
            userLoginData.Password
        );
        
        if (verificationResult == PasswordVerificationResult.Failed)
            throw new LoginException();

        return CreateJwtTokens(user);
    }

    public async Task<JwtTokens> SignupAsync(UserRegistrationData registrationData)
    {
        UserCreationData creationData = new UserCreationData(
            registrationData.Email,
            registrationData.Email,
            _passwordHasher.HashPassword(registrationData, registrationData.Password)
        );
        
        UserIdentificationData user = await _usersService.CreateNewUserAsync(creationData);
        return CreateJwtTokens(user);
    }

    public async Task<AccessToken> RefreshAsync(string refresh)
    {
        (ClaimsPrincipal claims, SecurityToken token) = ValidateRefreshToken(refresh);

        string strId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                       ?? throw new InvalidRefreshToken();
        
        if (!ulong.TryParse(strId, out ulong userId))
            throw new InvalidRefreshToken();
        
        return new AccessToken(
           _tokenHandler.WriteToken(CreateAccessToken(userId))
        );
    }

    private JwtTokens CreateJwtTokens(UserIdentificationData user)
    {
        JwtSecurityToken refreshToken = new(
            signingCredentials: new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow + RefreshTokenExpiration,
            notBefore: DateTime.UtcNow,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("t", "r")
            ]
        );
        
        JwtSecurityToken accessToken = CreateAccessToken(user.Id);
        
        return new JwtTokens(
            _tokenHandler.WriteToken(refreshToken),
            _tokenHandler.WriteToken(accessToken)
        );
    }

    private JwtSecurityToken CreateAccessToken(ulong userId)
         => new(
             signingCredentials: new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256),
             expires: DateTime.UtcNow + RefreshTokenExpiration,
             notBefore: DateTime.UtcNow,
             claims:
             [
                 new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                 new Claim("t", "a")
             ]
         );

    private (ClaimsPrincipal, SecurityToken) ValidateRefreshToken(
        string token
    ){
        TokenValidationParameters parameters = new()
        {
            ValidateLifetime = true,
            IssuerSigningKey = SymmetricSecurityKey,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false
        };

        try
        {
            ClaimsPrincipal claims = _tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);
            if (claims.HasClaim(x => x.Type == "t" && x.Value == "r"))
                return (claims, validatedToken);
        }
        catch (Exception exception)
        {
            throw new InvalidRefreshToken(innerException: exception);
        }
        
        throw new InvalidRefreshToken();
    }
}