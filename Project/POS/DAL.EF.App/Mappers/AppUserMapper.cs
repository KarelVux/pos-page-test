using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class AppUserMapper : BaseMapper<DalDto.Identity.AppUser, DomainApp.Identity.AppUser>
{
    public AppUserMapper(IMapper mapper) : base(mapper)
    {
    }
}