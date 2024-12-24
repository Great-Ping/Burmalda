using Burmalda.Services.Identification.Entities;

namespace Burmalda.Services.Identification;

public interface IIdentificationService
{
    Task<JwtTokens> LoginAsync(UserLoginData userLoginData);
    Task<JwtTokens> SignupAsync(UserRegistrationData registrationData);
    Task<AccessToken> RefreshAsync(string refresh);
}