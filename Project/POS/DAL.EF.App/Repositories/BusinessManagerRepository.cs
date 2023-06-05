using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO;
using DAL.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class BusinessManagerRepository :
    EFBaseRepository<DAL.DTO.BusinessManager, Domain.App.BusinessManager, ApplicationDbContext>,
    IBusinessManagerRepository
{
    public BusinessManagerRepository(ApplicationDbContext dataContext,
        IMapper<DAL.DTO.BusinessManager, Domain.App.BusinessManager> mapper) : base(dataContext, mapper)
    {
    }

    public async Task<IEnumerable<DAL.DTO.BusinessManager>> AllAsyncWithInclude()
    {
        return (await RepositoryDbSet
                .Include(b => b.AppUser)
                .Include(b => b.Business)
                .ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<DAL.DTO.BusinessManager?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(b => b.AppUser)
            .Include(b => b.Business)
            .FirstOrDefaultAsync(m => m.Id == id);
        return Mapper.Map(res);
    }


    /// <summary>
    /// Gets user business
    /// </summary>
    /// <param name="businessId">Business ID</param>
    /// <param name="userId">User ID</param>
    /// <returns></returns>
    public async Task<BusinessManager?> GetUserManagedBusiness(Guid businessId, Guid userId)
    {
        var res = await RepositoryDbSet.FirstOrDefaultAsync(x =>
            x.BusinessId.Equals(businessId) && x.AppUserId.Equals(userId));
        return Mapper.Map(res);
    }
}