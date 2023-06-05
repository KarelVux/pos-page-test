using AutoMapper;
using DAL.Base;
using DalDto = DAL.DTO;
using DomainApp = Domain.App;

namespace DAL.EF.App.Mappers;

public class AppRoleMapper : BaseMapper<DalDto.Identity.AppRole, DomainApp.Identity.AppRole>
{
    public AppRoleMapper(IMapper mapper) : base(mapper)
    {
    }
}