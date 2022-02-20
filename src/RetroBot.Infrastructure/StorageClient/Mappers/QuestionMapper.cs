using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient.Mappers;

internal class QuestionMapper : BsonClassMap<Question>
{
    public QuestionMapper()
    {
        AutoMap();
        MapIdMember(e => e.Id)
            .SetIgnoreIfDefault(true)
            .SetSerializer(new GuidSerializer(BsonType.Binary));
    }
}