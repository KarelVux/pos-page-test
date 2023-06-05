using AutoMapper;
using DAL.Base;

namespace Public.DTO.Mappers.Identity;

public class AppRefreshTokenMapper: BaseMapper<BLL.DTO.Identity.AppRefreshToken, Public.DTO.v1.Identity.AppRefreshToken>
{
    public AppRefreshTokenMapper(IMapper mapper) : base(mapper)
    {
    }
}