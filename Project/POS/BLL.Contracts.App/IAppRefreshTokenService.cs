using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IAppRefreshTokenService : IBaseRepository<BllDto.Identity.AppRefreshToken>,
    IAppRefreshTokenRepositoryCustom<BllDto.Identity.AppRefreshToken>
{
    // add your custom service methods here
    Task<IEnumerable<BllDto.Identity.AppRefreshToken>> GetAllUserRefreshTokens(Guid appUserId);
    Task<IEnumerable<BllDto.Identity.AppRefreshToken>>  GetAppUsersRefreshTokens(Guid appUserId, string logoutRefreshToken);
    Task<IEnumerable<BllDto.Identity.AppRefreshToken>> LoadAndCompareRefreshTokens(Guid appUserId, string refreshToken, DateTime utcNow);
}