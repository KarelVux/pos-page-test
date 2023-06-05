using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class ProductPictureMapper : BaseMapper<DalDto.ProductPicture, DomainApp.ProductPicture>
{
    public ProductPictureMapper(IMapper mapper) : base(mapper)
    {
    }
}