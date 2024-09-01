using AutoMapper;
using Messaging.Shared.Events;
using OrderService.DTO;
using OrderService.Model;

namespace OrderService.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
