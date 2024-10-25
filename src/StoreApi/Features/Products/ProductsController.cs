using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StoreApi.Entities;

namespace StoreApi.Features.Products;

[Route("api/v1/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IServiceManager serviceManager, ILogger<ProductsController> logger)
    {
        _serviceManager = serviceManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        _logger.LogInformation("Getting all products");
        var products = await _serviceManager
            .ProductService
            .GetAllProductsAsync();
        _logger.LogInformation("Successfully retrieved all products");
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        _logger.LogInformation($"Fetching product with id {id}");
        var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
        _logger.LogInformation($"Successfully retrieved product with id {id}");
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        // TODO: Add the new product
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product product)
    {
        // TODO: Update the product by id
        if (id != product.Id)
        {
            return BadRequest(new { Message = "Product ID mismatch" });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        // TODO: Delete the product by id
        return NoContent();
    }
}