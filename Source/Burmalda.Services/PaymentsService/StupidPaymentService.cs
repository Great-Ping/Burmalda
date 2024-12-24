using Burmalda.Services.Payment;
using Burmalda.Services.Payment.Entities;

namespace Burmalda.Services.PaymentsService;

public class StupidPaymentService: IPaymentService
{
    public async Task<Payments<T>> CreatePaymentsAsync<T>(T resource, decimal amount, ResourcePaidAsync<T>? onPaid)
    {
        if (onPaid is not null)
            await onPaid(resource);

        return new Payments<T>(
            0,
            "",
            true,
            resource
        );
    }
}