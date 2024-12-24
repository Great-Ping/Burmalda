using Burmalda.Entities.Users;

namespace Burmalda.Services.Users.Entities;

public record UserIdentificationData(
    ulong Id,
    string Email,
    string PasswordHash
)
{
    public static UserIdentificationData FromUser(User user)
    {
        return new UserIdentificationData(
            user.Id,
            user.Email,
            user.PasswordHash
        );
    }
};