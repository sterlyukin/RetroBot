namespace RetroBot.Infrastructure.DataStorageClient;

public sealed class DatabaseOptions
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}