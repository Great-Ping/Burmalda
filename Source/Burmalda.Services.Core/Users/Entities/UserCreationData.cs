using Burmalda.Entities.Users;

namespace Burmalda.Services.Users.Entities;

public record UserCreationData(
    string Nickname,
    string Email,
    string PasswordHash
);