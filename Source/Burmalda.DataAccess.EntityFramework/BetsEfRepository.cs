using Burmalda.Entities.Donation;
using Microsoft.EntityFrameworkCore;

namespace Burmalda.DataAccess;

public class BetsEfRepository(BurmaldaDbContext context)
    : BaseEfRepository<AuctionBet>(context), IBetsRepository;