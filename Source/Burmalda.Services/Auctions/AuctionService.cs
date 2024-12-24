using Burmalda.DataAccess;
using Burmalda.Entities;
using Burmalda.Entities.Auctions;
using Burmalda.Entities.Donation;
using Burmalda.Entities.Users;
using Burmalda.Services.Auctions.Entitites;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Users.Entities;

namespace Burmalda.Services.Auctions;

public class AuctionService(
    IAuctionsRepository auctions,
    IUsersRepository users,    
    ILotsRepository lots,
    IBetsRepository bets
): IAuctionService
{
    public async Task<AuctionDetails?> FindByIdAsync(ulong id)
    {
        Auction auction = await auctions.FindAsync(x => x.Id == id);
        
        if (auction is null)
            return null;
        
        return AuctionDetails.FromAuction(auction);
    }

    public async Task<AuctionDetails> CreateAuctionAsync(AuctionCreationModel auction)
    {
        Streamer? streamer = await users.FindStreamerAsync(x => x.Id == auction.OwnerId);

        if (streamer == null)
            throw new BurmaldaEntityNotFoundException(nameof(auction.OwnerId));
        
        Auction newAuction = new()
        {
            Id = 0,
            Title = auction.Title,
            IsCompleted = false,
            TimeIncrementingStrategy = TimeIncrementingStrategy.AlwaysIncrement,
            StartDate = DateTimeOffset.Now,
            Duration = auction.InitialDuration,
            WinnerId = null,
            Winner = null,
            OwnerId = auction.OwnerId,
            Owner = null,
            Lots = [],
        };
        
         newAuction = await auctions.AddAsync(newAuction);
         return AuctionDetails.FromAuction(newAuction);
    }

    public async Task<AuctionLotPreview> CreateLotByOwnerAsync(ulong userId, LotCreationModel lot)
    {
        Auction? auction = await auctions.FindAsync(auction => auction.Id == lot.AuctionId && auction.OwnerId == userId);
        
        if (auction is null)
            throw new BurmaldaEntityNotFoundException(nameof(lot.AuctionId));
        
        AuctionLot newLot = await CreateLotAsync(auction, lot);
        
        return AuctionLotPreview.FromLot(newLot);
    }
    
    public async Task<AuctionLot> CreateLotAsync(Auction auction, LotCreationModel lot)
    {
        AuctionLot newLot = new()
        {
            Id = 0,
            Title = lot.Title,
            Bets = [],
            AuctionId = auction.Id,
            Auction = null!
        };
        return await lots.AddAsync(newLot);
    }

    public async Task<AuctionBet> SendBetAsync(AuctionLot lot, Donate donate)
    {
        AuctionBet bet = new()
        {
            Id = 0,
            Amount = donate.Amount,
            IsFake = false,
            LotId = lot.Id,
            Lot = null!,
            DonateId = donate.Id,
            Donate = null,
        };
        
        return await bets.AddAsync(bet);
    }

    public async Task<IEnumerable<AuctionLotPreview>> GetAuctionLotsAsync(ulong auctionId)
    {
        Auction? auction = await auctions.FindAsync(auction => auction.Id == auctionId);
        
        if (auction is null)
            throw new BurmaldaEntityNotFoundException(nameof(auctionId));
        
        return auction?.Lots?.Select(AuctionLotPreview.FromLot) ?? [];
    }

    public async Task<AuctionDetails> UpdateAuctionAsync(ulong auctionId, AuctionCreationModel newParmeters)
    {
        Auction? auction = await auctions.FindAsync(auction => auction.Id == auctionId && auction.OwnerId == newParmeters.OwnerId);
        
        if (auction is null)
            throw new BurmaldaEntityNotFoundException(nameof(auctionId));
        
        
        auction.Title = newParmeters.Title;
        auction.TimeIncrementingStrategy = newParmeters.TimeIncrementingStrategy;
        
        return AuctionDetails.FromAuction(auction);
    }

    public async Task<IEnumerable<AuctionPreview>> GetUserAuctionsAsync(ulong userId)
    {
        IEnumerable<Auction> result = await auctions.WhereAsync(x => x.OwnerId == userId);
        return result.Select(AuctionPreview.FromAuction);
    }

    public async Task<AuctionPreview> CompleteAuctionAsync(ulong userId, ulong auctionId, ulong winnerId)
    {
        Auction? auction = await auctions.FindAsync(auction => auction.Id == auctionId && auction.OwnerId == userId);
    
        if (auction is null)
            throw new BurmaldaEntityNotFoundException(nameof(auctionId));

        auction.IsCompleted = true;
        auction.WinnerId = winnerId;
        Auction result = await auctions.UpdateAsync(auction);
        return AuctionPreview.FromAuction(result);
    }
}