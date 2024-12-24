using Burmalda.Entities.Auctions;
using Burmalda.Entities.Donation;
using Burmalda.Entities.Support;
using Burmalda.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Burmalda.DataAccess;

public sealed class BurmaldaDbContext : DbContext
{
    public DbSet<User> Users { get; private set; }
    public DbSet<Streamer> Streamers { get; private set; }
    public DbSet<Auction> Auctions { get; private set; }
    public DbSet<AuctionLot> AuctionLots { get; private set; }
    public DbSet<Donate> Donates { get; private set; }
    public DbSet<AuctionBet> Bets { get; private set; }
    public DbSet<SupportBranch> SupportBranches { get; private set; }
    public DbSet<SupportBranchMessage> SupportMessages { get; private set; }

    public BurmaldaDbContext(DbContextOptions<BurmaldaDbContext> options) : base(options)
    {
        Users = Set<User>();
        Streamers = Set<Streamer>();
        Auctions = Set<Auction>();
        AuctionLots = Set<AuctionLot>();
        Donates = Set<Donate>();
        Bets =  Set<AuctionBet>();
        SupportBranches = Set<SupportBranch>();
        SupportMessages = Set<SupportBranchMessage>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        //Устанавливаем автогенерируемые идентификаторы всем сущностям
        modelBuilder.Entity<AuctionBet>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Auction>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<AuctionLot>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Donate>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<SupportBranch>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<SupportBranchMessage>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        
        // Подписки
        modelBuilder.Entity<User>()
            .HasMany(x => x.Subscriptions)
            .WithMany();
        // Модераторы
        modelBuilder.Entity<Streamer>()
            .HasMany(x => x.Moderators)
            .WithMany(x => x.ModeratedStreamers);
        //Аукционы победители 
        modelBuilder.Entity<Auction>()
            .HasOne(x => x.Winner)
            .WithOne()
            .HasForeignKey<Auction>(x => x.WinnerId);
        
        //Аукционы с лотами
        modelBuilder.Entity<Auction>()
            .HasMany<AuctionLot>()
            .WithOne(x => x.Auction)
            .HasForeignKey(x => x.AuctionId);
        
        //Аукционы с владельцами
        modelBuilder.Entity<Auction>()
            .HasOne(x => x.Owner)
            .WithMany(x => x.Auctions)
            .HasForeignKey(x => x.OwnerId);
        
        //Аукционы и лоты
        modelBuilder.Entity<Auction>()
            .HasMany(x => x.Lots)
            .WithOne(x => x.Auction)
            .HasForeignKey(x => x.AuctionId);
        
        //Лоты и ставки
        modelBuilder.Entity<AuctionLot>()
            .HasMany(x => x.Bets)
            .WithOne(x => x.Lot)
            .HasForeignKey(x => x.LotId);
        
        //Пользователи и донаты
        modelBuilder.Entity<User>()
            .HasMany(x => x.Donates)
            .WithOne(x => x.Donator)
            .HasForeignKey(x => x.DonatorId)
            .IsRequired();
        
        //Донаты и ставки
        modelBuilder.Entity<Donate>()
            .HasOne(x => x.Bet)
            .WithMany()
            .IsRequired(false);
        
        modelBuilder.Entity<AuctionBet>()
            .HasOne(x => x.Donate)
            .WithMany()
            .IsRequired(false);

        
        //Пользователи и кошельки
        modelBuilder.Entity<User>()
            .OwnsOne(x => x.Account);
    }

}