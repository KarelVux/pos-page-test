using AutoMapper;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<DalDto.Settlement, DomainApp.Settlement>().ReverseMap();
        CreateMap<DalDto.Picture, DomainApp.Picture>().ReverseMap();
        CreateMap<DalDto.Product, DomainApp.Product>().ReverseMap();
        CreateMap<DalDto.ProductPicture, DomainApp.ProductPicture>().ReverseMap();
        CreateMap<DalDto.ProductCategory, DomainApp.ProductCategory>().ReverseMap();
        CreateMap<DalDto.OrderFeedback, DomainApp.OrderFeedback>().ReverseMap();
        CreateMap<DalDto.Order, DomainApp.Order>().ReverseMap();
        CreateMap<DalDto.InvoiceRow, DomainApp.InvoiceRow>().ReverseMap();
        CreateMap<DalDto.Invoice, DomainApp.Invoice>().ReverseMap();
        CreateMap<DalDto.BusinessManager, DomainApp.BusinessManager>().ReverseMap();
        CreateMap<DalDto.BusinessPicture, DomainApp.BusinessPicture>().ReverseMap();
        CreateMap<DalDto.BusinessCategory, DomainApp.BusinessCategory>().ReverseMap();
        CreateMap<DalDto.Business, DomainApp.Business>().ReverseMap();
        CreateMap<DalDto.Identity.AppRole, DomainApp.Identity.AppRole>().ReverseMap();
        CreateMap<DalDto.Identity.AppUser, DomainApp.Identity.AppUser>().ReverseMap();
        CreateMap<DalDto.Identity.AppRefreshToken, DomainApp.Identity.AppRefreshToken>().ReverseMap();
    }
}