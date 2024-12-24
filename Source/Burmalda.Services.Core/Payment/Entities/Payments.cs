namespace Burmalda.Services.Payment.Entities;

public record Payments<T>(
    ulong Id,
    string PaymentURL,
    bool IsPaid,
    T Resource
);