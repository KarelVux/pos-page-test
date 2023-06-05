using Domain.Contracts.Base;

namespace DAL.Contracts.Base;

public interface IBaseRepositoryWithInclude<TEntity> : IBaseRepositoryWithInclude<TEntity, Guid>
    where TEntity : class, IDomainEntityId
{
}

public interface IBaseRepositoryWithInclude<TEntity, in TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    Task<IEnumerable<TEntity>> AllAsyncWithInclude();
    Task<TEntity?> FindAsyncWithInclude(TKey id);
}