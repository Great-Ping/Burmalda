using Burmalda.Services.Dontes.Entities;

namespace Burmalda.Services.Dontes;

public interface IDonatesService
{
    Task DeleteDonateAsync(ulong userId, ulong donateId);
    Task<DonatePreview?> FindDonateAsync(ulong userId, ulong donateId);
} 