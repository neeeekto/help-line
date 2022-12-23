using System.Collections.Generic;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.UserAccess.Domain.Roles;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.Modules.UserAccess.Domain.Users;

namespace HelpLine.Modules.UserAccess.Infrastructure.Domain.Users
{
    internal class UserIdSerializer : SerializerBase<UserId>
    {
        public override UserId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return new UserId(context.Reader.ReadBinaryData().ToGuid());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, UserId value)
        {
            var data = new BsonBinaryData(value.Value);
            context.Writer.WriteBinaryData(data);
        }
    }
}
