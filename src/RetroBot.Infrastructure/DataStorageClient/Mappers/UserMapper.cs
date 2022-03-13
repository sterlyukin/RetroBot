using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.DataStorageClient.Mappers;

internal class UserMapper : BsonClassMap<User>
{
    public UserMapper()
    {
        AutoMap();
        MapIdMember(e => e.Id)
            .SetIgnoreIfDefault(true)
            .SetSerializer(new Int64Serializer(BsonType.Int64));
    }
}