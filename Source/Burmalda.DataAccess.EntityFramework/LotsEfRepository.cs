using Burmalda.Entities.Auctions;
using Microsoft.EntityFrameworkCore;

namespace Burmalda.DataAccess;

public class LotsEfRepository(BurmaldaDbContext contextFactory)
    : BaseEfRepository<AuctionLot>(contextFactory), ILotsRepository;