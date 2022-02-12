using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient.Mappers;

public class UserMapper : BsonClassMap<User>
{
    public UserMapper()
    {
        AutoMap();
        MapIdMember(e => e.Id)
            .SetIgnoreIfDefault(true)
            .SetSerializer(new Int64Serializer(BsonType.Int64));
    }
}