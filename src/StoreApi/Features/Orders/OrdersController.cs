using Microsoft.AspNetCore.Mvc;
using StoreApi.Entities;

namespace StoreApi.Features.Orders;

[Route("api/v1/orders")]
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

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        _logger.LogInformation("Fetching all orders");
        var orders = await _serviceManager.OrderService.GetOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        _logger.LogInformation($"Fetching order with ID: {id}");
        var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        _logger.LogInformation($"Creating order with ID: {order.Id}");
        await _serviceManager.OrderService.CreateOrderAsync(order);
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }
    
}