using AutoMapper;
using CustomerService.DTO;
using CustomerService.Model;
using Messaging.Shared.Events;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CustomerService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Customer, CustomerCreated>().ReverseMap();
            CreateMap<Customer, CustomerUpdated>().ReverseMap();
            CreateMap<Customer, CustomerDeleted>().ReverseMap();
        }
    }
}
