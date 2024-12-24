using Burmalda.Entities.Donation;
using Microsoft.EntityFrameworkCore;

namespace Burmalda.DataAccess;

public class BetsEfRepository(IDbContextFactory<BurmaldaDbContext> contextFactory)
    : BaseEfRepository<AuctionBet>(contextFactory), IBetsRepository;