using DAL.Contracts.Base;
using DAL.DTO;
using DAL.DTO.Identity;

namespace DAL.Contracts.App;

public interface IAppRefreshTokenRepository : IBaseRepository<DAL.DTO.Identity.AppRefreshToken>, 
    IAppRefreshTokenRepositoryCustom<DAL.DTO.Identity.AppRefreshToken>
{
    // add here custom methods for repo only
    Task<IEnumerable<DAL.DTO.Identity.AppRefreshToken>> GetAllUserRefreshTokens(Guid appUserId);
    Task<IEnumerable<AppRefreshToken>> GetAppUsersRefreshTokens(Guid appUserId ,string logoutRefreshToken);
    Task<IEnumerable<AppRefreshToken>> LoadAndCompareRefreshTokens(Guid appUserId, string refreshToken, DateTime utcNow);
}

public interface IAppRefreshTokenRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
}