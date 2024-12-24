using Burmalda.Entities.Users;

namespace Burmalda.Services.Users.Entities;

public record UserDetails(
    string Nickname,
    string Email,
    UserPermissions Permissions,
    UserAccount Account
){
    public static UserDetails FromUser(User user)
    {
        return new UserDetails(
            user.Nickname,
            user.Email,
            user.Permissions,
            user.Account
        );
    }
};