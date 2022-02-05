using Microsoft.EntityFrameworkCore;
using RetroBot.Core;
using RetroBot.Infrastructure.StorageClient.Configurations;

namespace RetroBot.Infrastructure.StorageClient;

public sealed class RetroBotDbContext : DbContext
{
    private readonly DatabaseOptions databaseOptions;
    
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;

    public RetroBotDbContext(DatabaseOptions databaseOptions)
    {
        this.databaseOptions = databaseOptions ?? throw new ArgumentNullException(nameof(databaseOptions));
        
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new TeamTableConfiguration())
            .ApplyConfiguration(new UserTableConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer($"Server={databaseOptions.ServerName};Database={databaseOptions.DatabaseName};Trusted_Connection=True;");
    }
}