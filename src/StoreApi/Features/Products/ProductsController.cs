﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StoreApi.Common.DataTransferObjects.Products;
using StoreApi.Common.QueryFeatures;

namespace StoreApi.Features.Products;

[Route("api/v1/")]
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

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts(
        [FromQuery] QueryParameters queryParameters)
    {
        var (productsToReturn, metadata) = await
            _serviceManager.ProductService.GetProductsAsync(queryParameters);

        Response.Headers["StoreApi-Pagination"] = JsonSerializer.Serialize(metadata);

        return Ok(productsToReturn);
    }

    [HttpGet("products/{id:guid}")]
    public async Task<ActionResult<ProductReadDto>> GetProductById(Guid id)
    {
        var product = await _serviceManager.ProductService.GetProductByIdAsync(id);
        return Ok(product);
    }

    [HttpGet("categories/{categoryId:guid}/products")]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProductsByCategoryId(Guid categoryId,
        [FromQuery] QueryParameters queryParameters)
    {
        var (productsToReturn, metadata) = await
            _serviceManager.ProductService.GetProductsByCategoryIdAsync(categoryId, queryParameters);
        
        Response.Headers["StoreApi-Pagination"] = JsonSerializer.Serialize(metadata);
        return Ok(productsToReturn);
    }

    [HttpPost("categories/{categoryId:guid}/products")]
    public async Task<ActionResult<ProductReadDto>> CreateProduct(Guid categoryId,
        [FromBody] ProductCreateDto productCreateDto)
    {
        var productToReturn = await
            _serviceManager.ProductService.CreateProductAsync(categoryId, productCreateDto);

        return CreatedAtAction(nameof(GetProductById), new { id = productToReturn.Id }, productToReturn);
    }

    [HttpPut("products/{id:guid}")]
    public async Task<ActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto productUpdateDto)
    {
        await _serviceManager.ProductService.UpdateProductAsync(id, productUpdateDto);
        return Ok();
    }

    [HttpDelete("products/{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        await _serviceManager.ProductService.DeleteProductAsync(id);
        return NoContent();
    }
}