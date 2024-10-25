using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        _logger.LogInformation($"Fetching order with ID: {id}");
        var order = await _serviceManager.OrderService.GetOrderByIdAsync(id);
        return Ok(order);
    }
}