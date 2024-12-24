using Burmalda.Services.Auctions.Entitites;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Payment.Entities;

namespace Burmalda.Services.Auctions;

public interface IAuctionService
{
    Task<Payments<DonatePreview>> SendDonateAsync(IDonateCreationModel donate);
    Task<Payments<DonatePreview>> SendDonateToNewLotAsync(IDonateCreationModel donate, ILotCreationModel lot);
}