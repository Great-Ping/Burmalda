using Burmalda.DataAccess;
using Burmalda.Entities.Auctions;
using Burmalda.Entities.Donation;
using Burmalda.Entities.Users;
using Burmalda.Services.Users.Entities;

namespace Burmalda.Services.Users;

public class UsersService(
    IUsersRepository usersRepository,
    IDonatesRepository donatesRepository
): IUsersService {
    IUsersRepository _usersRepository = usersRepository;
    IDonatesRepository _donatesRepository = donatesRepository;

    public async Task<UserIdentificationData> CreateNewUserAsync(UserCreationData userData)
    {
        User newUser = new()
        {
            Id = 0,
            Nickname = userData.Nickname,
            Email = userData.Email,
            PasswordHash = userData.PasswordHash,
            Permissions = UserPermissions.None,
            Subscriptions = null,
            Donates = null,
            Account = new UserAccount(balance: 0),
            ModeratedStreamers = null,
        };

        newUser = await _usersRepository.AddAsync(newUser);
        
        return UserIdentificationData.FromUser(newUser);
    }

    public async Task<UserIdentificationData?> FindUserIdentificationDataAsync(string email)
    {
        User? user = await _usersRepository.FindAsync(user => user.Email == email);
        return (user is null)
            ? null
            : new UserIdentificationData(
                user.Id,
                user.Email,
                user.PasswordHash
            );
    }

    public async Task<UserDetails?> FindUserDetailsAsync(ulong userId)
    {
        User? user = await _usersRepository.FindAsync(user => user.Id == userId);
        return (user is null)
            ? null
            : UserDetails.FromUser(user);
    }

    public async Task<IEnumerable<Donate>> FindUserDonates(ulong userId)
    {
        Donate[] donates = await _donatesRepository.WhereAsync(x => x.DonatorId == userId);
        return donates;
    }

    public async Task<IEnumerable<AuctionPreview>> FindUserAuctionsAsync(ulong userId)
    {
        Streamer? user = await _usersRepository.FindStreamerAsync(user => user.Id == userId);
        IEnumerable<Auction>? auctions = user?.Auctions;
        
        if (auctions is null)
            return [];
        
        
        return auctions.Select(AuctionPreview.FromAuction);
    }

    public async Task<IEnumerable<StreamerPreview>> FindUserSubscribesAsync(ulong userId)
    {
        Streamer? user = await _usersRepository.FindStreamerAsync(user => user.Id == userId);
        IEnumerable<Streamer>? subscribes =  user?.Subscriptions;
        
        if (subscribes is null)
            return [];
        
        return subscribes.Select(StreamerPreview.FromStreamer);
    }

    public async Task<StreamerPreview?> FindStreamerPreviewAsync(ulong userId)
    {
        Streamer? user = await _usersRepository.FindStreamerAsync(user => user.Id == userId);
        
        if (user is null)
            return null;
        
        return StreamerPreview.FromStreamer(user);
    }
}