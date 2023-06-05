using AutoMapper;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<BllDto.Settlement, DalDto.Settlement>().ReverseMap();
        CreateMap<BllDto.Picture, DalDto.Picture>().ReverseMap();
        CreateMap<BllDto.Product, DalDto.Product>().ReverseMap();
        CreateMap<BllDto.ProductPicture, DalDto.ProductPicture>().ReverseMap();
        CreateMap<BllDto.ProductCategory, DalDto.ProductCategory>().ReverseMap();
        CreateMap<BllDto.OrderFeedback, DalDto.OrderFeedback>().ReverseMap();
        CreateMap<BllDto.Order, DalDto.Order>().ReverseMap();
        CreateMap<BllDto.InvoiceRow, DalDto.InvoiceRow>().ReverseMap();
        CreateMap<BllDto.Invoice, DalDto.Invoice>().ReverseMap();
        CreateMap<BllDto.BusinessManager, DalDto.BusinessManager>().ReverseMap();
        CreateMap<BllDto.BusinessPicture, DalDto.BusinessPicture>().ReverseMap();
        CreateMap<BllDto.BusinessCategory, DalDto.BusinessCategory>().ReverseMap();
        CreateMap<BllDto.Business, DalDto.Business>().ReverseMap();
        
        CreateMap<BllDto.Identity.AppRole, DalDto.Identity.AppRole>().ReverseMap();
        CreateMap<BllDto.Identity.AppUser, DalDto.Identity.AppUser>().ReverseMap();
        CreateMap<BllDto.Identity.AppRefreshToken, DalDto.Identity.AppRefreshToken>().ReverseMap();
        
        
    }
}