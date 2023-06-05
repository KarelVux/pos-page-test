using BLL.Base;
using BLL.Contracts.App;
using Contracts.Base;
using DAL.Contracts.App;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Services;

public class BusinessPictureService :
    BaseEntityService<BllDto.BusinessPicture, DalDto.BusinessPicture, IBusinessPictureRepository>,
    IBusinessPictureService
{
    protected IAppUOW Uow;

    public BusinessPictureService(IAppUOW uow, IMapper<BllDto.BusinessPicture, DalDto.BusinessPicture> mapper)
        : base(uow.BusinessPictureRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BllDto.BusinessPicture>> AllAsyncWithInclude()
    {
        return (await Repository.AllAsyncWithInclude()).Select(e => Mapper.Map(e))!;
    }

    public async Task<BllDto.BusinessPicture?> FindAsyncWithInclude(Guid id)
    {
        return Mapper.Map(await Repository.FindAsyncWithInclude(id));
    }

    /// <summary>
    /// Find picture via picture id
    /// </summary>
    /// <param name="pictureId">Picture id</param>
    /// <returns>Searchable result</returns>
    public async Task<BllDto.BusinessPicture?> FindViaPictureId(Guid pictureId)
    {
        return Mapper.Map(await Repository.FindViaPictureId(pictureId));
    }

    /// <summary>
    /// Find picture via business id
    /// </summary>
    /// <param name="businessId">Business id</param>
    /// <returns>Searchable result</returns>
    public async Task<BllDto.BusinessPicture?> FindViaBusinessId(Guid businessId)
    {
        return Mapper.Map(await Repository.FindViaBusinessId(businessId));
    }
}