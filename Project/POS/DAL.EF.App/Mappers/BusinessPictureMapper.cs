using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class BusinessPictureMapper : BaseMapper<DalDto.BusinessPicture, DomainApp.BusinessPicture>
{
    public BusinessPictureMapper(IMapper mapper) : base(mapper)
    {
    }
}