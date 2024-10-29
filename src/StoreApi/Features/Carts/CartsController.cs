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
        public async Task<ActionResult<IEnumerable<CartReadDto>>> GetCarts()
        {
            var carts = await _serviceManager.CartService.GetCartsAsync();
            return Ok(carts);
        }

        // GET: api/carts/{id}
        [HttpGet("carts/{id:guid}")]
        public async Task<ActionResult<CartReadDto>> GetCartById(Guid id)
        {
            var cart = await _serviceManager.CartService.GetCartByIdAsync(id);
            return Ok(cart);
        }

        // GET: api/customers/{customerId}/carts
        [HttpGet("customers/{customerId:guid}/carts")]
        public async Task<ActionResult<Cart>> GetCartsByCustomerId(Guid customerId)
        {
            var cartItems = await _serviceManager.CartService.GetCartsByCustomerIdAsync(customerId);
            return Ok(cartItems);
        }

        // POST: api/customers/{customerId}/carts
        [HttpPost("customers/{customerId:guid}/carts")]
        public async Task<ActionResult<CartReadDto>> CreateCartForCustomer(Guid customerId,
            [FromBody] CartCreateDto cartCreateDto)
        {
            var cartToReturn = await _serviceManager.CartService
                .CreateCartForCustomerAsync(customerId, cartCreateDto);
            return CreatedAtAction(nameof(GetCartById), new { id = cartToReturn.Id }, cartToReturn);
        }

        // PUT: api/customers/{customerId}/carts/{id}
        [HttpPut("customers/{customerId:guid}/carts/{id:guid}")]
        public async Task UpdateCartItemForCustomer(Guid customerId, Guid id)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/carts/{cartId}
        [HttpDelete("carts/{id:guid}")]
        public async Task<ActionResult> DeleteCartItem(Guid id)
        {
            await _serviceManager.CartService.DeleteCartAsync(id);
            return NoContent();
        }
    }
}