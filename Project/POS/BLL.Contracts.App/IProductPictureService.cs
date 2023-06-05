using DAL.Contracts.App;
using DAL.Contracts.Base;
using BllDto = BLL.DTO;

namespace BLL.Contracts.App;

public interface IProductPictureService : IBaseRepository<BllDto.ProductPicture>,
    IProductPictureRepositoryCustom<BllDto.ProductPicture>
{
    // add your custom service methods here
    Task<BllDto.ProductPicture?> FindViaPictureId(Guid pictureId);
    Task<BllDto.ProductPicture?> FindViaProductId(Guid productId);
}