using DAL.Contracts.Base;
using DAL.DTO;
using Domain.Contracts.Base;

namespace DAL.Contracts.App;

public interface IBusinessManagerRepository : IBaseRepository<BusinessManager>,
    IBusinessManagerRepositoryCustom<BusinessManager>
{
    // add here custom methods for repo only
    Task<BusinessManager?> GetUserManagedBusiness(Guid businessId, Guid userId);
}

public interface IBusinessManagerRepositoryCustom<TEntity> : IBaseRepositoryWithInclude<TEntity>
    where TEntity : class, IDomainEntityId
{
    // add here shared methods between repo and service
}