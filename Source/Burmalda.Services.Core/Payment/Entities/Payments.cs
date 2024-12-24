namespace Burmalda.Services.Payment.Entities;

public class Payments<T>(
    ulong Id,
    string PaymentURL,
    bool IsPaid,
    T Resource
);