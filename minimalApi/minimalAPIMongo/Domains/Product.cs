using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Product
    {
        //define que está prop é id do objeto
        [BsonId]

        [BsonIgnoreIfDefault]

        //define o nome do campo no MongoDb como "_id" e o tipo como "ObjectId"
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        //Adiciona um dicionario para atributos adicionais (permite a criação de mais atributos)
        public Dictionary<string, string> AdditionalAtributes { get; set; }

        /// <summary>
        /// ao ser instÂnciado um objeto da classe Product, o atributo já virá cpom um novo dicionário, portanto, habilitado
        /// para adicionar um novo atributo.
        /// </summary>
        public Product()
        {
            AdditionalAtributes = new Dictionary<string, string>();
        }
    }
}