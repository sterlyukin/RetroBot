using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient.Configurations;

internal sealed class UserSetConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "retro_bot");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();
        
        builder.Property(e => e.State)
            .HasColumnName("state")
            .IsRequired();
    }
}