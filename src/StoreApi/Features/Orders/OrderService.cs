using Microsoft.AspNetCore.Mvc;
using StoreApi.Common.DataTransferObjects.Orders;
using StoreApi.Entities;

namespace StoreApi.Features.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepositoryManager repositoryManager, ILogger<OrderService> logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderReadDto>> GetOrdersAsync()
        {
            _logger.LogInformation("Fetching all orders");
            var orders = await _repositoryManager.OrderRepository.GetOrdersAsync();

            _logger.LogInformation($"Returning orders by converting them to read-only data transfer objects");
            var ordersToReturn = orders.Select(o =>
                new OrderReadDto
                (
                    Id: o.Id,
                    Date: o.Date,
                    TotalPrice: o.TotalPrice(),
                    CustomerId: o.CustomerId,
                    OrderItems: o.OrderItems.Select(oi =>
                        new OrderItemReadDto
                        (
                            Id: oi.Id,
                            ProductId: oi.ProductId,
                            ProductName: oi.Product.Name,
                            UnitPrice: oi.Product.Price,
                            Quantity: oi.Quantity
                        )).ToList()
                )
            );

            return ordersToReturn;
        }

        public async Task<IEnumerable<OrderReadDto>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            _logger.LogInformation($"Fetching orders by CustomerId: {customerId}");
            var orders =
                await _repositoryManager.OrderRepository.GetOrdersByCustomerIdAsync(customerId);
            
            _logger.LogInformation($"Returning orders by converting them to read-only data transfer objects");
            var ordersToReturn = orders.Select(o =>
                new OrderReadDto
                (
                    Id: o.Id,
                    Date: o.Date,
                    TotalPrice: o.TotalPrice(),
                    CustomerId: o.CustomerId,
                    OrderItems: o.OrderItems.Select(oi =>
                        new OrderItemReadDto
                        (
                            Id: oi.Id,
                            ProductId: oi.ProductId,
                            ProductName: oi.Product.Name,
                            UnitPrice: oi.Product.Price,
                            Quantity: oi.Quantity
                        )).ToList()
                )
            );

            return ordersToReturn;
        }

        public async Task<OrderReadDto> GetOrderByIdAsync(Guid id)
        {
            _logger.LogInformation($"Fetching order by Id: {id}");
            var order = await _repositoryManager.OrderRepository.GetOrderByIdAsync(id);

            _logger.LogInformation($"Returning order with Id: {id} by converting it to read dto");
            var orderToReturn = new OrderReadDto
            (
                Id: order.Id,
                Date: order.Date,
                TotalPrice: order.TotalPrice(),
                CustomerId: order.CustomerId,
                OrderItems: order.OrderItems.Select(oi =>
                    new OrderItemReadDto
                    (
                        Id: oi.Id,
                        ProductId: oi.ProductId,
                        ProductName: oi.Product.Name,
                        UnitPrice: oi.Product.Price,
                        Quantity: oi.Quantity
                    )).ToList()
            );

            return orderToReturn;
        }

        public async Task<OrderReadDto> CreateOrderForCustomerAsync(Guid customerId, OrderCreateDto orderCreateDto)
        {
            if (orderCreateDto.OrderItems != null)
            {
                // TODO: Create an option for ordering directly without adding to cart
            }

            _logger.LogInformation($"Fetching customer with ID: {customerId}'s cart items.");
            var cartItems = await
                _repositoryManager.CartRepository.GetCartsByCustomerIdAsync(customerId);

            _logger.LogInformation($"Creating order items from each cart item.");
            var orderId = Guid.NewGuid();
            var orderItems = cartItems
                .Select(ci =>
                    new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        Quantity = ci.Quantity,
                        ProductId = ci.ProductId,
                        Product = ci.Product,
                        OrderId = orderId,
                        Price = ci.Product.Price
                    })
                .ToList();

            _logger.LogInformation($"Creating order with ID: {orderId}.");
            var order = new Order
            {
                Id = orderId,
                Date = DateTime.Now,
                CustomerId = customerId,
                OrderItems = orderItems,
                // ShipmentId = orderCreateDto.ShipmentId
                // PaymentId = orderCreateDto.PaymentId
            };
            _repositoryManager.OrderRepository.CreateOrder(order);

            _logger.LogInformation($"Saving order with ID: {orderId} to database.");
            await _repositoryManager.SaveAsync();

            _logger.LogInformation($"Converting order items to read-only data transfer object");
            var orderItemsToReturn = orderItems.Select(oi =>
                new OrderItemReadDto
                (
                    Id: oi.Id,
                    ProductId: oi.ProductId,
                    ProductName: oi.Product.Name,
                    UnitPrice: oi.Product.Price,
                    Quantity: oi.Quantity
                )
            ).ToList();

            _logger.LogInformation($"Returning by converting order to read-only data transfer object");
            var orderToReturn = new OrderReadDto
            (
                Id: order.Id,
                Date: order.Date,
                TotalPrice: order.TotalPrice(),
                CustomerId: order.CustomerId,
                OrderItems: orderItemsToReturn
            );

            return orderToReturn;
        }
    }
}