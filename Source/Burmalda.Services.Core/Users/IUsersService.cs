using Burmalda.Entities.Auctions;
using Burmalda.Entities.Donation;
using Burmalda.Entities.Users;
using Burmalda.Services.Users.Entities;

namespace Burmalda.Services.Users;

public interface IUsersService
{
    Task<UserIdentificationData> CreateNewUserAsync(UserCreationData userData);
    Task<UserIdentificationData?> FindUserIdentificationDataAsync(string email);
    Task<UserDetails?> FindUserDetailsAsync(ulong userId);
    Task<IEnumerable<Donate>> FindUserDonates(ulong userId);
    Task<IEnumerable<AuctionPreview>> FindUserAuctionsAsync(ulong userId);
    Task<IEnumerable<StreamerPreview>> FindUserSubscribesAsync(ulong userId);
    Task<StreamerPreview?> FindStreamerPreviewAsync(ulong userId);
}