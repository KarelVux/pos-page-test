using AutoMapper;
using DAL.Base;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers;

public class AppRefreshTokenMapper : BaseMapper<BllDto.Identity.AppRefreshToken, DalDto.Identity.AppRefreshToken>
{
    public AppRefreshTokenMapper(IMapper mapper) : base(mapper)
    {
    }
}