using MongoDB.Driver;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient;

public class DatabaseClient
{
    private readonly IMongoClient client;
    private readonly DatabaseOptions options;
    
    public IMongoCollection<User> Users => GetCollection<User>();
    public IMongoCollection<Team> Teams => GetCollection<Team>();
    public IMongoCollection<Question> Questions => GetCollection<Question>();
    public IMongoCollection<Answer> Answers => GetCollection<Answer>();

    public DatabaseClient(
        IMongoClient client,
        DatabaseOptions options)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.options = options ?? throw new ArgumentNullException(nameof(options));

        InitializeQuestionsCollection();
    }
    
    private IMongoCollection<T> GetCollection<T>()
    {
        return client
            .GetDatabase(options.DatabaseName)
            .GetCollection<T>(typeof(T).Name);
    }

    private void InitializeQuestionsCollection()
    {
        var questions =  Questions.Find(_ => true).ToList();
        if (questions is not null && questions.Any())
        {
            return;
        }
        
        Questions.InsertMany(new List<Question>
        {
            new Question
            {
                Id = Guid.NewGuid(),
                Text = "What we should stop to do?",
            },
            new Question
            {
                Id = Guid.NewGuid(),
                Text = "What we should start to do?",
            },
            new Question
            {
                Id = Guid.NewGuid(),
                Text = "What we should continue to do?",
            }
        });
    }
}