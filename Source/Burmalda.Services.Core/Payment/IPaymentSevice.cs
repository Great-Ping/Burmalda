using Burmalda.Entities.Donation;
using Burmalda.Services.Payment.Entities;

namespace Burmalda.Services.Payment;

public delegate Task ResourcePaidAsync<T>(T resource);

public interface IPaymentService
{
    Task<Payments<T>> CreatePaymentsAsync<T>(T resource, decimal amount, ResourcePaidAsync<T> onPaid);
}