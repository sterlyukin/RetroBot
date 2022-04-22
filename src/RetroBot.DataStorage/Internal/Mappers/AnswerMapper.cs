using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RetroBot.Core.Entities;

namespace RetroBot.DataStorage.Internal.Mappers;

internal sealed class AnswerMapper : BsonClassMap<Answer>
{
    public AnswerMapper()
    {
        AutoMap();
        MapIdMember(e => e.Id)
            .SetIgnoreIfDefault(true)
            .SetSerializer(new GuidSerializer(BsonType.Binary));
    }
}