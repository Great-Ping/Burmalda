using Burmalda.DataAccess;
using Burmalda.Services.Auctions.Entitites;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Payment.Entities;

namespace Burmalda.Services.Auctions;

public class AuctionService(IAuctionsRepository auctions, ILotsRepository lots) : IAuctionService
{
    public Task<Payments<DonatePreview>> SendDonateAsync(IDonateCreationModel donate)
    {
        throw new NotImplementedException();
    }

    public Task<Payments<DonatePreview>> SendDonateToNewLotAsync(IDonateCreationModel donate, ILotCreationModel lot)
    {
        throw new NotImplementedException();
    }
}