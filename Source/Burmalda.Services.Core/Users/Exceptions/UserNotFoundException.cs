using Burmalda.Entities;

namespace Burmalda.Services.Users.Exceptions;

public class UserNotFoundException(): BurmaldaException(BurmaldaExceptionCode.UserNotFoundException)
{
    
}