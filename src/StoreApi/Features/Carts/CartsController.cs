using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoreApi.Common.DataTransferObjects.Carts;
using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    [ApiController]
    [Route("api/v1")]
    public class CartsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<CartsController> _logger;

        public CartsController(IServiceManager serviceManager, ILogger<CartsController> logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        // GET: api/carts
        [HttpGet("carts")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCarts()
        {
            _logger.LogInformation("Getting carts");
            var categories = await _serviceManager.CartService.GetCartsAsync();
            return Ok(categories);
        }

        // GET: api/carts/{id}
        [HttpGet("carts/{id:guid}")]
        public async Task<ActionResult<Category>> GetCartById(Guid id)
        {
            _logger.LogInformation($"Getting cart with ID: {id}");
            var category = await _serviceManager.CartService.GetCartByIdAsync(id);
            return Ok(category);
        }

        // GET: api/customers/{customerId}/carts
        [HttpGet("customers/{customerId:guid}/carts")]
        public async Task<ActionResult<Cart>> GetCartsByCustomerId(Guid customerId)
        {
            throw new NotImplementedException();
        }

        // POST: api/customers/{customerId}/carts
        [HttpPost("customers/{customerId:guid}/carts")]
        public async Task<ActionResult> CreateCartForCustomer(Guid customerId, [FromBody] CartCreateDto cartCreateDto)
        {
            _logger.LogInformation($"Adding cart with ID: {cartCreateDto} for Customer: {customerId}");
            throw new NotImplementedException();
            // return CreatedAtAction(nameof(GetCartsByCustomerId), new { id = cart.Id }, cart);
        }

        // PUT: api/customers/{customerId}/cars/{id}
        [HttpPut("customers/{customerId:guid}/carts/{id:guid}")]
        public async Task UpdateCartForCustomer(Guid customerId, Guid id)
        {
            _logger.LogInformation($"Updating cart with ID: {id} for Customer: {customerId}");
            throw new NotImplementedException();
        }

        // DELETE: api/customers/{customerId}/carts/{id}
        [HttpDelete("customers/{customerId:guid}/carts/{id:guid}")]
        public async Task DeleteCartForCustomer(Guid customerId, Guid id)
        {
            _logger.LogInformation($"Deleting cart with ID: {id} for Customer: {customerId}");
            throw new NotImplementedException();
        }
    }
}