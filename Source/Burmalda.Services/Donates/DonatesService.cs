using Burmalda.DataAccess;
using Burmalda.Entities.Donation;
using Burmalda.Services.Dontes;
using Burmalda.Services.Dontes.Entities;

namespace Burmalda.Services.Donates;

public class DonatesService(IDonatesRepository donates) : IDonatesService
{
    public async Task DeleteDonateAsync(ulong userId, ulong donateId)
    {
        Donate? donate = await donates.FindAsync(donate => donate.Id == donateId && donate.DonatorId == userId);
        if (donate == null)
            return;
        
        await donates.DeleteAsync(donate);
    }

    public async Task<DonatePreview?> FindDonateAsync(ulong userId, ulong donateId)
    {
        Donate? donate = await donates.FindAsync(donate => donate.Id == donateId && donate.DonatorId == userId);
            
        if (donate == null)
            return null;

        return DonatePreview.FromDonate(donate);
    }
}