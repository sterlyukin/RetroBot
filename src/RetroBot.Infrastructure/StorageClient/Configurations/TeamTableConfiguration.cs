using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient.Configurations;

internal sealed class TeamTableConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("teams", "retro_bot");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.TeamLeadEmail)
            .HasColumnName("teamLeadEmail");
    }
}