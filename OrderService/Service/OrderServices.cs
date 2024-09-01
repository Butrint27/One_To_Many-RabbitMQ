using AutoMapper;
using MassTransit;
using Messaging.Shared.Events;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderService.Data;
using OrderService.DTO;
using OrderService.Model;

namespace OrderService.Service
{
    public class OrderServices : IOrderServices
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly HttpClient _httpClient; // Add HttpClient

        public OrderServices(OrderDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint, HttpClient httpClient)
        {
            _orders = context.Orders;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _httpClient = httpClient; // Initialize HttpClient
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orders.Find(order => true).ToListAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(string id) // Use string for OrderId
        {
            var order = await _orders.Find(o => o.OrderId == id).FirstOrDefaultAsync();
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto)
        {
            // Check if the CustomerId exists in CustomerService
            var customerExists = await CheckCustomerExists(orderDto.CustomerId);
            if (!customerExists)
            {
                throw new KeyNotFoundException($"Customer with ID {orderDto.CustomerId} does not exist.");
            }

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                ProductName = orderDto.ProductName,
                Quantity = orderDto.Quantity,
                Price = orderDto.Price,
                CreatedAt = DateTime.UtcNow
            };

            await _orders.InsertOneAsync(order);

            // Publish the OrderCreated event
            var orderCreatedEvent = new OrderCreated
            {
                OrderId = order.OrderId, // Now string
                CustomerId = order.CustomerId,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                Price = order.Price
            };
            await _publishEndpoint.Publish(orderCreatedEvent);

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<bool> UpdateOrderAsync(string id, OrderDTO orderDto) // Use string for OrderId
        {
            // Check if the CustomerId exists in CustomerService
            var customerExists = await CheckCustomerExists(orderDto.CustomerId);
            if (!customerExists)
            {
                throw new KeyNotFoundException($"Customer with ID {orderDto.CustomerId} does not exist.");
            }

            var updatedOrder = new Order
            {
                OrderId = id, // Now string
                CustomerId = orderDto.CustomerId,
                ProductName = orderDto.ProductName,
                Quantity = orderDto.Quantity,
                Price = orderDto.Price,
                CreatedAt = orderDto.CreatedAt
            };

            var result = await _orders.ReplaceOneAsync(o => o.OrderId == id, updatedOrder);

            if (result.ModifiedCount > 0)
            {
                // Publish the OrderUpdated event
                var orderUpdatedEvent = new OrderUpdated
                {
                    OrderId = updatedOrder.OrderId, // Now string
                    CustomerId = updatedOrder.CustomerId,
                    ProductName = updatedOrder.ProductName,
                    Quantity = updatedOrder.Quantity,
                    Price = updatedOrder.Price
                };
                await _publishEndpoint.Publish(orderUpdatedEvent);
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteOrderAsync(string id) // Use string for OrderId
        {
            var result = await _orders.DeleteOneAsync(o => o.OrderId == id);
            return result.DeletedCount > 0;
        }

        private async Task<bool> CheckCustomerExists(int customerId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5202/api/customers/{customerId}");
            return response.IsSuccessStatusCode;
        }
    }
}

