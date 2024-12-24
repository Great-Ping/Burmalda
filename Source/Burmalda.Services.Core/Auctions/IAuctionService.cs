using Burmalda.Entities.Auctions;
using Burmalda.Entities.Donation;
using Burmalda.Services.Auctions.Entitites;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Users.Entities;

namespace Burmalda.Services.Auctions;

public interface IAuctionService
{
    Task<AuctionDetails?> FindByIdAsync(ulong id);
    Task<AuctionDetails> CreateAuctionAsync(AuctionCreationModel auction);
    
    Task<AuctionLotPreview> CreateLotByOwnerAsync(ulong userId, LotCreationModel lot);
    Task<AuctionLot> CreateLotAsync(Auction auction, LotCreationModel lot);
    
    Task<AuctionBet> SendBetAsync(AuctionLot lot, Donate donate);
    Task<IEnumerable<AuctionLotPreview>> GetAuctionLotsAsync(ulong auctionId);
    Task<AuctionDetails> UpdateAuctionAsync(ulong auctionId, AuctionCreationModel auction);
    Task<IEnumerable<AuctionPreview>> GetUserAuctionsAsync(ulong userId);
    Task<AuctionPreview> CompleteAuctionAsync(ulong userId, ulong auctionId, ulong winnerId);
}