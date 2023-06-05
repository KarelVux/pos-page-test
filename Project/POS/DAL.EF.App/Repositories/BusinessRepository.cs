using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO;
using DAL.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class BusinessRepository :
    EFBaseRepository<DAL.DTO.Business, Domain.App.Business, ApplicationDbContext>, IBusinessRepository
{
    public BusinessRepository(ApplicationDbContext dataContext, IMapper<DAL.DTO.Business, Domain.App.Business> mapper) :
        base(dataContext, mapper)
    {
    }

    public async Task<IEnumerable<DAL.DTO.Business>> AllAsyncWithInclude()
    {
        var res = await RepositoryDbSet
            .Include(b => b.BusinessCategory)
            .Include(b => b.Settlement)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    public async Task<DAL.DTO.Business?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(b => b.BusinessCategory)
            .Include(b => b.Settlement)
            .FirstOrDefaultAsync(m => m.Id == id);

        return Mapper.Map(res);
    }


    public async Task<IEnumerable<DAL.DTO.Business>> GetBusinessesWithIncludes(Guid settlementId,
        Guid? businessCategoryId = null)
    {
        var query = RepositoryDbSet.AsQueryable();

        query = query.Include(x => x.Settlement)
            .Where(x => x.Settlement!.Id.Equals(settlementId));

        if (businessCategoryId != null)
        {
            query = query.Include(x => x.BusinessCategory)
                .Where(x => x.BusinessCategory!.Id.Equals(businessCategoryId));
        }
        else
        {
            query = query.Include(x => x.BusinessCategory);
        }

        return (await query.ToListAsync()).Select(z => Mapper.Map(z)!);
    }

    public async Task<IEnumerable<Business>> AllAsyncWithBusinessOwner(Guid userId)
    {
        var res = await RepositoryDbSet
            .Include(b => b.BusinessCategory)
            .Include(b => b.Settlement)
            .Include(b => b.BusinessManagers)
            .Where(x => x.BusinessManagers != null && x.BusinessManagers.Any(y => y.AppUserId.Equals(userId)))
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }
}