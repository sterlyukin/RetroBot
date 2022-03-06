using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RetroBot.Core;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.StorageClient.Mappers;

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