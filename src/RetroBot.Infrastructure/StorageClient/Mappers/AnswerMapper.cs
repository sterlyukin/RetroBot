﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using RetroBot.Core;
using RetroBot.Core.Entities;

namespace RetroBot.Infrastructure.StorageClient.Mappers;

internal class AnswerMapper : BsonClassMap<Answer>
{
    public AnswerMapper()
    {
        AutoMap();
        MapIdMember(e => e.Id)
            .SetIgnoreIfDefault(true)
            .SetSerializer(new GuidSerializer(BsonType.Binary));
    }
}