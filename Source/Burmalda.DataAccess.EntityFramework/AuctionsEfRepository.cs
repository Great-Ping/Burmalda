using Burmalda.Entities.Auctions;
using Microsoft.EntityFrameworkCore;

namespace Burmalda.DataAccess;

public class AuctionsEfRepository(BurmaldaDbContext contextFactory)
    : BaseEfRepository<Auction>(contextFactory), IAuctionsRepository;