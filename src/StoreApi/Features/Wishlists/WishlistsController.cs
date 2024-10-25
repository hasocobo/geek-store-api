using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StoreApi.Features;

namespace StoreApi.Features.Wishlists
{
    [ApiController]
    [Route("api/v1/wishlists")]
    public class WishlistController : ControllerBase
    {
        private readonly ILogger<WishlistController> _logger;
        private readonly IServiceManager _serviceManager;

        public WishlistController(ILogger<WishlistController> logger, IServiceManager serviceManager)
        {
            _logger = logger;
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlistItems()
        {
            _logger.LogInformation("Fetching wishlist items");
            var wishlistItems = await _serviceManager.WishlistService.GetWishlistsAsync();
            return Ok(wishlistItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishlistItemById(Guid id)
        {
            _logger.LogInformation($"Fetching wishlist item with id {id}");
            var wishlistItem = await _serviceManager.WishlistService.GetWishlistByIdAsync(id);

            return Ok(wishlistItem);
        }
    }
}