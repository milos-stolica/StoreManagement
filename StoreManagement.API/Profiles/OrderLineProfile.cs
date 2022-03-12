using AutoMapper;
using StoreManagement.API.Models;
using StoreManagement.API.Services;
using StoreManagement.API.Utils;

namespace StoreManagement.API.Profiles
{
    public class OrderLineProfile : Profile
    {
        public OrderLineProfile()
        {
            CreateMap<OrderedProductDetails, OrderLineDTO>()
                .ForMember(
                    dest => dest.ProductSku, 
                    opt => opt.MapFrom(src => src.Sku))
                .ForMember(
                    dest => dest.Quantity, 
                    opt => opt.MapFrom(src => MathWrapper.Round(src.OrderedQuantity, 2)))
                .ForMember(
                    dest => dest.Price, 
                    opt => opt.MapFrom(src => MathWrapper.Round(src.Price * (src.OrderedQuantity / src.UnitWeight), 2)));
        }
    }
}
