using DAL.Contracts.Base;
using DAL.DTO;
using Domain.Contracts.Base;

namespace DAL.Contracts.App;

public interface IBusinessRepository : IBaseRepository<Business>,
    IBusinessRepositoryRepositoryCustom<Business>
{
    // add here custom methods for repo only
    Task<IEnumerable<Business>> AllAsyncWithBusinessOwner(Guid userId);
}

public interface IBusinessRepositoryRepositoryCustom<TEntity> : IBaseRepositoryWithInclude<TEntity>
    where TEntity : class, IDomainEntityId
{
    // add here shared methods between repo and service
    Task<IEnumerable<TEntity>> GetBusinessesWithIncludes(Guid settlementId,
        Guid? businessCategoryId = null);
}