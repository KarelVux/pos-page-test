using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class ProductPictureService :
    BaseEntityService<BllDto.ProductPicture, DalDto.ProductPicture, IProductPictureRepository>,
    IProductPictureService
{
    protected IAppUOW Uow;

    public ProductPictureService(IAppUOW uow, IMapper<BllDto.ProductPicture, DalDto.ProductPicture> mapper)
        : base(uow.ProductPictureRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BllDto.ProductPicture>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    public async Task<BllDto.ProductPicture?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

    /// <summary>
    /// Find product picture by picture id  
    /// </summary>
    /// <param name="pictureId">Product ID</param>
    /// <returns>Searchable result</returns>
    public async Task<BllDto.ProductPicture?> FindViaPictureId(Guid pictureId)
    {
        return Mapper.Map(await Repository.FindViaPictureId(pictureId));
    }

    /// <summary>
    /// Find product picture by product id  
    /// </summary>
    /// <param name="productId">product ID</param>
    /// <returns>Searchable result</returns>
    public async Task<BllDto.ProductPicture?> FindViaProductId(Guid productId)
    {
        return Mapper.Map(await Repository.FindViaProductId(productId));
    }
}