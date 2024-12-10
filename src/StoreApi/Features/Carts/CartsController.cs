using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpGet("carts")]
        public async Task<ActionResult<IEnumerable<CartReadDto>>> GetCarts()
        {
            var carts = await _serviceManager.CartService.GetCartsAsync();
            return Ok(carts);
        }

        [HttpGet("carts/{id:guid}")]
        public async Task<ActionResult<CartReadDto>> GetCartItemById(Guid id)
        {
            var cart = await _serviceManager.CartService.GetCartItemByIdAsync(id);
            return Ok(cart);
        }

        [HttpGet("customers/{customerId:guid}/cart")]
        public async Task<ActionResult<Cart>> GetCartByCustomerId(Guid customerId)
        {
            var cartItems = await _serviceManager.CartService.GetCartByCustomerIdAsync(customerId);
            return Ok(cartItems);
        }

        [HttpPost("customers/{customerId:guid}/cart")]
        public async Task<ActionResult<CartReadDto>> CreateCartItemForCustomer(Guid customerId,
            [FromBody] CartCreateDto cartCreateDto)
        {
            var cartToReturn = await _serviceManager.CartService
                .CreateCartItemForCustomerAsync(customerId, cartCreateDto);
            return CreatedAtAction(nameof(GetCartItemById), new { id = cartToReturn.Id }, cartToReturn);
        }

        [HttpPut("customers/{customerId:guid}/cart/{id:guid}")]
        public async Task<ActionResult> UpdateCartItemForCustomer(Guid customerId, Guid id, CartUpdateDto cartUpdateDto)
        {
            await _serviceManager.CartService.UpdateCartItemAsync(customerId, id, cartUpdateDto);
            return Ok();
        }

        [HttpDelete("cart/{id:guid}")]
        public async Task<ActionResult> DeleteCartItemById(Guid id)
        {
            await _serviceManager.CartService.DeleteCartItemByIdAsync(id);
            return NoContent();
        }

        [HttpDelete("customers/{customerId:guid}/cart")]
        public async Task<ActionResult> DeleteCartByCustomerId(Guid customerId)
        {
            await _serviceManager.CartService.DeleteCartForCustomerAsync(customerId);
            return NoContent();
        }
    }
}