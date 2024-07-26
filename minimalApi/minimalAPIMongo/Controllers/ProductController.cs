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
    public class ProductController : ControllerBase
    {
        private readonly IMongoCollection<Product> _product;

        public ProductController(MongoDbService mongoDbService)
        {
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        private FilterDefinition<Product> FindById(string id)
        {
            return Builders<Product>.Filter.Eq(m => m.Id, id);
        }
    


        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
                return Ok(products);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

  
        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
            try
            {
                await _product!.InsertOneAsync(product);
                return StatusCode(201, product);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetById")]

        public Task<Product> GetOne(string id)
        {
            try
            {
                var filter = FindById(id);
                return _product
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
            DeleteResult deleteResult = await _product.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
            && deleteResult.DeletedCount > 0;
        }


        [HttpPut]

        public async Task<bool> Update(Product product)
        {
            ReplaceOneResult updateResult =
                await _product
                        .ReplaceOneAsync(
                            filter: g => g.Id == product.Id,
                            replacement: product);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
