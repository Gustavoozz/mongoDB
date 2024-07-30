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
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        private FilterDefinition<Order> FindById(string id)
        {
            return Builders<Order>.Filter.Eq(m => m.Id, id);
        }




        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
                return Ok(orders);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpPost]
        public async Task<ActionResult> Post(Order Order)
        {
            try
            {
                await _order!.InsertOneAsync(Order);
                return StatusCode(201, Order);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("GetById")]
        public Task<Order> GetOne(string id)
        {
            try
            {
                var filter = FindById(id);
                return _order
                        .Find(filter)
                        .FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            try
            {
            var filter = FindById(id);
                        DeleteResult deleteResult = await _order.DeleteOneAsync(filter);
                        return Ok(deleteResult.IsAcknowledged
                        && deleteResult.DeletedCount > 0);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }

        // Put - Alterar todos os atributos do obj ( Deve preencher todos os atributos da requisicao ). / Patch - Atualizar um atributo especifico do obj ( Deve preencher apenas uma tributo da requsicao ).
        [HttpPut]

        public async Task<ActionResult<bool>> Update(Order Order)
        {
            try
            {
                ReplaceOneResult updateResult =
               await _order
                       .ReplaceOneAsync(
                           filter: g => g.Id == Order.Id,
                           replacement: Order);
                return Ok(updateResult.IsAcknowledged
                        && updateResult.ModifiedCount > 0);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }



    }
}
