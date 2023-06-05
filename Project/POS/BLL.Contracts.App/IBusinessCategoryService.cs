using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IBusinessCategoryService : IBaseRepository<BllDto.BusinessCategory>,
    IBusinessCategoryRepositoryCustom<BllDto.BusinessCategory>
{
    // add your custom service methods here
}