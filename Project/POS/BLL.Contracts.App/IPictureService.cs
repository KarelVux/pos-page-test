using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IPictureService : IBaseRepository<BllDto.Picture>,
    IPictureRepositoryCustom<BllDto.Picture>
{
    // add your custom service methods here
}