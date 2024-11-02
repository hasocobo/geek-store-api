using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StoreApi.Common.DataTransferObjects.Wishlists;
using StoreApi.Entities;
using StoreApi.Features;

namespace StoreApi.Features.Wishlists
{
    [ApiController]
    [Route("api/v1/")]
    public class WishlistController : ControllerBase
    {
        private readonly ILogger<WishlistController> _logger;
        private readonly IServiceManager _serviceManager;

        public WishlistController(ILogger<WishlistController> logger, IServiceManager serviceManager)
        {
            _logger = logger;
            _serviceManager = serviceManager;
        }

        [HttpGet("wishlists")]
        public async Task<ActionResult<IEnumerable<WishlistReadDto>>> GetWishlists()
        {
            var wishlistItems = await _serviceManager.WishlistService.GetWishlistsAsync();
            return Ok(wishlistItems);
        }

        [HttpGet("wishlists/{id}")]
        public async Task<ActionResult<WishlistReadDto>> GetWishlistItemById(Guid id)
        {
            var wishlistItem = await _serviceManager.WishlistService.GetWishlistItemByIdAsync(id);

            return Ok(wishlistItem);
        }

        [HttpGet("customers/{customerId}/wishlist")]
        public async Task<ActionResult<IEnumerable<WishlistReadDto>>> GetWishlistByCustomerId(Guid customerId)
        {
            var wishlist = await 
                _serviceManager.WishlistService.GetWishlistByCustomerIdAsync(customerId);

            return Ok(wishlist);
        }

        [HttpPost("customers/{customerId}/wishlist")]
        public async Task<ActionResult> CreateWishlistItem(Guid customerId,
            [FromBody] WishlistCreateDto wishlistCreateDto)
        {
            var wishlistToReturn =
                await _serviceManager.WishlistService.CreateWishlistItemForCustomerAsync(customerId, wishlistCreateDto);

            return CreatedAtAction(nameof(GetWishlistItemById),
                new { id = wishlistToReturn.Id }, wishlistToReturn);
        }
        
        // Updating wishlist items only consist of adding or removing them from the list,
        // so no need for a put method.

        [HttpDelete("wishlists/{id}")]
        public async Task<ActionResult> DeleteWishlistItem(Guid id)
        {
            await _serviceManager.WishlistService.DeleteWishlistItemAsync(id);
            return NoContent();
        }
    }
}