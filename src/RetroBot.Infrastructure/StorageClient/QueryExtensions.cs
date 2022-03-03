using MongoDB.Driver;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient;

internal static class QueryExtensions
{
    public static async Task<IList<TType>> GetAllAsync<TType>(this IMongoCollection<TType> collection)
    {
        return await collection.Find(_ => true).ToListAsync();
    }

    public static async Task DeleteAllAsync<TType>(this IMongoCollection<TType> collection)
    {
        await collection.DeleteManyAsync(_ => true);
    }

    public static async Task<TType> GetByIdAsync<TType>(this IMongoCollection<TType> collection, Guid id)
        where TType : IWithGeneratedId
    {
        return await collection.Find(entity => entity.Id == id).FirstOrDefaultAsync();
    }
    
    public static async Task<TType> GetByIdAsync<TType>(this IMongoCollection<TType> collection, long id)
        where TType : IWithIssuedId
    {
        return await collection.Find(entity => entity.Id == id).FirstOrDefaultAsync();
    }

    public static async Task UpdateByGeneratedIdAsync<TType>(this IMongoCollection<TType> collection, TType entity)
        where TType : IWithGeneratedId
    {
        var filter = Builders<TType>.Filter.Eq(currentEntity => currentEntity.Id, entity.Id);
        await collection.ReplaceOneAsync(filter, entity);
    }
    
    public static async Task UpdateByIssuedIdAsync<TType>(this IMongoCollection<TType> collection, TType entity)
        where TType : IWithIssuedId
    {
        var filter = Builders<TType>.Filter.Eq(currentEntity => currentEntity.Id, entity.Id);
        await collection.ReplaceOneAsync(filter, entity);
    }
}