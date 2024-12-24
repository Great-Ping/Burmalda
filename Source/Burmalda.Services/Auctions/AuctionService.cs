using Burmalda.DataAccess;
using Burmalda.Entities.Auctions;
using Burmalda.Entities.Donation;
using Burmalda.Entities.Users;
using Burmalda.Services.Auctions.Entitites;
using Burmalda.Services.Dontes.Entities;
using Burmalda.Services.Payment;
using Burmalda.Services.Payment.Entities;

namespace Burmalda.Services.Auctions;

public class AuctionService(
    IPaymentService paymentService, 
    IAuctionsRepository auctions, 
    ILotsRepository lots, 
    IUsersRepository users,
    IBetsRepository bets,
    IDonatesRepository donates
) : IAuctionService 
{
    public async Task<Payments<DonatePreview>> SendDonateAsync(DonateCreationModel donate, LotSummary lotSummary)
    {
       Auction? auction = await auctions.FindAsync(auction => auction.Id == lotSummary.AuctionId);
        
        if (auction is null)
            throw new KeyNotFoundException(nameof(lotSummary.AuctionId));
        
        User? user = await users.FindAsync(user => user.Id == donate.DonatorId);
        
        if (user is null)
            throw new KeyNotFoundException(nameof(donate.DonatorId));

        AuctionLot? lot = await lots.FindAsync(x => lotSummary.LotId == x.Id);
        
        if (lot is null)
            throw new KeyNotFoundException(nameof(lotSummary.LotId));
        
        
        Donate newDonate = new Donate
        {
            IsPaid = false,
            Amount = donate.Amount,
            Message = donate.Message,
            DonatorId = user.Id,
            Donator = user,
            Bet = null,
            Id = 0
        };
        
        newDonate = await donates.AddAsync(newDonate);

        ResourcePaidAsync<Donate> after = async (newDonate) =>
        {
            AuctionBet bet = new()
            {
                Id = 0,
                Amount = donate.Amount,
                IsFake = false,
                LotId = lot.Id,
                Lot = lot,
                OwnerId = user.Id,
                Owner = user,
                DonateId = newDonate.Id,
                Donate = newDonate,

            };
            bet = await bets.AddAsync(bet);

            newDonate.IsPaid = true;
            newDonate.BetId = bet.Id;
            newDonate.Bet = bet;
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
            throw new KeyNotFoundException(nameof(lot.AuctionId));
        
        User? user = await users.FindAsync(user => user.Id == donate.DonatorId);
        
        if (user is null)
            throw new KeyNotFoundException(nameof(donate.DonatorId));

        Donate newDonate = new Donate
        {
            IsPaid = false,
            Amount = donate.Amount,
            Message = donate.Message,
            DonatorId = user.Id,
            Donator = user,
            Bet = null,
            Id = 0
        };
        
        newDonate = await donates.AddAsync(newDonate);

        ResourcePaidAsync<Donate> after = async (newDonate) =>
        {
            AuctionLot newLot = new()
            {
                Id = 0,
                Title = lot.Title,
                Bets = new List<AuctionBet>(),
                AuctionId = auction.Id,
                Auction = auction
            };
            newLot = await lots.AddAsync(newLot);

            AuctionBet bet = new()
            {
                Id = 0,
                Amount = donate.Amount,
                IsFake = false,
                LotId = newLot.Id,
                Lot = newLot,
                OwnerId = user.Id,
                Owner = user,
                DonateId = newDonate.Id,
                Donate = newDonate,

            };
            bet = await bets.AddAsync(bet);

            newDonate.IsPaid = true;
            newDonate.BetId = bet.Id;
            newDonate.Bet = bet;
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
}