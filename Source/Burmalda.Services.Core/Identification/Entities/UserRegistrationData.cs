namespace Burmalda.Services.Identification.Entities;

public record UserRegistrationData(
    string Nickname,
    string Email,
    string Password    
);