using AutoMapper;
using CustomerService.Data;
using CustomerService.DTO;
using CustomerService.Model;
using MassTransit;
using Messaging.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Service
{
    public class CustomerServices : ICustomerServices
    {
        private readonly CustomerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public CustomerServices(CustomerDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<CustomerDTO> CreateCustomerAsync(CustomerDTO customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            customer.CreatedAt = DateTime.UtcNow;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var customerCreated = _mapper.Map<CustomerCreated>(customer);
            await _publishEndpoint.Publish(customerCreated);

            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task UpdateCustomerAsync(CustomerDTO customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            var customerUpdated = _mapper.Map<CustomerUpdated>(customer);
            await _publishEndpoint.Publish(customerUpdated);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                var customerDeleted = _mapper.Map<CustomerDeleted>(customer);
                await _publishEndpoint.Publish(customerDeleted);
            }
        }
    }
}

