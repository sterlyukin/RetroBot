namespace RetroBot.Infrastructure.StorageClient;

public sealed class DatabaseOptions
{
    public string ServerName { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}