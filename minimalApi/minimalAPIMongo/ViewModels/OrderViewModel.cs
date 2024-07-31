using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace minimalAPIMongo.ViewModels
{
    public class OrderViewModel
    {
        [BsonId]

        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime? Date { get; set; }
        public bool? Status { get; set; }

        public List<string>? ProductId { get; set; }
        public string? ClientId { get; set; }
        //public Dictionary<string, string>? AdditionalAttributes { get; set; }
    }
}