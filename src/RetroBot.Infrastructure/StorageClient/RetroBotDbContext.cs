using Microsoft.EntityFrameworkCore;
using RetroBot.Core;
using RetroBot.Infrastructure.StorageClient.Configurations;

namespace RetroBot.Infrastructure.StorageClient;

public sealed class RetroBotDbContext : DbContext
{
    //private readonly DatabaseOptions databaseOptions;
    
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;

    public RetroBotDbContext(/*DatabaseOptions databaseOptions*/)
    {
        //this.databaseOptions = databaseOptions ?? throw new ArgumentNullException(nameof(databaseOptions));
        
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new UserSetConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-M4AV3Q2;Database=helloappdb;Trusted_Connection=True;");
    }
}