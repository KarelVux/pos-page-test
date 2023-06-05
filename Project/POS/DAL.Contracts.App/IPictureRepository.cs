using DAL.Contracts.Base;
using DAL.DTO;

namespace DAL.Contracts.App;

public interface IPictureRepository:  IBaseRepository<Picture>, IPictureRepositoryCustom<Picture>
{
    // add here custom methods for repo only
}

public interface IPictureRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
}