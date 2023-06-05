using AutoMapper;
using PublicDtoV1Shop = Public.DTO.v1.Shop;
using PublicDtoV1Management = Public.DTO.v1.Management;
using PublicDtoV1Manager = Public.DTO.v1.Manager;
using BllDto = BLL.DTO;

namespace Public.DTO;

/// <summary>
/// Automapper configuration
/// </summary>
public class AutomapperConfig : Profile
{
    /// <summary>
    /// Automapper configuration
    /// </summary>
    public AutomapperConfig()
    {
        CreateMap<BllDto.Settlement, PublicDtoV1Management.Settlement>().ReverseMap();
        CreateMap<BllDto.Picture, PublicDtoV1Management.Picture>().ReverseMap();
        CreateMap<BllDto.ProductCategory, PublicDtoV1Management.ProductCategory>().ReverseMap();
        CreateMap<BllDto.BusinessCategory, PublicDtoV1Management.BusinessCategory>().ReverseMap();
        CreateMap<BllDto.Business, PublicDtoV1Management.Business>().ReverseMap();
        CreateMap<BllDto.Product, PublicDtoV1Management.Product>().ReverseMap();
        CreateMap<BllDto.Order, PublicDtoV1Management.Order>().ReverseMap();
        CreateMap<BllDto.Invoice, PublicDtoV1Management.Invoice>().ReverseMap();
        
        
        
        
        CreateMap<BllDto.Business, PublicDtoV1Shop.Business>().ReverseMap();
        CreateMap<BllDto.Settlement, PublicDtoV1Shop.Settlement>().ReverseMap();
        CreateMap<BllDto.BusinessCategory, PublicDtoV1Shop.BusinessCategory>().ReverseMap();
        CreateMap<BllDto.Product, PublicDtoV1Shop.Product>().ReverseMap();
        CreateMap<BllDto.ProductCategory, PublicDtoV1Shop.ProductCategory>().ReverseMap();
        CreateMap<BllDto.Invoice, PublicDtoV1Shop.Invoice>().ReverseMap();
        CreateMap<BllDto.InvoiceRow, PublicDtoV1Shop.InvoiceRow>().ReverseMap();
        CreateMap<BllDto.Order, PublicDtoV1Shop.Order>().ReverseMap();
        
        
        
        
        CreateMap<BllDto.Business, PublicDtoV1Manager.Business>().ReverseMap();
        CreateMap<BllDto.BusinessCategory, PublicDtoV1Manager.BusinessCategory>().ReverseMap();
        CreateMap<BllDto.BusinessManager, PublicDtoV1Manager.BusinessManager>().ReverseMap();
        CreateMap<BllDto.BusinessPicture, PublicDtoV1Manager.BusinessPicture>().ReverseMap();
        CreateMap<BllDto.Invoice, PublicDtoV1Manager.Invoice>().ReverseMap();
        CreateMap<BllDto.InvoiceRow, PublicDtoV1Manager.InvoiceRow>().ReverseMap();
        CreateMap<BllDto.Order, PublicDtoV1Manager.Order>().ReverseMap();
        CreateMap<BllDto.OrderFeedback, PublicDtoV1Manager.OrderFeedback>().ReverseMap();
        CreateMap<BllDto.Picture, PublicDtoV1Manager.Picture>().ReverseMap();
        CreateMap<BllDto.Product, PublicDtoV1Manager.Product>().ReverseMap();
        CreateMap<BllDto.ProductCategory, PublicDtoV1Manager.ProductCategory>().ReverseMap();
        CreateMap<BllDto.ProductPicture, PublicDtoV1Manager.ProductPicture>().ReverseMap();
        CreateMap<BllDto.Settlement, PublicDtoV1Manager.Settlement>().ReverseMap();
        CreateMap<BllDto.Invoice, PublicDtoV1Manager.InvoiceLimitedEdit>().ReverseMap();
    }
}