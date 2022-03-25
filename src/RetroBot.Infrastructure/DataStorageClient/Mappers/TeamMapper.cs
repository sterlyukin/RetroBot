using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.DataStorageClient.Mappers;

internal sealed class TeamMapper : BsonClassMap<Team>
{
    public TeamMapper()
    {
        AutoMap();
        MapIdMember(e => e.Id)
            .SetIgnoreIfDefault(true)
            .SetSerializer(new GuidSerializer(BsonType.Binary));
    }
}