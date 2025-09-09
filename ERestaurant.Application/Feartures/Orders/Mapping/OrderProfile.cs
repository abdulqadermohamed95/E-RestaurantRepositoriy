using AutoMapper;
using ERestaurant.Application.Feartures.Orders.Dtos;
using ERestaurant.Domain.Entities.Orders;

namespace ERestaurant.Application.Feartures.Orders.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateUpdateOrderDto, Order>()
                     .ForMember(dest => dest.Id, opt => opt.Ignore())
                     .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
                     .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                     .ForMember(dest => dest.TotalBeforeTax, opt => opt.Ignore())
                     .ForMember(dest => dest.TotalTax, opt => opt.Ignore())
                     .ForMember(dest => dest.TotalAfterTax, opt => opt.Ignore())
                     .ForMember(dest => dest.OrderItems, opt => opt.Ignore()); 

            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemDto>(); 
        }
    }
}
