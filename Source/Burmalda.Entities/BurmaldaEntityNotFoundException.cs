namespace Burmalda.Entities;

public class BurmaldaEntityNotFoundException(string? message = null, Exception? innerException = null) 
    : BurmaldaException(BurmaldaExceptionCode.EntityNotFound, message, innerException)
{
}