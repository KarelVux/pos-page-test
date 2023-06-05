using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class AppRefreshTokenMapper : BaseMapper<DalDto.Identity.AppRefreshToken, DomainApp.Identity.AppRefreshToken>
{
    public AppRefreshTokenMapper(IMapper mapper) : base(mapper)
    {
    }
}