using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IProductCategoryService : IBaseRepository<BllDto.ProductCategory>,
    IPictureRepositoryCustom<BllDto.ProductCategory>
{
    // add your custom service methods here
}