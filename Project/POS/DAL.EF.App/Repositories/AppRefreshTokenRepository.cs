using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO.Identity;
using DAL.EF.Base;
using Domain.App;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class AppRefreshTokenRepository :
    EFBaseRepository<DAL.DTO.Identity.AppRefreshToken, Domain.App.Identity.AppRefreshToken, ApplicationDbContext>,
    IAppRefreshTokenRepository
{
    public AppRefreshTokenRepository(ApplicationDbContext dataContext,
        IMapper<DAL.DTO.Identity.AppRefreshToken, Domain.App.Identity.AppRefreshToken> mapper) : base(dataContext,
        mapper)
    {
    }

    public async Task<IEnumerable<AppRefreshToken>> GetAllUserRefreshTokens(Guid appUserId)
    {
        var res = (await RepositoryDbSet
            .Where(t => t.AppUserId == appUserId)
            .ToListAsync());

        return res.Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<AppRefreshToken>> LoadAndCompareRefreshTokens(Guid appUserId, string refreshToken,
        DateTime utcNow)
    {
        /*
        await _context.Entry(appUser).Collection(u => u.AppRefreshTokens!)
            .Query()
            .Where(x =>
                (x.RefreshToken == refreshTokenModel.RefreshToken && x.ExpirationDT > DateTime.UtcNow) ||
                (x.PreviousRefreshToken == refreshTokenModel.RefreshToken &&
                 x.PreviousExpirationDT > DateTime.UtcNow)
            )
            .ToListAsync();
        
        */

        var res = (await RepositoryDbSet
            .Where(t => t.AppUserId == appUserId && (
                (t.RefreshToken == refreshToken && t.ExpirationDT > utcNow) ||
                (t.PreviousRefreshToken == refreshToken &&
                 t.PreviousExpirationDT > utcNow)
            ))
            .ToListAsync());

        return res.Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<AppRefreshToken>> GetAppUsersRefreshTokens(Guid appUserId, string logoutRefreshToken)
    {
        var res = (await RepositoryDbSet
            .Where(t => t.AppUserId == appUserId && (
                (t.RefreshToken == logoutRefreshToken) ||
                (t.PreviousRefreshToken == logoutRefreshToken)
            ))
            .ToListAsync());

        return res.Select(x => Mapper.Map(x)!);
    }
}