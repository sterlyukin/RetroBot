namespace RetroBot.Infrastructure.EmailClient;

public sealed class EmailOptions
{
    public string FromEmail { get; set; } = default!;
    public string EmailPassword { get; set; } = default!;
    public string FromDisplayName { get; set; } = default!;
    public string Host { get; set; } = default!;
    public int Port { get; set; } = default!;
    public string Subject { get; set; } = default!;
}