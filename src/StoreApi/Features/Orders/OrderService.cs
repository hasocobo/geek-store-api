using StoreApi.Common.DataTransferObjects.Orders;
using StoreApi.Entities;
using StoreApi.Entities.Exceptions;

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
            var orders =
                await _repositoryManager.OrderRepository.GetOrdersAsync();

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
                            ProductName: oi.Product!.Name,
                            UnitPrice: oi
                                .Price, // not oi.Product.Price because then it results in inconsistency in case of a price change.
                            Quantity: oi.Quantity
                        )).ToList()
                )
            );

            return ordersToReturn;
        }

        public async Task<IEnumerable<OrderReadDto>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            if (!await _repositoryManager.CustomerRepository.CheckIfCustomerExists(customerId))
                throw new NotFoundException("Customer", customerId);

            _logger.LogInformation($"Fetching orders by CustomerId: {customerId}.");
            var orders =
                await _repositoryManager.OrderRepository.GetOrdersByCustomerIdAsync(customerId);

            _logger.LogInformation($"Returning orders.");
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
                            ProductName: oi.Product!.Name,
                            UnitPrice: oi.Price,
                            Quantity: oi.Quantity
                        )).ToList()
                )
            );

            return ordersToReturn;
        }

        public async Task<OrderReadDto> GetOrderByIdAsync(Guid id)
        {
            _logger.LogInformation($"Fetching order by Id: {id}");
            var order =
                await _repositoryManager.OrderRepository.GetOrderByIdAsync(id);
            if (order is null)
                throw new NotFoundException("Order", id);

            _logger.LogInformation($"Returning order with Id: {id}.");
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
                        ProductName: oi.Product!.Name,
                        UnitPrice: oi.Price,
                        Quantity: oi.Quantity
                    )).ToList()
            );

            return orderToReturn;
        }

        public async Task<OrderReadDto> CreateOrderForCustomerAsync(Guid customerId, OrderCreateDto orderCreateDto)
        {
            if (!await _repositoryManager.CustomerRepository.CheckIfCustomerExists(customerId))
                throw new NotFoundException("Customer", customerId);

            if (orderCreateDto.OrderItems != null)
            {
                // TODO: Create an option for ordering directly without adding to cart
            }

            _logger.LogInformation($"Fetching customer with ID: {customerId}'s cart items.");
            var cartItems = (await
                _repositoryManager.CartRepository.GetCartsByCustomerIdAsync(customerId)).ToList();

            if (cartItems.Count == 0)
                throw new NotFoundException("CartItems for customer", customerId);

            _logger.LogInformation($"Beginning transaction for user registration.");
            await using var transaction = await _repositoryManager.BeginTransactionAsync();
            try
            {
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
                            Price = ci.Product!.Price
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

                _logger.LogInformation($"Deleting cart items for customer: {customerId}.");
                foreach (var cartItem in cartItems)
                {
                    _repositoryManager.CartRepository.DeleteCart(cartItem);
                }

                _logger.LogInformation($"Saving changes and committing transaction.");
                await _repositoryManager.SaveAsync();
                await transaction.CommitAsync();

                _logger.LogInformation($"Converting order items to data transfer objects.");
                var orderItemsToReturn = orderItems.Select(oi =>
                    new OrderItemReadDto
                    (
                        Id: oi.Id,
                        ProductId: oi.ProductId,
                        ProductName: oi.Product!.Name,
                        UnitPrice: oi.Price,
                        Quantity: oi.Quantity
                    )
                ).ToList();

                _logger.LogInformation($"Returning order.");
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

            catch (Exception e)
            {
                _logger.LogError($"An error occured while saving order data: {e.Message}, rolling back transaction.");
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            _logger.LogInformation($"Fetching order by ID: {orderId} to delete");
            var orderToDelete =
                await _repositoryManager.OrderRepository.GetOrderByIdAsync(orderId);
            if (orderToDelete is null)
                throw new NotFoundException("Order", orderId);

            _logger.LogInformation($"Deleting order with ID: {orderId}.");
            _repositoryManager.OrderRepository.DeleteOrder(orderToDelete);

            await _repositoryManager.SaveAsync();
        }
    }
}