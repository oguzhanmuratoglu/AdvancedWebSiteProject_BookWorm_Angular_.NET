using AutoMapper;
using BookStoreServer.Dtos;
using BookStoreServer.Models;

namespace BookStoreServer.Mapping;

public class MapProfile : Profile
{
	public MapProfile()
    {
        CreateMap<Coupon, CouponResponseDto>();
        CreateMap<OrderItemDto, OrderItem>();
        CreateMap<OrderItem, OrderItemsDto>();
        CreateMap<Order, OrderDetailResponseDto>()
            .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => null as Address))
            .ForMember(dest => dest.Coupon, opt => opt.MapFrom(src => null as Coupon))
            .ForMember(dest => dest.BillingAddress, opt => opt.MapFrom(src => null as Address))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => null as List<OrderItemsDto>))
            .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType.ToString()));
    }
}
