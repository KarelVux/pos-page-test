using DAL.Contracts.Base;
using DAL.DTO;

namespace DAL.Contracts.App;

public interface ISettlementRepository : IBaseRepository<Settlement>, ISettlementRepositoryCustom<Settlement>
{
    // add here custom methods for repo only
}

public interface ISettlementRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
}