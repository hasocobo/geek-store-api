using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoreApi.Entities;

namespace StoreApi.Features.Carts
{
    [ApiController]
    [Route("api/v1/carts")]
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCarts()
        {
            _logger.LogInformation("Getting carts");
            var categories = await _serviceManager.CartService.GetCartsAsync();
            return Ok(categories);
        }

        // GET: api/carts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCart(Guid id)
        {
            _logger.LogInformation($"Getting cart with ID: {id}");
            var category = await _serviceManager.CartService.GetCartByIdAsync(id);
            return Ok(category);
        }
    }
}