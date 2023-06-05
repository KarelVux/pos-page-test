using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class AppRoleMapper : BaseMapper<BllDto.Identity.AppRole, DalDto.Identity.AppRole>
{
    public AppRoleMapper(IMapper mapper) : base(mapper)
    {
    }
}