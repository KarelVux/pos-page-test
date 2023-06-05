using Contracts.Base;
using DAL.Contracts.App;
using DAL.DTO;
using DAL.EF.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Repositories;

public class ProductPictureRepository :
    EFBaseRepository<DAL.DTO.ProductPicture, Domain.App.ProductPicture, ApplicationDbContext>,
    IProductPictureRepository
{
    public ProductPictureRepository(ApplicationDbContext dataContext,
        IMapper<DAL.DTO.ProductPicture, Domain.App.ProductPicture> mapper) : base(dataContext, mapper)
    {
    }

    public async Task<IEnumerable<DAL.DTO.ProductPicture>> AllAsyncWithInclude()
    {
        var res = await RepositoryDbSet
            .Include(p => p.Picture)
            .Include(p => p.Product)
            .ToListAsync();

        return res.Select(x => Mapper.Map(x)!);
    }

    public async Task<DAL.DTO.ProductPicture?> FindAsyncWithInclude(Guid id)
    {
        var res = await RepositoryDbSet
            .Include(p => p.Picture)
            .Include(p => p.Product)
            .FirstOrDefaultAsync(m => m.Id == id);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Find product picture by product ID
    /// </summary>
    /// <param name="productId">ProductId ID</param>
    /// <returns>Searched picture</returns>
    public async Task<ProductPicture?> FindProductPicture(Guid productId)
    {
        var res = await RepositoryDbSet
            .Include(b => b.Picture)
            .FirstOrDefaultAsync(m => m.ProductId == productId);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Find product picture by picture ID
    /// </summary>
    /// <param name="pictureId">Picture ID</param>
    /// <returns>Searchable result</returns>
    public async Task<ProductPicture?> FindViaPictureId(Guid pictureId)
    {
        var res = await RepositoryDbSet
            .Include(b => b.Picture)
            .FirstOrDefaultAsync(m => m.PictureId == pictureId);

        return Mapper.Map(res);
    }

    /// <summary>
    /// Find product picture by product ID
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <returns>Searchable result</returns>
    public async Task<ProductPicture?> FindViaProductId(Guid productId)
    {
        var res = await RepositoryDbSet
            .Include(b => b.Picture)
            .FirstOrDefaultAsync(m => m.ProductId == productId);

        return Mapper.Map(res);    }
}