using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StoreApi.Features.Customers;

[Route("api/v1/customers")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(IServiceManager serviceManager, ILogger<CustomersController> logger)
    {
        _serviceManager = serviceManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _serviceManager.CustomerService.GetCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var customer = await _serviceManager.CustomerService.GetCustomerByIdAsync(id);
        return Ok(customer);
    }
}