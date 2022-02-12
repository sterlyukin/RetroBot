﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RetroBot.Core;

namespace RetroBot.Infrastructure.StorageClient.Mappers;

public class AnswerMapper : BsonClassMap<Answer>
{
    public AnswerMapper()
    {
        AutoMap();
        MapIdMember(e => e.Id)
            .SetIgnoreIfDefault(true)
            .SetSerializer(new GuidSerializer(BsonType.Binary));
    }
}