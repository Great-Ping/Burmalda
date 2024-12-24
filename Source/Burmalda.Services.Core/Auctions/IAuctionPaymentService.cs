using Burmalda.Services.Auctions.Entitites;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Payment.Entities;

namespace Burmalda.Services.Auctions;

public interface IAuctionPaymentService
{
    Task<Payments<DonatePreview>> SendDonateAsync(DonateCreationModel donate, LotSummary lot);
    Task<Payments<DonatePreview>> SendDonateToNewLotAsync(DonateCreationModel donate, LotCreationModel lot); 
}