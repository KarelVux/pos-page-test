using Contracts.Base;
using DAL.Contracts.Base;
using Domain.Contracts.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Base;

public class
    EFBaseRepository<TDalEntity, TDomainEntity, TDbContext> : EFBaseRepository<TDalEntity, TDomainEntity, Guid,
        TDbContext>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TDomainEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext
{
    public EFBaseRepository(TDbContext dataContext, IMapper<TDalEntity, TDomainEntity> mapper) : base(dataContext,
        mapper)
    {
    }
}

public class EFBaseRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : IBaseRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity : class, IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
{
    protected TDbContext RepositoryDbContext;
    protected DbSet<TDomainEntity> RepositoryDbSet;
    protected readonly IMapper<TDalEntity, TDomainEntity> Mapper;

    public EFBaseRepository(TDbContext dataContext, IMapper<TDalEntity, TDomainEntity> mapper)
    {
        RepositoryDbContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        Mapper = mapper;
        RepositoryDbSet = RepositoryDbContext.Set<TDomainEntity>();
    }


    public virtual async Task<IEnumerable<TDalEntity>> AllAsync()
    {
        return (await RepositoryDbSet.ToListAsync()).Select(x => Mapper.Map(x)!);
    }

    public virtual async Task<TDalEntity?> FindAsync(TKey id)
    {
        var res = await RepositoryDbSet.FindAsync(id);
        return Mapper.Map(res);
    }

    public virtual TDalEntity Add(TDalEntity entity)
    {
        var res = RepositoryDbSet.Add(Mapper.Map(entity)!).Entity;
        return Mapper.Map(res)!;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        var res = RepositoryDbSet.Update(Mapper.Map(entity)!).Entity;

        return Mapper.Map(res)!;
    }

    public async Task<TDalEntity?> UpdateAsync(TDalEntity entity)
    {
        var trackedProduct = await RepositoryDbSet.FindAsync(entity.Id);

        if (trackedProduct != null)
        {
            RepositoryDbContext.Entry(trackedProduct).CurrentValues.SetValues(entity);
        }

        return Mapper.Map(trackedProduct);
    }

    public virtual TDalEntity Remove(TDalEntity deletableEntity)
    {
        var entity = RepositoryDbSet.Find(deletableEntity.Id);


        var res = RepositoryDbSet.Remove(entity!).Entity;
        return Mapper.Map(res)!;
    }

    public virtual async Task<TDalEntity?> RemoveAsync(TKey id)
    {
        var entity = await RepositoryDbSet.FindAsync(id);

        if (entity != null)
        {
            var res = RepositoryDbSet.Remove(entity).Entity;
            return Mapper.Map(res);
        }

        return null;
    }
}