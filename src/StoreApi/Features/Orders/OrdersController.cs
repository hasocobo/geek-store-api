using Microsoft.AspNetCore.Mvc;
using StoreApi.Common.DataTransferObjects.Orders;
using StoreApi.Entities;

namespace StoreApi.Features.Orders;

[Route("api/v1/")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IServiceManager _serviceManager;

    public OrdersController(ILogger<OrdersController> logger, IServiceManager serviceManager)
    {
        _logger = logger;
        _serviceManager = serviceManager;
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _serviceManager.OrderService.GetOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("customers/{customerId}/orders")]
    public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetOrdersByCustomerId(Guid customerId)
    {
        var orders = await _serviceManager.OrderService.GetOrdersByCustomerIdAsync(customerId);
        return Ok(orders);
    }

    [HttpGet("orders/{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        _logger.LogInformation($"Fetching order with ID: {id}");
        var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    [HttpPost("customers/{customerId}/orders")]
    public async Task<ActionResult<OrderReadDto>> CreateOrder(Guid customerId, [FromBody] OrderCreateDto orderCreateDto)
    {
        var orderToReturn =
            await _serviceManager.OrderService.CreateOrderForCustomerAsync(customerId, orderCreateDto);

        return CreatedAtAction(nameof(GetOrderById), new { id = orderToReturn.Id }, orderToReturn);
    }
}