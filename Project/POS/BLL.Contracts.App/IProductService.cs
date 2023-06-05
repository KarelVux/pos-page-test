using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IProductService : IBaseRepository<BllDto.Product>,
    IProductRepositoryCustom<BllDto.Product>
{
    // add your custom service methods here
}