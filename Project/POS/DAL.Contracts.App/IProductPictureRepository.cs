using DAL.Contracts.Base;
using DAL.DTO;
using Domain.Contracts.Base;

namespace DAL.Contracts.App;

public interface IProductPictureRepository : IBaseRepository<ProductPicture>,
    IProductPictureRepositoryCustom<ProductPicture>
{
    // add here custom methods for repo only
    Task<ProductPicture?> FindProductPicture(Guid productId);
    Task<ProductPicture?> FindViaPictureId(Guid pictureId);
    Task<ProductPicture?> FindViaProductId(Guid productId);
}

public interface IProductPictureRepositoryCustom<TEntity> : IBaseRepositoryWithInclude<TEntity>
    where TEntity : class, IDomainEntityId
{
    // add here shared methods between repo and service
}