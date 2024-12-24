using Burmalda.Entities.Donation;

namespace Burmalda.Services.Dontes.Entities;

public record DonatePreview(
    ulong Id,
    ulong DonatorId, 
    decimal Amount, 
    string Message, 
    BetPreview? Bet
){
    public static DonatePreview FromDonate(Donate donate)
    {
        return new DonatePreview(
            donate.Id,
            donate.DonatorId,
            donate.Amount,
            donate.Message,
            (donate.Bet != null) ? BetPreview.FromBet(donate.Bet) : null
        );
    }
}