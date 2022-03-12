using AutoMapper;
using StoreManagement.API.Entities;
using StoreManagement.API.Services;

namespace StoreManagement.API.Profiles
{
    public class OrderedProductDetailsProfile : Profile
    {
        public OrderedProductDetailsProfile()
        {
            CreateMap<Product, OrderedProductDetails>()
                .ForMember(
                    dest => dest.RemainingQuantity,
                    opt => opt.MapFrom(src => src.Quantity))
                .ForMember(
                    dest => dest.OrderedQuantity,
                    opt => opt.Ignore());
        }
    }
}
