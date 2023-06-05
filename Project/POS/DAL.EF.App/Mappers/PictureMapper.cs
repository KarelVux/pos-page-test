using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class PictureMapper : BaseMapper<DalDto.Picture, DomainApp.Picture>
{
    public PictureMapper(IMapper mapper) : base(mapper)
    {
    }
}