namespace SimplePOS.Business.Mapping;
using AutoMapper;
using SimplePOS.Domain.Entities;
using SimplePOS.Business.DTOs;

public class SimplePOSProfile : Profile
{
    public SimplePOSProfile()
    {
        //SaleItem mapeos
        CreateMap<SaleItem, SaleItemReadDto>()
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null));
        CreateMap<SaleItemCreateDto, SaleItem>();
        CreateMap<SaleItemUpdateDto, SaleItem>().ReverseMap();

        //Category mapeos
        CreateMap<Category, CategoryReadDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>().ReverseMap();

        //Client mapeos
        CreateMap<Client, ClientReadDto>();
        CreateMap<ClientCreateDto, Client>();
        CreateMap<ClientUpdateDto, Client>().ReverseMap();

        //Product mapeos
        CreateMap<Product, ProductReadDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<ProductCreateDto,  Product>();
        CreateMap<ProductUpdateDto, Product>().ReverseMap();

        //Sale mapeos
        CreateMap<Sale, SaleReadDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.SaleItem))
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.Name : null));
        CreateMap<SaleCreateDto, Sale>()
            .ForMember(dest => dest.SaleItem, opt => opt.MapFrom(src => src.Items));
        CreateMap<SaleUpdateDto, Sale>().ReverseMap();
    }
}