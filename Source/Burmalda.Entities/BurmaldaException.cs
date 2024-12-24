namespace Burmalda.Entities;

public class BurmaldaException(
    BurmaldaExceptionCode exceptionCode, 
    string? message = null, 
    Exception? innerException = null
): Exception {
    public BurmaldaExceptionCode Code { get; }
}