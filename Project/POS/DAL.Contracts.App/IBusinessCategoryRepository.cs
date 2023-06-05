using DAL.Contracts.Base;
using DAL.DTO;

namespace DAL.Contracts.App;

public interface IBusinessCategoryRepository : IBaseRepository<BusinessCategory>, IBusinessCategoryRepositoryCustom<BusinessCategory>
{
    // add here custom methods for repo only
}

public interface IBusinessCategoryRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
}