using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using HelpLine.Modules.UserAccess.Domain.Roles;

namespace HelpLine.Modules.UserAccess.Infrastructure.Domain.Roles
{
    internal class RoleIdSerializer : SerializerBase<RoleId>
    {
        public override RoleId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return new RoleId(context.Reader.ReadBinaryData().ToGuid());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, RoleId value)
        {
            var data = new BsonBinaryData(value.Value);
            context.Writer.WriteBinaryData(data);
        }
    }
}