using Burmalda.Entities.Donation;

namespace Burmalda.Services.Dontes.Entities;

public record BetPreview(
    ulong Id, 
    decimal Amount, 
    ulong BetLotId, 
    ulong? BetOwnerId, 
    ulong BetDonateId
){
    public static BetPreview FromBet(AuctionBet bet)
    {
        return new BetPreview(
            bet.Id,
            bet.Amount,
            bet.LotId,
            bet.OwnerId,
            bet.DonateId
        );
    }
}