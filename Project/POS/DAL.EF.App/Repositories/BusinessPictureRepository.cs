using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO;
using DAL.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class BusinessPictureRepository :
    EFBaseRepository<DAL.DTO.BusinessPicture, Domain.App.BusinessPicture, ApplicationDbContext>,
    IBusinessPictureRepository
{
    public BusinessPictureRepository(ApplicationDbContext dataContext,
        IMapper<DAL.DTO.BusinessPicture, Domain.App.BusinessPicture> mapper) : base(dataContext, mapper)
    {
    }

    public async Task<IEnumerable<DAL.DTO.BusinessPicture>> AllAsyncWithInclude()
    {
        var res = await RepositoryDbSet
            .Include(b => b.Business)
            .Include(b => b.Picture)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    public async Task<DAL.DTO.BusinessPicture?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(b => b.Business)
            .Include(b => b.Picture)
            .FirstOrDefaultAsync(m => m.Id == id);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Find business picture by business ID
    /// </summary>
    /// <param name="businessId">Business ID</param>
    /// <returns>Searched picture</returns>
    public async Task<BusinessPicture?> FindViaBusinessId(Guid businessId)
    {
        var res = await RepositoryDbSet
            .Include(b => b.Picture)
            .FirstOrDefaultAsync(m => m.BusinessId == businessId);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Find business picture by picture ID
    /// </summary>
    /// <param name="pictureId">Picture ID</param>
    /// <returns>Searched picture</returns>
    public async Task<BusinessPicture?> FindViaPictureId(Guid pictureId)
    {
        var res = await RepositoryDbSet
            .Include(b => b.Picture)
            .FirstOrDefaultAsync(m => m.PictureId == pictureId);

        return Mapper.Map(res);
    }
}