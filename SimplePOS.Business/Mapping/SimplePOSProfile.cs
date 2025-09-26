using AutoMapper;
using SimplePOS.Domain.Entities;
using SimplePOS.Business.DTOs;

public class SimplePOSProfile : Profile
{
    public SimplePOSProfile()
    {
        CreateMap<SaleItem, SaleItemReadDto>()
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null));
        // Add other mappings for your entities and DTOs here
    }
}