namespace Burmalda.Entities.Auctions;

public enum TimeIncrementingStrategy
{
    AlwaysIncrement,
    DontIncrement,
    IncrementIfLessAMinuteLeft,
}