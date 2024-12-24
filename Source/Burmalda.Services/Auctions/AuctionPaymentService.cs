using Burmalda.DataAccess;
using Burmalda.Entities;
using Burmalda.Entities.Auctions;
using Burmalda.Entities.Donation;
using Burmalda.Entities.Users;
using Burmalda.Services.Auctions.Entitites;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Payment;
using Burmalda.Services.Payment.Entities;

namespace Burmalda.Services.Auctions;

public class AuctionPaymentService(
    IPaymentService paymentService, 
    IAuctionsRepository auctions, 
    ILotsRepository lots, 
    IUsersRepository users,
    IBetsRepository bets,
    IDonatesRepository donates,
    IAuctionService auctionSerivce
) : IAuctionPaymentService 
{
    public async Task<Payments<DonatePreview>> SendDonateAsync(DonateCreationModel donate, LotSummary lotSummary)
    {
       Auction? auction = await auctions.FindAsync(auction => auction.Id == lotSummary.AuctionId);
        
        if (auction is null)
            throw new BurmaldaEntityNotFoundException(nameof(lotSummary.AuctionId));
        
        User? user = await users.FindAsync(user => user.Id == donate.DonatorId);
        
        if (user is null)
            throw new BurmaldaEntityNotFoundException(nameof(donate.DonatorId));

        AuctionLot? lot = await lots.FindAsync(x => lotSummary.LotId == x.Id);
        
        if (lot is null)
            throw new BurmaldaEntityNotFoundException(nameof(lotSummary.LotId));
        
        
        Donate newDonate = new Donate
        {
            IsPaid = false,
            Amount = donate.Amount,
            Message = donate.Message,
            DonatorId = user.Id,
            Donator = null!,
            Bet = null,
            Id = 0
        };
        
        newDonate = await donates.AddAsync(newDonate);

        ResourcePaidAsync<Donate> after = async (newDonate) =>
        {
            newDonate.IsPaid = true;
            AuctionBet bet = await auctionSerivce.SendBetAsync(lot, newDonate);
            newDonate.BetId = bet.Id;
            await donates.UpdateAsync(newDonate);
        };

        Payments<Donate> payments = await paymentService.CreatePaymentsAsync(newDonate, newDonate.Amount, after);
        
        return new Payments<DonatePreview>(
            payments.Id,
            payments.PaymentURL,
            payments.IsPaid,
            DonatePreview.FromDonate(payments.Resource)
        );
    }

    public async Task<Payments<DonatePreview>> SendDonateToNewLotAsync(DonateCreationModel donate, LotCreationModel lot)
    {
        Auction? auction = await auctions.FindAsync(auction => auction.Id == lot.AuctionId);
        
        if (auction is null)
            throw new BurmaldaEntityNotFoundException(nameof(lot.AuctionId));
        
        User? user = await users.FindAsync(user => user.Id == donate.DonatorId);
        
        if (user is null)
            throw new BurmaldaEntityNotFoundException(nameof(donate.DonatorId));

        Donate newDonate = new Donate
        {
            Id = 0,
            IsPaid = false,
            Amount = donate.Amount,
            Message = donate.Message,
            DonatorId = user.Id,
            Donator = null!,
            Bet = null,
        };
        
        newDonate = await donates.AddAsync(newDonate);

        ResourcePaidAsync<Donate> after = async (newDonate) =>
        {
            AuctionLot newLot = await auctionSerivce.CreateLotAsync(auction, lot);
            newDonate.IsPaid = true;
            
            AuctionBet bet = await auctionSerivce.SendBetAsync(newLot, newDonate);
            newDonate.BetId = bet.Id;
            newDonate = await donates.UpdateAsync(newDonate);
        };

        Payments<Donate> payments = await paymentService.CreatePaymentsAsync(newDonate, newDonate.Amount, after);
        
        return new Payments<DonatePreview>(
            payments.Id,
            payments.PaymentURL,
            payments.IsPaid,
            DonatePreview.FromDonate(payments.Resource)
        );
    }
}