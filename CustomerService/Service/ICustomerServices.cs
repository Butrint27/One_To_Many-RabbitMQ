using CustomerService.DTO;

namespace CustomerService.Service
{
    public interface ICustomerServices
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<CustomerDTO> CreateCustomerAsync(CustomerDTO customerDto);
        Task UpdateCustomerAsync(CustomerDTO customerDto);
        Task DeleteCustomerAsync(int id);
    }
}
