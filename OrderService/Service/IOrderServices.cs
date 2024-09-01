using OrderService.DTO;

namespace OrderService.Service
{
    public interface IOrderServices
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(string id); // Use string for OrderId
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDto);
        Task<bool> UpdateOrderAsync(string id, OrderDTO orderDto); // Use string for OrderId
        Task<bool> DeleteOrderAsync(string id); // Use string for OrderId
    }
}
