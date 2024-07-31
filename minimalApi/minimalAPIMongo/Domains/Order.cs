using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace minimalAPIMongo.Domains
{
    public class Order
    {
        [BsonId]
        [BsonIgnoreIfDefault]

        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("date")]
        public DateTime? Date { get; set; }

        [BsonElement("status")]
        public bool? Status { get; set; }

        //ref para cadastrar um pedido com produtos
        [BsonElement("productId")]
        [JsonIgnore]
        public List<string> ProductId { get; set; }


        //ref para listar pedidos
        [BsonElement("products")]
        public List<Product>? Products { get; set; }


        [BsonElement("clientId")]
        [JsonIgnore]
        public string? ClientId { get; set; }

        [BsonElement("client")]
        public Client? Client { get; set; }


        public Dictionary<string, string> AdditionalAtributes { get; set; }


        public Order()
        {
            Products = new List<Product>();
        }
    }
}