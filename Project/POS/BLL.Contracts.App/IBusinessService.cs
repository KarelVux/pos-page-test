using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IBusinessService : IBaseRepository<BllDto.Business>,
    IBusinessRepositoryRepositoryCustom<BllDto.Business>
{
    // add your custom service methods here
    Task<BllDto.Business?> GetBusinessDetailsPageData(Guid id);
    Task<IEnumerable<BllDto.Business>> AllAsyncWithBusinessOwnerAndPicture(Guid userId);
}