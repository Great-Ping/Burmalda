using System.Linq.Expressions;
using Burmalda.Entities.Donation;
using Microsoft.EntityFrameworkCore;

namespace Burmalda.DataAccess;

public class DonatesEfRepository(BurmaldaDbContext contextFactory)
    : BaseEfRepository<Donate>(contextFactory), IDonatesRepository;