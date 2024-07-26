using MongoDB.Driver;

namespace minimalAPIMongo.Services
{
    public class MongoDbService
    {
        /// <summary>
        /// Armazenar a configuração da aplicação:
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Armazenar uma referência ao MongoDB:
        /// </summary>
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Contém a configuração necessária para o acesso ao MongoDB:
        /// </summary>
        /// <param name="configuration"> Objeto contendo toda a configuração da aplicação </param>
        public MongoDbService(IConfiguration configuration)
        {
            // Atribui config recebida em _configuration: 
            _configuration = configuration;

            // Acessa a string de conexao:
            var connectionString = _configuration.GetConnectionString("DbConnection");
            // var connectionString = _configuration.GetConnectionString("mongodb://localhost:27017/ProductDatabase_Tarde");

            // Transforma a string obtida em MongoUrl:
            var mongoUrl = MongoUrl.Create(connectionString);

            // Cria um client:
            var mongoClient = new MongoClient(mongoUrl);

            // Obtem a referencia ao MongoDb:
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        /// <summary>
        /// Propriedade para acessar o banco de dados => retorna os dados em _database:
        /// </summary>
        public IMongoDatabase GetDatabase => _database;
    }
}
