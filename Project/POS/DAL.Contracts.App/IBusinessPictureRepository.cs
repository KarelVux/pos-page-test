using DAL.Contracts.Base;
using DAL.DTO;
using Domain.Contracts.Base;

namespace DAL.Contracts.App;

public interface IBusinessPictureRepository : IBaseRepository<BusinessPicture>,
    IBusinessPictureRepositoryCustom<BusinessPicture>
{
    // add here custom methods for repo only
    Task<BusinessPicture?> FindViaBusinessId(Guid businessId);
    Task<BusinessPicture?> FindViaPictureId(Guid pictureId);
}

public interface IBusinessPictureRepositoryCustom<TEntity> : IBaseRepositoryWithInclude<TEntity>
    where TEntity : class, IDomainEntityId
{
    // add here shared methods between repo and service
}