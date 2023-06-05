using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IBusinessPictureService : IBaseRepository<BllDto.BusinessPicture>,
    IBusinessPictureRepositoryCustom<BllDto.BusinessPicture>
{
    // add your custom service methods here
    Task<BllDto.BusinessPicture?> FindViaPictureId(Guid pictureId);
    Task<BllDto.BusinessPicture?> FindViaBusinessId(Guid businessId);
}