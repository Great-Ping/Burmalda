using Burmalda.Entities;

namespace Burmalda.Services.Identification.Exceptions;

public class InvalidRefreshToken(
    string? message = null, 
    Exception? innerException = null
): BurmaldaException(
    BurmaldaExceptionCode.InvalidRefreshTokenException, 
    message, 
    innerException
){
    
}