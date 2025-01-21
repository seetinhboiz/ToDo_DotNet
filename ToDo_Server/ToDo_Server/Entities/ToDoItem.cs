using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDo_Server.Entities
{
    public class ToDoItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string? Name { get; set; }

        [BsonElement("status"), BsonRepresentation(BsonType.Boolean)]
        public bool? Status { get; set; }
    }
}
