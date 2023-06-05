using DAL.Contracts.Base;
using DAL.DTO;
using Domain.Contracts.Base;

namespace DAL.Contracts.App;

public interface IProductRepository : IBaseRepository<Product>, IProductRepositoryCustom<Product>
{
    // add here custom methods for repo only
}

public interface IProductRepositoryCustom<TEntity> : IBaseRepositoryWithInclude<TEntity>
    where TEntity : class, IDomainEntityId
{
    // add here shared methods between repo and service
    Task<IEnumerable<TEntity>> GetBusinessProductsWithIncludes(Guid businessId, bool isFrozen);
    Task<TEntity?> GetBusinessProduct(Guid productId, Guid businessId, bool isFrozen);
    Task<IEnumerable<TEntity>> GetBusinessProductsWithIncludes(Guid businessId);
    Task<TEntity?> GetBusinessProduct(Guid productId, Guid businessId);
}