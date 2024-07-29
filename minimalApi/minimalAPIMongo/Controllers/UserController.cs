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
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        private FilterDefinition<User> FindById(string id)
        {
            return Builders<User>.Filter.Eq(m => m.Id, id);
        }




        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var users = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(users);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpPost]
        public async Task<ActionResult> Post(User user)
        {
            try
            {
                await _user!.InsertOneAsync(user);
                return StatusCode(201, user);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("GetById")]
        public Task<User> GetOne(string id)
        {
            try
            {
                var filter = FindById(id);
                return _user
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
            var filter = FindById(id);
            DeleteResult deleteResult = await _user.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
            && deleteResult.DeletedCount > 0;
        }

        // Put - Alterar todos os atributos do obj ( Deve preencher todos os atributos da requisicao ). / Patch - Atualizar um atributo especifico do obj ( Deve preencher apenas uma tributo da requsicao ).
        [HttpPut]

        public async Task<bool> Update(User user)
        {
            try
            {
                ReplaceOneResult updateResult =
               await _user
                       .ReplaceOneAsync(
                           filter: g => g.Id == user.Id,
                           replacement: user);
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


