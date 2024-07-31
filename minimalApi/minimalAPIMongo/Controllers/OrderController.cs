using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using minimalAPIMongo.ViewModels;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;


        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
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
                //lista os pedidos da collection order
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();

                //para cada pedido em pedidos
                foreach (var order in orders)
                {
                    //se o produto for diferente de nulo
                    if (order.ProductId != null)
                    {
                        //se cria um filtro ("separa" os produtos que vão estar no pedido)
                        //seleciona os ids dos produtos cujo id está presente na lita order.ProductId 
                        var filter = Builders<Product>.Filter.In(p => p.Id, order.ProductId);

                        //busca os produtos que corresponde ao pedido e adiconar em order.products
                        //trás as informações do pedido.
                        order.Products = await _product.Find(filter).ToListAsync();
                    }

                    if (order.ClientId != null)
                    {
                        order.Client = await _client.Find(z => z.Id == order.ClientId).FirstOrDefaultAsync();
                    }
                }

                return Ok(orders);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Order>>> GetById(string id)
        {
            try
            {
                var order = await _order.Find(x => x.Id == id).FirstOrDefaultAsync();
                return Ok(order);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderViewModel orderViewModel)
        {
            try
            {
                Order order = new Order
                {
                    Id = orderViewModel.Id,
                    Date = orderViewModel.Date,
                    Status = orderViewModel.Status,
                    ProductId = orderViewModel.ProductId,
                    ClientId = orderViewModel.ClientId,
                    //AdditionalAtributes = orderViewModel.AdditionalAttributes
                };

                var client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return BadRequest("Cliente não encontrado!");
                }

                await _order.InsertOneAsync(order);

                order.Client = await _client.Find(z => z.Id == order.ClientId).FirstOrDefaultAsync();

                return StatusCode(201, order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpPut]
        public async Task<ActionResult> Update(string id, OrderViewModel orderViewModel)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

                var updatedOrder = new Order
                {
                    Id = orderViewModel.Id,
                    Date = orderViewModel.Date,
                    Status = orderViewModel.Status,
                    ClientId = orderViewModel.ClientId,
                    ProductId = orderViewModel.ProductId

                };

                var result = await _order.ReplaceOneAsync(filter, updatedOrder);

                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
                await _order.DeleteOneAsync(filter);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
