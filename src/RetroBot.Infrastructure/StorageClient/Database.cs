using MongoDB.Driver;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient;

internal class Database
{
    private readonly IMongoClient client;
    private readonly DatabaseOptions options;
    
    public IMongoCollection<User> Users => GetCollection<User>();
    public IMongoCollection<Team> Teams => GetCollection<Team>();
    public IMongoCollection<Question> Questions => GetCollection<Question>();
    public IMongoCollection<Answer> Answers => GetCollection<Answer>();

    public Database(
        IMongoClient client,
        DatabaseOptions options)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
        this.options = options ?? throw new ArgumentNullException(nameof(options));

        InitializeData();
    }
    
    private IMongoCollection<T> GetCollection<T>()
    {
        return client
            .GetDatabase(options.DatabaseName)
            .GetCollection<T>(typeof(T).Name);
    }

    private void InitializeData()
    {
        Remove(Users);
        Remove(Teams);
        Remove(Answers);
        
        InitializeQuestions();
    }

    private void Remove<TType>(IMongoCollection<TType> collection)
    {
        var entities = collection.GetAllAsync().Result;
        if (entities.Any())
        {
            collection.DeleteAllAsync().Wait();
        }
    }

    private void InitializeQuestions()
    {
        var questions = Questions.GetAllAsync().Result;
        if (questions.Any())
        {
            return;
        }
        
        Questions.InsertMany(new List<Question>
        {
            new ()
            {
                Id = Guid.NewGuid(),
                Text = "What we should stop to do?",
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Text = "What we should start to do?",
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Text = "What we should continue to do?",
            }
        });
    }
}