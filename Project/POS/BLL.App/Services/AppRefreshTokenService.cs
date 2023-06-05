using BLL.Base;
using BLL.Contracts.App;
using BLL.DTO.Identity;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class AppRefreshTokenService :
    BaseEntityService<BllDto.Identity.AppRefreshToken, DalDto.Identity.AppRefreshToken, IAppRefreshTokenRepository>,
    IAppRefreshTokenService
{
    protected IAppUOW Uow;

    public AppRefreshTokenService(IAppUOW uow,
        IMapper<BllDto.Identity.AppRefreshToken, DalDto.Identity.AppRefreshToken> mapper)
        : base(uow.AppRefreshTokenRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<AppRefreshToken>> GetAllUserRefreshTokens(Guid appUserId)
    {
        return (await Repository.GetAllUserRefreshTokens(appUserId)).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<AppRefreshToken>> GetAppUsersRefreshTokens(Guid appUserId, string logoutRefreshToken)
    {
        return (await Repository.GetAppUsersRefreshTokens(appUserId, logoutRefreshToken)).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<AppRefreshToken>> LoadAndCompareRefreshTokens(Guid appUserId, string refreshToken, DateTime utcNow)
    {
        return (await Repository.LoadAndCompareRefreshTokens(appUserId, refreshToken, utcNow)).Select(e => Mapper.Map(e))!;
    }
}