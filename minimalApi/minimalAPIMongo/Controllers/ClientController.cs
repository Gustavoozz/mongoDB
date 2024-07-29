using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client> _client;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
        }

        private FilterDefinition<Client> FindById(string id)
        {
            return Builders<Client>.Filter.Eq(m => m.Id, id);
        }


        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var clients = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(clients);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpPost]
        public async Task<ActionResult> Post(Client client)
        {
            try
            {
                await _client!.InsertOneAsync(client);
                return StatusCode(201, client);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("GetById")]
        public Task<Client> GetOne(string id)
        {
            try
            {
                var filter = FindById(id);
                return _client
                        .Find(filter)
                        .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpDelete]
        public async Task<bool> Delete(string id)
        {
            try
            {
            var filter = FindById(id);
            DeleteResult deleteResult = await _client.DeleteOneAsync(filter);
            return (deleteResult.IsAcknowledged
            && deleteResult.DeletedCount > 0);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        // Put - Alterar todos os atributos do obj ( Deve preencher todos os atributos da requisicao ). / Patch - Atualizar um atributo especifico do obj ( Deve preencher apenas uma tributo da requsicao ).
        [HttpPut]

        public async Task<bool> Update(Client client)
        {
            try
            {
                ReplaceOneResult updateResult =
               await _client
                       .ReplaceOneAsync(
                           filter: g => g.Id == client.Id,
                           replacement: client);
                return updateResult.IsAcknowledged
                        && updateResult.ModifiedCount > 0;
            }
            catch (Exception)   
            {

                throw;
            }
        }
    }
}
