using DAL.Contracts.Base;
using DAL.DTO;
using Domain.Contracts.Base;

namespace DAL.Contracts.App;

public interface IOrderRepository : IBaseRepository<Order>, IOrderRepositoryCustom<Order>
{
    // add here custom methods for repo only
}

public interface IOrderRepositoryCustom<TEntity> : IBaseRepositoryWithInclude<TEntity>
    where TEntity : class, IDomainEntityId
{
    // add here shared methods between repo and service
}