using Burmalda.Entities.Users;

namespace Burmalda.Services.Users.Entities;

public record UserDetails(
    ulong Id,
    string Nickname,
    string Email,
    UserPermissions Permissions,
    UserAccount Account
){
    public static UserDetails FromUser(User user)
    {
        return new UserDetails(
            user.Id,
            user.Nickname,
            user.Email,
            user.Permissions,
            user.Account
        );
    }
};