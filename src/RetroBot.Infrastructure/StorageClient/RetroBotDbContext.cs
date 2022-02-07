using Microsoft.EntityFrameworkCore;
using RetroBot.Core;
using RetroBot.Infrastructure.StorageClient.Configurations;

namespace RetroBot.Infrastructure.StorageClient;

public sealed class RetroBotDbContext : DbContext
{
    private readonly DatabaseOptions databaseOptions;
    
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Team> Teams { get; set; } = default!;
    public DbSet<Question> Questions { get; set; } = default!;
    public DbSet<Answer> Answers { get; set; } = default!;

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
            .ApplyConfiguration(new UserTableConfiguration())
            .ApplyConfiguration(new AnswerTableConfiguration())
            .ApplyConfiguration(new QuestionTableConfiguration());

        modelBuilder
            .Entity<Question>()
            .HasData(
                new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "What should we start doing in the new sprint?",
                },
                new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "What should we continue to do in the new sprint?",
                },
                new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "What should we stop doing in the new sprint?"
                }
            );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer($"Server={databaseOptions.ServerName};Database={databaseOptions.DatabaseName};Trusted_Connection=True;");
    }
}