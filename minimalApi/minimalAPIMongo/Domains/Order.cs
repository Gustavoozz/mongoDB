using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace minimalAPIMongo.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateTime? Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        [BsonElement("product")]
        public List <Product> Products { get; set; }

        [BsonElement("client")]
        public  Client? Client { get; set; }

        public Order()
        {
            Products = new List<Product>();
        }
    }
}
