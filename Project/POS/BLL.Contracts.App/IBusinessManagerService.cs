using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IBusinessManagerService : IBaseRepository<BllDto.BusinessManager>,
    IBusinessManagerRepositoryCustom<BllDto.BusinessManager>
{
    // add your custom service methods here
    Task<BllDto.BusinessManager?> GetUserManagedBusiness(Guid businessId, Guid userId);
}